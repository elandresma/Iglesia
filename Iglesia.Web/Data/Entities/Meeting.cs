﻿using Iglesia.Common.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iglesia.Web.Data.Entities
{
    public class Meeting
    {
        public int Id { get; set; }


        public Church Church { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();

        
        public ICollection<Assistance> Assistances { get; set; }

        [Display(Name = "# Assistances")]
        public int AssistancesNumber => Assistances == null ? 0 : Assistances.Count;

    }
}
