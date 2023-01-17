using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MafiaOnline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Player")]
    public class HeadquartersController : ControllerBase
    {
        private readonly IHeadquartersService _headquartersService;

        public HeadquartersController(IHeadquartersService mapService)
        {
            _headquartersService = mapService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHeadquartersDetails(long id)
        {
            var map = await _headquartersService.GetHeadquartersDetailsByMapElementId(id);
            return new JsonResult(map);
        }
        
    }
}
