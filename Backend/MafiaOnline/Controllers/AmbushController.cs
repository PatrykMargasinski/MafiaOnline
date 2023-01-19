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

namespace MafiaOnline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Player,Administrator")]
    public class AmbushController : ControllerBase
    {
        private readonly IAmbushService _ambushService;
        private readonly ITokenUtils _tokenUtils;

        public AmbushController(IAmbushService ambushService, ITokenUtils tokenUtils)
        {
            _ambushService = ambushService;
            _tokenUtils = tokenUtils;
        }

        [HttpGet]
        public async Task<IActionResult> GetAmbushDetailsByMapElementId(long mapElementId)
        {
            var ambush = await _ambushService.GetAmbushDetailsByMapElementId(mapElementId);
            return new JsonResult(ambush);
        }

        [HttpPost("arrange")]
        public async Task<IActionResult> MoveToArrangeAmbush([FromBody]ArrangeAmbushRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            await _ambushService.MoveToArrangeAmbush(request);
            return Ok();
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelAmbush([FromBody] CancelAmbushRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            await _ambushService.CancelAmbush(request);
            return Ok();
        }

        [HttpPost("attack")]
        public async Task<IActionResult> MoveToAttackAmbush([FromBody] AttackAmbushRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            await _ambushService.MoveToAttackAmbush(request);
            return Ok();
        }
    }
}
