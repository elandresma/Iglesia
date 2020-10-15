﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Iglesia.Common.Responses
{
   public class MeetingResponse
    {
        public int Id { get; set; }


        public ChurchResponse Church { get; set; }


        public DateTime Date { get; set; }


        public DateTime DateLocal => Date.ToLocalTime();


        public ICollection<AssistancesResponse> Assistances { get; set; }


        public int AssistancesNumber => Assistances == null ? 0 : Assistances.Count;
    }
}
