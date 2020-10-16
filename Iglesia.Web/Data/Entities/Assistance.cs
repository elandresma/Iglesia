using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Iglesia.Web.Data.Entities
{
    public class Assistance
    {
        public int Id { get; set; }


        public User User { get; set; }

        [JsonIgnore]
        public Meeting Meeting { get; set; }

        [Display(Name = "Is Present")]
        public bool IsPresent { get; set; }

    }
}
