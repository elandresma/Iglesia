using Iglesia.Common.Enum;
using Iglesia.Common.Requests;
using Iglesia.Web.Data;
using Iglesia.Web.Data.Entities;
using Iglesia.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Prng.Drbg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChurchesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ChurchesController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("GetUsersByChurch")]
        public async Task<IActionResult> GetUsersByChurch([FromBody] EmailRequest request)
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

            return Ok(await _context.Users
                                .Include(u => u.Church)
                                .Include(u => u.Assistances)
                                .ThenInclude(A => A.Meeting)
                                .Where(c => c.Church.Id == user.Church.Id && c.UserType.ToString().Equals(UserType.User.ToString()))
                                .ToListAsync());

        }
    }
}
