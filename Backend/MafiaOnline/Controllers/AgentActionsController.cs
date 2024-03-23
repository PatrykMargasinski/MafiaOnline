using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Entities.Queries;
using Microsoft.EntityFrameworkCore;

namespace MafiaOnline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Player, Administrator")]
    public class AgentActionsController : ControllerBase
    {
        private readonly IAgentActionsService _agentService;
        private readonly ITokenUtils _tokenUtils;

        public AgentActionsController(IAgentActionsService movingAgentService, ITokenUtils tokenUtils)
        {
            _agentService = movingAgentService;
            _tokenUtils = tokenUtils;
        }


        [HttpGet("cancelAmbush")]
        public async Task<IActionResult> CancelAgentAmbush(long agentId)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            await _agentService.CancelAgentAmbush(agentId, jwtDatas.BossId);
            return new JsonResult("Ambush canceled");
        }

        [HttpPost("patrol")]
        public async Task<IActionResult> SendToPatrol(PatrolRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            await _agentService.SendToPatrol(request);
            return Ok();
        }

        [HttpGet("position")]
        public async Task<IActionResult> GetAgentPosition([FromQuery] long agentId)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var point = await _agentService.GetAgentPosition(agentId, jwtDatas.BossId);
            return Ok(point);
        }


        [HttpPost("dismiss")]
        public async Task<IActionResult> DismissAgent([FromBody] DismissAgentRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            var agents = await _agentService.DismissAgent(request);
            return new JsonResult(agents);
        }

        [HttpPost("recruit")]
        public async Task<IActionResult> RecruitAgent([FromBody] RecruitAgentRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            var agent = await _agentService.RecruitAgent(request);
            return new JsonResult($"Agent {agent.FirstName} {agent.LastName} is at your service.");
        }
    }
}
