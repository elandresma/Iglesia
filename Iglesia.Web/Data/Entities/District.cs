﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iglesia.Web.Data.Entities
{
    public class District
    {
        public int Id { get; set; }

        [Display(Name = "District")]
        [MaxLength(50)]
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
