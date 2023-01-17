using MafiaOnline.BusinessLogic.Entities;
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
    public class MissionController : ControllerBase
    {
        private readonly IMissionService _missionService;
        private readonly ITokenUtils _tokenUtils;

        public MissionController(IMissionService missionService, ITokenUtils tokenUtils)
        {
            _missionService = missionService;
            _tokenUtils = tokenUtils;
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
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
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
        public async Task<IActionResult> GetPerformingMissions()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var missions = await _missionService.GetPerformingMissions(jwtDatas.BossId);
            return new JsonResult(missions);
        }
    }
}
