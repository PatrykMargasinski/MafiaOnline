using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Entities.Mission;
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
    public class MissionController : ControllerBase
    {
        private readonly IMissionService _missionService;

        public MissionController(IMissionService missionService)
        {
            _missionService = missionService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartMission([FromBody] StartMissionRequest request)
        {

            await _missionService.StartMission(request);
            return Ok();
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableMissions()
        {
            var agents = await _missionService.GetAvailableMissions();
            return new JsonResult(agents);
        }

        [HttpGet("performing")]
        public async Task<IActionResult> GetPerformingMissions(long bossId)
        {
            var missions = await _missionService.GetPerformingMissions(bossId);
            return new JsonResult(missions);
        }
    }
}
