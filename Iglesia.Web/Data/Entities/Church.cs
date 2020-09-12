using Iglesia.Common.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iglesia.Web.Data.Entities
{
    public class Church
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        [NotMapped]
        public int IdDistrict { get; set; }


        [JsonIgnore]
        public District District { get; set; }

        public ICollection<User> User { get; set; }


    }

}
