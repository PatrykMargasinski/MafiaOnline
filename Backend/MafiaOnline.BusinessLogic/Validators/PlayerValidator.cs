using AutoMapper;
using Castle.Core.Internal;
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
        Task ValidateChangePassword(ChangePasswordRequest request);
        Task ValidateDeleteAccount(DeleteAccountRequest request);
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
            if (string.IsNullOrEmpty(request.Nick)) errors.Add("Nick is empty");
            else if (await _unitOfWork.Players.GetByNick(request.Nick) != null)
            {
                errors.Add("There is a player with a such nick");
            }
            if (string.IsNullOrEmpty(request.Password)) errors.Add("Password is empty");

            ValidatePassword(request.Password);

            if (string.IsNullOrEmpty(request.Email)) errors.Add("Email is empty");
            else if (CorrectEmail(request.Email) == false)
            {
                errors.Add("Incorrect email");
            }
            else if (await _unitOfWork.Players.GetByEmail(request.Email) != null)
            {
                errors.Add("There is a player with a such email");
            }

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
            if (request.HeadquartersName == "") errors.Add("Headquarters name is empty");
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

        private void ValidatePassword(string password)
        {

            if (password.Length < 8 || password.Length > 20)
                throw new Exception("The password must be at least 8 characters long and no longer than 20");

            if (!password.Any(char.IsUpper))
                throw new Exception("The password must to have at least one upper letter");

            if (!password.Any(char.IsLower))
                throw new Exception("The password must to have at leat one lower letter");

            if (password.Contains(" "))
                throw new Exception("The password cannot contain white space");



            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            bool containsSpecialChar = false;
            foreach (char ch in specialChArray)
            {
                if (password.Contains(ch))
                {
                    containsSpecialChar = true;
                    break;
                }
            }

            if (containsSpecialChar == false)
                throw new Exception("The password must contain at least one special character");
        }

        private bool CorrectEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task ValidateChangePassword(ChangePasswordRequest request)
        {
            Player player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId);
            if (player == null)
            {
                throw new Exception("User not found");
            }

            if (string.IsNullOrEmpty(request.OldPassword))
                throw new Exception("Old password not provided");

            if (string.IsNullOrEmpty(request.NewPassword))
                throw new Exception("New password not provided");

            if (string.IsNullOrEmpty(request.RepeatedNewPassword))
                throw new Exception("Repeated new password not provided");

            if (_securityUtils.VerifyPassword(player, request.OldPassword) == false)
            {
                throw new Exception("Invalid old password");
            }

            if (!request.NewPassword.Equals(request.RepeatedNewPassword))
            {
                throw new Exception("Repeated new password isn't correct");
            }

            ValidatePassword(request.NewPassword);
        }

        public async Task ValidateDeleteAccount(DeleteAccountRequest request)
        {
            Player player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId);
            if (player == null)
            {
                throw new Exception("User not found");
            }

            if (_securityUtils.VerifyPassword(player, request.Password) == false)
            {
                throw new Exception("Invalid password");
            }
        }
    }
}
