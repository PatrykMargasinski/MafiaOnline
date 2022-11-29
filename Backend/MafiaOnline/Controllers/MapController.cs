using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Entities;
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
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;

        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateMap(long x, long y, long size)
        {
            var map = await _mapService.GenerateMap(x, y, size);
            return new JsonResult(map);
        }


        [HttpGet("edge")]
        public async Task<IActionResult> GetEdgeForBoss(long bossId)
        {
            var edge = await _mapService.GetEdgeForBoss(bossId);
            return new JsonResult(edge);
        }

    }
}
