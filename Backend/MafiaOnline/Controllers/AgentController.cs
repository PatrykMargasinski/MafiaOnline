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
    }
}
