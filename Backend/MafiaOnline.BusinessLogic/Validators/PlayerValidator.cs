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

namespace MafiaOnline.BusinessLogic.Validators
{
    public interface IPlayerValidator
    {
        Task ValidateLogin(LoginRequest request);
        Task ValidateRegister(RegisterRequest request);
        Task ValidateChangePassword(ChangePasswordRequest changeModel);
    }

    public class PlayerValidator : IPlayerValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBasicUtils _basicUtils;
        private readonly ISecurityUtils _securityUtils;

        public PlayerValidator(IUnitOfWork unitOfWork, IMapper mapper, IBasicUtils basicUtils, ISecurityUtils securityUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _basicUtils = basicUtils;
            _securityUtils = securityUtils;
        }

        public async Task ValidateLogin(LoginRequest request)
        {
            if (request == null)
            {
                throw new Exception("There is no data");
            }

            IList<string> errors = new List<string>();
            if (string.IsNullOrEmpty(request.Nick)) errors.Add("Nick is empty");
            if (string.IsNullOrEmpty(request.Password)) errors.Add("Password is empty");

            if (errors.Count > 0)
            {
                throw new Exception(string.Join('\n', errors));
            }

            Player player = await _unitOfWork.Players.GetByNick(request.Nick);
            if (player == null)
            {
                throw new Exception("There is no player with such nick");
            }

            if (_securityUtils.VerifyPassword(player, request.Password) == false)
            {
                throw new Exception("Wrong password");
            }
        }

        public async Task ValidateRegister(RegisterRequest request)
        {
            if (request == null)
            {
                throw new Exception("There is no data");
            }
            IList<string> errors = new List<string>();
            if (request.Nick == "") errors.Add("Nick is empty");
            else if (await _unitOfWork.Players.GetByNick(request.Nick) != null)
            {
                errors.Add("There is a player with a such nick");
            }
            if (request.Password == "") errors.Add("Password is empty");
            if (request.BossFirstName == "") errors.Add("Boss first name is empty");
            else if (!_basicUtils.IsAlphabets(request.BossFirstName)) errors.Add("Boss first name should include only alphabets");
            if (request.BossLastName == "") errors.Add("Boss last name is empty");
            else
            {
                if (!_basicUtils.IsAlphabets(request.BossLastName)) errors.Add("Boss last name should include only alphabets");
                else if ((await _unitOfWork.Bosses.GetByLastName(request.BossLastName)).Count > 0)
                {
                    errors.Add("There is a boss with a such last name");
                }
            }
            if (request.AgentNames == null) errors.Add("There is no agent names");
            else
            {
                if (request.AgentNames.Length != 3) errors.Add("Wrong number of agent first names");
                else for (int i = 0; i < request.AgentNames.Length; i++)
                    {
                        if (request.AgentNames[i] == "")
                            errors.Add($"Agent{i}'s first name is empty");
                        else if (!_basicUtils.IsAlphabets(request.AgentNames[i]))
                            errors.Add($"Agent{i}'s first name should include only alphabets");
                    }
            }
            if (errors.Count > 0) throw new Exception(string.Join('\n', errors));
        }


        public async Task ValidateChangePassword(ChangePasswordRequest request)
        {
            Player player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId);
            if (player == null)
            {
                throw new Exception("User not found");
            }

            if (_securityUtils.VerifyPassword(player, request.OldPassword) == false)
            {
                throw new Exception("Invalid old password");
            }

            if (!request.NewPassword.Equals(request.RepeatedNewPassword))
            {
                throw new Exception("Repeated new password isn't correct");
            }
        }
    }
}
