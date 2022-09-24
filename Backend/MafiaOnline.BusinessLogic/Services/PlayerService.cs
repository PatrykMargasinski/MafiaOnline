using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IPlayerService
    {
        Task<Tokens> Login(LoginRequest user);
        Task<Player> Register(RegisterRequest user);
        Task<Player> ChangePassword(ChangePasswordRequest user);
        Task<bool> DeleteAccount(DeleteAccountRequest user);
    }

    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurityUtils _securityUtils;
        private readonly ITokenUtils _tokenUtils;

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper, 
            ISecurityUtils securityUtils, ITokenUtils tokenUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
            _tokenUtils = tokenUtils;
        }

        public async Task<Tokens> Login(LoginRequest user)
        {
            if (user == null)
            {
                throw new Exception("There is no data");
            }

            IList<string> errors = new List<string>();
            if (string.IsNullOrEmpty(user.Nick)) errors.Add("Nick is empty");
            if (string.IsNullOrEmpty(user.Password)) errors.Add("Password is empty");

            if (errors.Count > 0)
            {
                throw new Exception(string.Join('\n', errors));
            }

            Player player = await _unitOfWork.Players.GetByNick(user.Nick);
            if (player == null)
            {
                throw new Exception("There is no player with such nick");
            }

            if (_securityUtils.VerifyPassword(player, user.Password) == false)
            {
                throw new Exception("Wrong password");
            }

            var token = _tokenUtils.CreateToken(player);
            var refreshToken = _tokenUtils.GenerateRefreshToken();
            player.RefreshToken = refreshToken;
            player.RefreshTokenExpiryTime = DateTime.Now.AddHours(1);
            _unitOfWork.Commit();

            return new Tokens()
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<Player> Register(RegisterRequest user)
        {
            throw new NotImplementedException();
        }

        public async Task<Player> ChangePassword(ChangePasswordRequest user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAccount(DeleteAccountRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
