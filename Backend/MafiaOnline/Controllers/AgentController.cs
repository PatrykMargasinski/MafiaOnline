﻿using MafiaOnline.BusinessLogic.Services;
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
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;
        private readonly ITokenUtils _tokenUtils;

        public AgentController(IAgentService agentService, ITokenUtils tokenUtils)
        {
            _agentService = agentService;
            _tokenUtils = tokenUtils;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgents()
        {
            var agents = await _agentService.GetAllAgents();
            return new JsonResult(agents);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAgents()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var agents = await _agentService.GetActiveAgents(jwtDatas.BossId);
            return new JsonResult(agents);
        }

        [HttpGet("cancelAmbush")]
        public async Task<IActionResult> CancelAgentAmbush(long agentId)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            await _agentService.CancelAgentAmbush(agentId, jwtDatas.BossId);
            return new JsonResult("Ambush canceled");
        }

        [HttpGet("moving")]
        public async Task<IActionResult> GetMovingAgents()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var agents = await _agentService.GetMovingAgents(jwtDatas.BossId);
            return new JsonResult(agents);
        }

        [HttpGet("onMission")]
        public async Task<IActionResult> GetAgentsOnMission()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var agents = await _agentService.GetAgentsOnMission(jwtDatas.BossId);
            return new JsonResult(agents);
        }

        [HttpGet("forSale")]
        public async Task<IActionResult> GetAgentsForSale()
        {
            var agents = await _agentService.GetAgentsForSale();
            return new JsonResult(agents);
        }

        [HttpGet("ambushing")]
        public async Task<IActionResult> GetAmbushingAgents()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var agents = await _agentService.GetAmbushingAgents(jwtDatas.BossId);
            return new JsonResult(agents);
        }

        [HttpPost("query")]
        public async Task<IActionResult> GetAgentsByQuery([FromBody]AgentQuery query)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            query.BossId = jwtDatas.BossId;
            var agents = await _agentService.GetAgentsByQuery(query);
            return new JsonResult(agents);
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAgents()
        {
            await _agentService.RefreshAgents();
            return Ok();
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
    }
}
