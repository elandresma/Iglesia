using Iglesia.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Iglesia.Common.Requests
{
   public class UserRequest
    {

        [Required]
        [EmailAddress]
        public string Username { get; set; }


        [Required]
        public string Document { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Address { get; set; }

        public int ProfessionId { get; set; }

        public int ChurchId { get; set; }

    }
}
