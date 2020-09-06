﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iglesia.Common.Entities
{
    public class District
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        public ICollection<Church> Churches { get; set; }

        [DisplayName("Churches Number")]
        public int ChurchesNumber => Churches == null ? 0 : Churches.Count;

        [JsonIgnore]
        [NotMapped]
        public int IdRegion { get; set; }


        [JsonIgnore]
        public Region Region { get; set; }

    }

}
