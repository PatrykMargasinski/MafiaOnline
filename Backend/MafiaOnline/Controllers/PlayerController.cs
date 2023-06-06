using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
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
            user.ApiUrl = Request.Host.Value;
            await _playerService.Register(user);
            return Ok();
        }

        [Authorize(Roles = "Player,Administrator")]
        [HttpPost("/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest user)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            user.UserName = jwtDatas.UserName;
            user.ApiUrl = Request.Host.Value;
            await _playerService.ChangePassword(user);
            return Ok();
        }

        [Authorize(Roles = "Player,Administrator")]
        [HttpPost("/deleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest user)
        {
            var jwtDatas = _tokenUtils.DecodeToken(Request.Headers["Authorization"]);
            user.UserName = jwtDatas.UserName;
            await _playerService.DeleteAccount(user);
            return Ok();
        }

        [HttpGet("/activate")]
        public async Task<IActionResult> Activate([FromQuery] ActivationToken activationToken)
        {
            var message = await _playerService.Activate(activationToken);
            return Ok(message);
        }

        [HttpPost("/resetPasswordCode")]
        public async Task<IActionResult> CreateResetPasswordCode([FromBody] CreateResetPasswordCodeRequest request)
        {
            await _playerService.CreateAndSendResetPasswordCode(request);
            return Ok();
        }
        [HttpPost("/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _playerService.ResetPassword(request);
            return Ok();
        }

        [HttpPost("/refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] Tokens tokens)
        {
            var newTokens = await _playerService.RefreshToken(tokens);
            return Ok(newTokens);
        }
    }
}
