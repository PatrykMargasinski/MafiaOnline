﻿using MafiaOnline.BusinessLogic.Entities;
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

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(SendMessageRequest request)
        {
            await _messageService.SendMessage(request);
            return Ok();
        }

        [HttpGet("toBoss/{bossId}")]
        public async Task<IActionResult> GetMessagesToBoss(long bossId)
        {
            var messages = await _messageService.GetMessagesToBoss(bossId);
            return Ok(messages);
        }

        [HttpGet("fromBoss/{bossId}")]
        public async Task<IActionResult> GetMessagesFromBoss(long bossId)
        {
            var messages = await _messageService.GetMessagesFromBoss(bossId);
            return Ok(messages);
        }

        [HttpGet("content/{messageId}")]
        public async Task<IActionResult> GetMessageContent(long messageId)
        {
            var message = await _messageService.GetMessageContent(messageId);
            return Ok(message);
        }

        [HttpGet("report/{bossId}")]
        public async Task<IActionResult> GetReports(long bossId)
        {
            var messages = await _messageService.GetReports(bossId);
            return Ok(messages);
        }
    }
}
