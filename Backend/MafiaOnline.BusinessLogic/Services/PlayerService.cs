using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
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
        Task Register(RegisterRequest user);
        Task ChangePassword(ChangePasswordRequest user);
        Task<bool> DeleteAccount(DeleteAccountRequest user);
    }

    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurityUtils _securityUtils;
        private readonly ITokenUtils _tokenUtils;
        private readonly IBasicUtils _basicUtils;
        private readonly IPlayerValidator _playerValidator;

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper, 
            ISecurityUtils securityUtils, ITokenUtils tokenUtils, 
            IBasicUtils basicUtils, IPlayerValidator playerValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
            _tokenUtils = tokenUtils;
            _basicUtils = basicUtils;
            _playerValidator = playerValidator;
        }

        public async Task<Tokens> Login(LoginRequest request)
        {
            await _playerValidator.ValidateLogin(request);
            Player player = await _unitOfWork.Players.GetByNick(request.Nick);
            var token = _tokenUtils.CreateToken(player);
            var refreshToken = _tokenUtils.GenerateRefreshToken();
            player.RefreshToken = refreshToken;
            player.RefreshTokenExpiryTime = DateTime.Now.AddHours(1);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(player.BossId);
            boss.LastSeen = DateTime.Now;
            _unitOfWork.Commit();

            return new Tokens()
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task Register(RegisterRequest request)
        {
            await _playerValidator.ValidateRegister(request);

            Boss boss = new Boss()
            {
                FirstName = _basicUtils.UppercaseFirst(request.BossFirstName),
                LastName = _basicUtils.UppercaseFirst(request.BossLastName),
                Money = 5000
            };
            _unitOfWork.Bosses.Create(boss);
            Player player = new Player()
            {
                Nick = request.Nick,
                HashedPassword = _securityUtils.Hash(request.Password),
            };
            player.Boss = boss;

            _unitOfWork.Players.Create(player);

            Random random = new Random();

            foreach (var agentName in request.AgentNames)
            {
                var newAgent = new Agent()
                {
                    FirstName = _basicUtils.UppercaseFirst(agentName),
                    LastName = _basicUtils.UppercaseFirst(request.BossLastName),
                    Strength = random.Next(2, 5),
                    Intelligence = random.Next(2, 5),
                    Dexterity = random.Next(2, 5),
                    Upkeep = random.Next(2, 5) * 10,
                    BossId = boss.Id,
                    State = AgentState.Active
                };
                _unitOfWork.Agents.Create(newAgent);
            }

            _unitOfWork.Commit();
        }

        public async Task ChangePassword(ChangePasswordRequest request)
        {
            await _playerValidator.ValidateChangePassword(request);
            Player player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId);

            player.HashedPassword = _securityUtils.Hash(request.NewPassword);
            _unitOfWork.Commit();
        }

        public async Task<bool> DeleteAccount(DeleteAccountRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
