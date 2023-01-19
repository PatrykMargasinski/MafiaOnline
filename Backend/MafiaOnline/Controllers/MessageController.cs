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
    [Authorize(Roles = "Player,Administrator")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ITokenUtils _tokenUtils;

        public MessageController(IMessageService messageService, ITokenUtils tokenUtils)
        {
            _messageService = messageService;
            _tokenUtils = tokenUtils;
        }


        [HttpGet("toBoss/{bossId}")]
        public async Task<IActionResult> GetMessagesToBoss()
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var messages = await _messageService.GetMessagesToBoss(jwtDatas.BossId);
            return Ok(messages);
        }

        [HttpGet("bossMessagesTo")]
        public async Task<IActionResult> GetAllMessagesToInRange(int? fromRange, int? toRange, string bossNameFilter = "", bool onlyUnseen = false)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var messages = await _messageService.GetAllMessagesToInRange(jwtDatas.BossId, fromRange.Value, toRange.Value, bossNameFilter, onlyUnseen);
            return new JsonResult(messages);
        }


        [HttpGet("reportsTo")]
        public async Task<IActionResult> GetAllReportsToInRange(int? fromRange, int? toRange, bool onlyUnseen = false)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            var messages = await _messageService.GetAllReportsToInRange(jwtDatas.BossId, fromRange.Value, toRange.Value, onlyUnseen);
            return new JsonResult(messages);
        }

        [HttpGet("fromBoss/{bossId}")]
        public async Task<IActionResult> GetMessagesFromBoss(long bossId)
        {
            var messages = await _messageService.GetMessagesFromBoss(bossId);
            return Ok(messages);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            await _messageService.SendMessage(request);
            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(long messageId)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            _messageService.DeleteMessage(messageId, jwtDatas.BossId);
            return Ok();
        }

        [HttpDelete("messages")]
        public async Task<IActionResult> DeleteMessages(string messageIds)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            _messageService.DeleteMessages(messageIds, jwtDatas.BossId);
            return Ok();
        }

        [HttpGet("bossMessageCount")]
        public IActionResult CountMessages(long bossId)
        {
            var count = _messageService.CountMessages(bossId);
            return Ok(count);
        }

        [HttpGet("reportCount")]
        public IActionResult CountReports(long bossId)
        {
            var count = _messageService.CountReports(bossId);
            return Ok(count);
        }

        [HttpPost("seen")]
        public IActionResult SetSeen([FromBody] SetSeenRequest request)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            request.BossId = jwtDatas.BossId;
            var count = _messageService.SetSeen(request);
            return Ok(count);
        }
    }
}
