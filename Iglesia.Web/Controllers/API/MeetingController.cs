using Iglesia.Common.Enum;
using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using Iglesia.Web.Data;
using Iglesia.Web.Data.Entities;
using Iglesia.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingController: ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public MeetingController( DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        [HttpPost]
        public IActionResult GetMeeting([FromBody] MeetingRequest request)
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
            return Ok(_context.Meetings
                .Include(m => m.Church)
                .Include(m => m.Assistances)
                .ThenInclude(a => a.User).Where(m => m.Church.Id == request.ChurchId));
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateMeetingAsync([FromBody] MeetingRequest request)
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

            Church church = _context.Churches.FirstOrDefault(ch => ch.Id == request.ChurchId);
            Meeting meeting = new Meeting
            {
                Date=request.Date,
                Church = church
            };

            _context.Add(meeting);
            await _context.SaveChangesAsync();
            await CreateAssistancesAsync(request.ChurchId);
            return Ok(new Response { IsSuccess = true });
        }
        public async Task CreateAssistancesAsync(int ChurchID) 
        {
           Meeting meeting= _context.Meetings.LastOrDefault();
           List<User>Users = await _context.Users
                               .Include(u => u.Church)
                               .Where(c => c.Church.Id == ChurchID && c.UserType.ToString().Equals(UserType.User.ToString()))
                               .ToListAsync();

            foreach (var user in Users)
            {
                Assistance assistance = new Assistance
                {
                    User=user,
                    Meeting=meeting,
                    IsPresent=false
                };
                _context.Add(assistance);
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        [Route("GetAssistances")]
        public async Task<IActionResult> GetAssistancesAsync([FromBody] EmailRequest request)
        {
          var resultado= (await _context.Assistances
                .Include(a => a.Meeting)
                .Where(a => a.User.Email == request.Email)
                .ToListAsync());

           
            return Ok(_converterHelper.ToAssistancesResponseList(resultado));

        }
        [HttpPut]
        [Route("UpdateAssistances")]
        public async Task<IActionResult> UpdateAssistances([FromBody] AssistancesRequest request)
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

            foreach (var item in request.AssistancesList)
            {
                Assistance assitance = new Assistance
                {
                    Id = item.Id,
                    IsPresent = item.IsPresent
                };

                _context.Update(assitance);
                await _context.SaveChangesAsync();
            }

            return Ok(new Response { IsSuccess = true });
        }
    }
}
