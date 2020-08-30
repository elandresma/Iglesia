using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Iglesia.Common.Entities;
using Iglesia.Web.Data;

namespace Iglesia.Web.Controllers
{
    public class RegionsController : Controller
    {
        private readonly DataContext _context;

        public RegionsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Regions
                .Include(d => d.Districts)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regions
                .Include(d => d.Districts)
                .ThenInclude(c => c.Churches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Region region)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(region);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a region with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(region);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return View(region);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Region region)
        {
            if (id != region.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a region with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(region);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Region region = await _context.Regions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> AddDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Region region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            District model = new District { IdRegion = region.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDistrict(District district)
        {
            if (ModelState.IsValid)
            {
                Region region= await _context.Regions
                    .Include(c => c.Districts)
                    .FirstOrDefaultAsync(c => c.Id == district.IdRegion);

                if (region == null)
                {
                    return NotFound();
                }

                try
                {
                    district.Id = 0;
                    region.Districts.Add(district);
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{region.Id}");

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a district with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(district);
        }

    }
}
