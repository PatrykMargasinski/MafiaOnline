using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.BusinessLogic.Utils;
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
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;
        private readonly ITokenUtils _tokenUtils;

        public MapController(IMapService mapService, ITokenUtils tokenUtils)
        {
            _mapService = mapService;
            _tokenUtils = tokenUtils;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateMap(long x, long y, long size)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var map = await _mapService.GenerateMap(x, y, size, jwtDatas.BossId);
            return new JsonResult(map);
        }


        [HttpGet("edge")]
        public async Task<IActionResult> GetEdgeForBoss()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var edge = await _mapService.GetEdgeForBoss(jwtDatas.BossId);
            return new JsonResult(edge);
        }

    }
}
