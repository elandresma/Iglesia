using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Iglesia.Web.Data.Entities
{
    public class Region
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        public ICollection<District> Districts { get; set; }

        [Display(Name = "# Districts")]
        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;

        [Display(Name = "# Churches")]
        public int ChurchesNumber => Districts == null ? 0 : Districts.Sum(d => d.ChurchesNumber);

    }
}
