using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.BusinessLogic.Entities;
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
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgents()
        {
            var agents = await _agentService.GetAllAgents();
            return new JsonResult(agents);
        }

        [HttpGet("{bossId}")]
        public async Task<IActionResult> GetBossAgents(long bossId)
        {
            var agents = await _agentService.GetBossAgents(bossId);
            return new JsonResult(agents);
        }

        [HttpGet("active/{bossId}")]
        public async Task<IActionResult> GetActiveAgents(long bossId)
        {
            var agents = await _agentService.GetActiveAgents(bossId);
            return new JsonResult(agents);
        }

        [HttpGet("moving/{bossId}")]
        public async Task<IActionResult> GetMovingAgents(long bossId)
        {
            var agents = await _agentService.GetMovingAgents(bossId);
            return new JsonResult(agents);
        }

        [HttpGet("onMission/{bossId}")]
        public async Task<IActionResult> GetAgentsOnMission(long bossId)
        {
            var agents = await _agentService.GetAgentsOnMission(bossId);
            return new JsonResult(agents);
        }

        [HttpGet("forSale")]
        public async Task<IActionResult> GetAgentsForSale()
        {
            var agents = await _agentService.GetAgentsForSale();
            return new JsonResult(agents);
        }

        [HttpPost("dismiss")]
        public async Task<IActionResult> DismissAgent([FromBody] DismissAgentRequest request)
        {
            var agents = await _agentService.DismissAgent(request);
            return new JsonResult(agents);
        }

        [HttpPost("recruit")]
        public async Task<IActionResult> RecruitAgent([FromBody] RecruitAgentRequest request)
        {
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
            await _agentService.SendToPatrol(request);
            return Ok();
        }
    }
}
