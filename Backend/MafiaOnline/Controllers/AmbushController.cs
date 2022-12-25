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
    public class AmbushController : ControllerBase
    {
        private readonly IAmbushService _ambushService;

        public AmbushController(IAmbushService ambushService)
        {
            _ambushService = ambushService;
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
            await _ambushService.MoveToArrangeAmbush(request);
            return Ok();
        }
    }
}
