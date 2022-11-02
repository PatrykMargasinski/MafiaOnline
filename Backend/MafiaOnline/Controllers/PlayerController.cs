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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            var tokens = await _playerService.Login(user);
            return Ok(tokens);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            await _playerService.Register(user);
            return Ok();
        }

        [HttpPost("/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest user)
        {
            await _playerService.ChangePassword(user);
            return Ok();
        }

        [HttpPost("/deleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest user)
        {
            await _playerService.DeleteAccount(user);
            return Ok();
        }
    }
}
