using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iglesia.Common.Responses
{
   public class RegionResponse
    {
        public int Id { get; set; }


        public string Name { get; set; }

        public ICollection<DistrictResponse> Districts { get; set; }


        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;


        public int ChurchesNumber => Districts == null ? 0 : Districts.Sum(d => d.ChurchesNumber);
    }
}
