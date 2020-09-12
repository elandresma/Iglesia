using Iglesia.Web.Data;
using Iglesia.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Iglesia.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetComboChurches(int districtId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            District district = _context.Districts
                .Include(d => d.Churches)
                .FirstOrDefault(d => d.Id == districtId);
            if (district != null)
            {
                list = district.Churches.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.Id}"
                })
                    .OrderBy(t => t.Text)
                    .ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a church...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboRegions()
        {
            List<SelectListItem> list = _context.Regions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a region...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboProfessions()
        {
            List<SelectListItem> list = _context.Professions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a profession...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDistricts(int regionId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Region region = _context.Regions
                .Include(c => c.Districts)
                .FirstOrDefault(c => c.Id == regionId);
            if (region != null)
            {
                list = region.Districts.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = $"{t.Id}"
                })
                    .OrderBy(t => t.Text)
                    .ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a district...]",
                Value = "0"
            });

            return list;
        }
    }
}
