﻿using Iglesia.Common.Entities;
using Iglesia.Common.Enum;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Web.Data;
using Iglesia.Web.Data.Entities;
using Iglesia.Web.Helpers;
using Iglesia.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Iglesia.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;



        public AccountController(IUserHelper userHelper, IConfiguration configuration,DataContext context, IBlobHelper blobHelper,
    IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _context = context;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
        }

        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        Claim[] claims = new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(99),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("PutUser")]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            User user = await _userHelper.GetUserAsync(request.Username);
            if (user == null)
            {
                return NotFound("Error001");
            }

            Guid imageId = user.ImageId;

            if (request.ImageArray != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.ImageArray, "users");
            }

            Church church = await _context.Churches.FindAsync(request.ChurchId);
            if (church == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error004"
                });
            }


            user.Address = request.Address;
            user.Church = await _context.Churches.FindAsync(request.ChurchId);
            user.Document = request.Document;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            user.ImageId = imageId;
            user.Profession = await _context.Professions.FindAsync(request.ProfessionId);


            IdentityResult respose = await _userHelper.UpdateUserAsync(user);

            if (!respose.Succeeded) 
            { 
                return BadRequest(respose.Errors.FirstOrDefault().Description); 
            }
            User updatedUser = await _userHelper.GetUserAsync(request.Username);
            return Ok(updatedUser);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            User user = await _userHelper.GetUserAsync(request.Username);
            if (user != null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error003"
                });
            }


            Church church = await _context.Churches.FindAsync(request.ChurchId);
            if (church == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error004"
                });
            }

            Profession profession = await _context.Professions.FindAsync(request.ProfessionId);
            if (profession == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error002"
                });
            }

            Guid imageId = Guid.Empty;

            if (request.ImageArray != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.ImageArray, "users");
            }

            user = new User
            {
                Address = request.Address,
                Document = request.Document,
                Email = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.Username,
                ImageId = imageId,
                UserType = UserType.User,
                Church = church,
                Profession = profession
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            User userNew = await _userHelper.GetUserAsync(request.Username);
            await _userHelper.AddUserToRoleAsync(userNew, user.UserType.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.Username, "Email Confirmation", $"<h1>Email Confirmation</h1>" +
                $"To confirm your email please click on the link<p><a href = \"{tokenLink}\">Confirm Email</a></p>");

            return Ok(new Response { IsSuccess = true });
        }

        [HttpPost]
        [Route("RecoverPasswordAPI")]
        public async Task<IActionResult> RecoverPasswordAPI([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            User user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error001"
                });
            }

            string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, "Password Recover", $"<h1>Password Recover</h1>" +
                $"Click on the following link to change your password:<p>" +
                $"<a href = \"{link}\">Change Password</a></p>");

            return Ok(new Response { IsSuccess = true });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePasswordAPI")]
        public async Task<IActionResult> ChangePasswordAPI([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            User user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var message = result.Errors.FirstOrDefault().Description;
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error005"
                });
            }

            return Ok(new Response { IsSuccess = true });
        }

        [HttpGet]
        [Route("GetProfesionsAPI")]
        public async Task<IActionResult> GetProfesionsAPI()
        {
            return Ok(await _context.Professions.ToListAsync());
        }
    }
}

