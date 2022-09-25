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

        [HttpPost("start")]
        public async Task<IActionResult> StartMission(long agentId, long missionId)
        {

            await _missionService.DoMission(agentId, missionId);
            return Ok();
        }
    }
}
