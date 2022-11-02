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
    public class BossController : ControllerBase
    {
        private readonly IBossService _bossService;

        public BossController(IBossService bossService)
        {
            _bossService = bossService;
        }

        [HttpGet("best")]
        public async Task<IActionResult> GetBestBosses(int? from, int? to)
        {
            if (!from.HasValue || !to.HasValue)
                throw new Exception("No range was specified");
            var bosses = await _bossService.GetBestBosses(from.Value, to.Value);
            return new JsonResult(bosses);
        }

        [HttpGet("{bossId}")]
        public async Task<IActionResult> GetBossDatas(long bossId)
        {
            var boss = await _bossService.GetBossDatas(bossId);
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
