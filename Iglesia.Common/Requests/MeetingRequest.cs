using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Iglesia.Common.Requests
{
    public class MeetingRequest
    {
        [Required]
        public int ChurchId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; } 



    }
}
