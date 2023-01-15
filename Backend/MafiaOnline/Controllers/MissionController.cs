using MafiaOnline.BusinessLogic.Entities;
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

        [HttpGet("mapElement")]
        public async Task<IActionResult> GetMissionByMapElement(long mapElementId)
        {
            var missions = await _missionService.GetMissionByMapElement(mapElementId);
            return new JsonResult(missions);
        }

        [HttpPost("move")]
        public async Task<IActionResult> MoveOnMission([FromBody] StartMissionRequest request)
        {
            await _missionService.MoveOnMission(request);
            return Ok();
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartMission([FromBody] StartMissionRequest request)
        {

            await _missionService.StartMission(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMissionById(long missionId)
        {
            var mission = await _missionService.GetMissionById(missionId);
            return new JsonResult(mission);
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
