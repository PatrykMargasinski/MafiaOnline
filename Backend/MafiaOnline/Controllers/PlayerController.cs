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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ITokenUtils _tokenUtils;

        public PlayerController(IPlayerService playerService, ITokenUtils tokenUtils)
        {
            _playerService = playerService;
            _tokenUtils = tokenUtils;
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

        [Authorize(Roles = "Player,Administrator")]
        [HttpPost("/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest user)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            user.PlayerId = jwtDatas.PlayerId;
            await _playerService.ChangePassword(user);
            return Ok();
        }

        [Authorize(Roles = "Player,Administrator")]
        [HttpPost("/deleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest user)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            user.PlayerId = jwtDatas.PlayerId;
            await _playerService.DeleteAccount(user);
            return Ok();
        }
    }
}
