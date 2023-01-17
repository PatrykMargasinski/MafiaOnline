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
    public class BossController : ControllerBase
    {
        private readonly IBossService _bossService;
        private readonly ITokenUtils _tokenUtils;

        public BossController(IBossService bossService, ITokenUtils tokenUtils)
        {
            _bossService = bossService;
            _tokenUtils = tokenUtils;
        }

        [HttpGet("best")]
        public async Task<IActionResult> GetBestBosses(int from, int to)
        {
            var bosses = await _bossService.GetBestBosses(from, to);
            return new JsonResult(bosses);
        }

        [HttpGet("datas")]
        public async Task<IActionResult> GetBossDatas()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var boss = await _bossService.GetBossDatas(jwtDatas.BossId);
            return new JsonResult(boss);
        }

        [HttpGet("similarNames")]
        public async Task<IActionResult> GetSimilarBossFullNames(string startingWith)
        {
            var boss = await _bossService.GetSimilarBossFullNames(startingWith);
            return new JsonResult(boss);
        }
    }
}
