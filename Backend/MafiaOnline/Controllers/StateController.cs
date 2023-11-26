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
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly ITokenUtils _tokenUtils;

        public StateController(IStateService stateService, ITokenUtils tokenUtils)
        {
            _stateService = stateService;
            _tokenUtils = tokenUtils;
        }

        [HttpGet("agent")]
        public async Task<IActionResult> GetAvailableAgentStates()
        {
            var token = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var states = await _stateService.GetAvailableAgentStates(token.BossId);
            return new JsonResult(states);
        }
    }
}
