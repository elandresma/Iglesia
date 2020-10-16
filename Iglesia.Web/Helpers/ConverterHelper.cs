using Iglesia.Common.Responses;
using Iglesia.Web.Data;
using Iglesia.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;


        public ConverterHelper(DataContext context)
        {
            _context = context;

        }
        public List<AssistancesResponse> ToAssistancesResponseList(List<Assistance> model)
        {
            if (model != null)
            {
                List<AssistancesResponse> Assistance = new List<AssistancesResponse>();
                foreach (Assistance assistance in model)
                {
                    Assistance.Add(new AssistancesResponse
                    {
                        User = null,
                        Meeting = ToMeetingResponse(assistance.Meeting),
                        IsPresent= assistance.IsPresent,
                        Id = assistance.Id

                    });
                }
                return Assistance;
            }
            else
            {
                return null;
            }
        }
        public MeetingResponse ToMeetingResponse(Meeting meeting)
        {
            return new MeetingResponse
            {
                Date = meeting.Date,
                Church = null,
                Assistances = null,
                Id =meeting.Id
               
            };
        }
    }
}
