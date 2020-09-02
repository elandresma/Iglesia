using Iglesia.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Controllers.API
{

    [ApiController]
    [Route("api/[controller]")]

    public class RegionsController : ControllerBase
    {
        private readonly DataContext _context;

        public RegionsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRegions()
        {
            return Ok(_context.Regions
                .Include(r => r.Districts)
                .ThenInclude(d => d.Churches));
        }
    }

}

