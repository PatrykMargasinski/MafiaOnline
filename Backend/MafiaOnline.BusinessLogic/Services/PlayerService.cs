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

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper, 
            ISecurityUtils securityUtils, ITokenUtils tokenUtils, 
            IBasicUtils basicUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
            _tokenUtils = tokenUtils;
            _basicUtils = basicUtils;
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
            var boss = await _unitOfWork.Bosses.GetByIdAsync(player.BossId);
            boss.LastSeen = DateTime.Now;
            _unitOfWork.Commit();

            return new Tokens()
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task Register(RegisterRequest model)
        {
            if (model == null)
            {
                throw new Exception("There is no data");
            }
            IList<string> errors = new List<string>();
            if (model.Nick == "") errors.Add("Nick is empty");
            else if (await _unitOfWork.Players.GetByNick(model.Nick) != null)
            {
                errors.Add("There is a player with a such nick");
            }
            if (model.Password == "") errors.Add("Password is empty");
            if (model.BossFirstName == "") errors.Add("Boss first name is empty");
            else if (!_basicUtils.IsAlphabets(model.BossFirstName)) errors.Add("Boss first name should include only alphabets");
            if (model.BossLastName == "") errors.Add("Boss last name is empty");
            else
            {
                if (!_basicUtils.IsAlphabets(model.BossLastName)) errors.Add("Boss last name should include only alphabets");
                else if ((await _unitOfWork.Bosses.GetByLastName(model.BossLastName)).Count>0)
                {
                    errors.Add("There is a boss with a such last name");
                }
            }
            if (model.AgentNames == null) errors.Add("There is no agent names");
            else
            {
                if (model.AgentNames.Length != 3) errors.Add("Wrong number of agent first names");
                else for (int i = 0; i < model.AgentNames.Length; i++)
                    {
                        if (model.AgentNames[i] == "")
                            errors.Add($"Agent{i}'s first name is empty");
                        else if (!_basicUtils.IsAlphabets(model.AgentNames[i]))
                            errors.Add($"Agent{i}'s first name should include only alphabets");
                    }
            }
            if (errors.Count > 0) throw new Exception(string.Join('\n', errors));

            Boss boss = new Boss()
            {
                FirstName = _basicUtils.UppercaseFirst(model.BossFirstName),
                LastName = _basicUtils.UppercaseFirst(model.BossLastName),
                Money = 5000
            };
            _unitOfWork.Bosses.Create(boss);
            Player player = new Player()
            {
                Nick = model.Nick,
                HashedPassword = _securityUtils.Hash(model.Password),
            };
            player.Boss = boss;

            _unitOfWork.Players.Create(player);

            Random random = new Random();

            foreach (var agentName in model.AgentNames)
            {
                var newAgent = new Agent()
                {
                    FirstName = _basicUtils.UppercaseFirst(agentName),
                    LastName = _basicUtils.UppercaseFirst(model.BossLastName),
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

        public async Task ChangePassword(ChangePasswordRequest changeModel)
        {
            Player player = await _unitOfWork.Players.GetByIdAsync(changeModel.PlayerId);
            if (player == null)
            {
                throw new Exception("User not found");
            }

            if (_securityUtils.VerifyPassword(player, changeModel.OldPassword) == false)
            {
                throw new Exception("Invalid old password");
            }

            if (!changeModel.NewPassword.Equals(changeModel.RepeatedNewPassword))
            {
                throw new Exception("Repeated new password isn't correct");
            }


            player.HashedPassword = _securityUtils.Hash(changeModel.NewPassword);
            _unitOfWork.Commit();
        }

        public async Task<bool> DeleteAccount(DeleteAccountRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
