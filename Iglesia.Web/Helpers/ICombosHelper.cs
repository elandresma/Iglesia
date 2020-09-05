using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Iglesia.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboProfessions();
        IEnumerable<SelectListItem> GetComboRegions();

        IEnumerable<SelectListItem> GetComboDistricts(int RegionId);

        IEnumerable<SelectListItem> GetComboChurches(int DistrictId);

    }
}
