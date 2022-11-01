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
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }


        [HttpGet("toBoss/{bossId}")]
        public async Task<IActionResult> GetMessagesToBoss(long bossId)
        {
            var messages = await _messageService.GetMessagesToBoss(bossId);
            return Ok(messages);
        }

        [HttpGet("bossMessagesTo")]
        public async Task<IActionResult> GetAllMessagesToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter = "", bool onlyUnseen = false)
        {
            var messages = await _messageService.GetAllMessagesToInRange(bossId, fromRange.Value, toRange.Value, bossNameFilter, onlyUnseen);
            return new JsonResult(messages);
        }


        [HttpGet("reportsTo")]
        public async Task<IActionResult> GetAllReportsToInRange(long bossId, int? fromRange, int? toRange, string bossNameFilter = "", bool onlyUnseen = false)
        {
            var messages = await _messageService.GetAllReportsToInRange(bossId, fromRange.Value, toRange.Value, onlyUnseen);
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
            await _messageService.SendMessage(request);
            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(long messageId)
        {
            _messageService.DeleteMessage(messageId);
            return Ok();
        }

        [HttpDelete("messages")]
        public async Task<IActionResult> DeleteMessages(string messageIds)
        {
            _messageService.DeleteMessages(messageIds);
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

        [HttpGet("seen")]
        public IActionResult SetSeen(long messageId)
        {
            var count = _messageService.SetSeen(messageId);
            return Ok(count);
        }
    }
}
