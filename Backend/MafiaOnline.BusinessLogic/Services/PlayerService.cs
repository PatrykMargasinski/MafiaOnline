using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions.Equivalency;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IPlayerService
    {
        Task<Tokens> Login(LoginRequest user);
        Task Register(RegisterRequest user);
        Task ChangePassword(ChangePasswordRequest user);
        Task DeleteAccount(DeleteAccountRequest user, bool withoutValidation = false);
        Task<string> Activate(ActivationToken activationToken);
        Task CreateAndSendActivationLink(Player player, string apiUrl);
        Task ResetPassword(ResetPasswordRequest request);
        Task CreateAndSendResetPasswordCode(CreateResetPasswordCodeRequest request);
    }

    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PlayerService> _logger;
        private readonly ISecurityUtils _securityUtils;
        private readonly ITokenUtils _tokenUtils;
        private readonly IBasicUtils _basicUtils;
        private readonly IPlayerValidator _playerValidator;
        private readonly ISchedulerFactory _factory;
        private readonly IMapUtils _mapUtils;
        private readonly IRandomizer _randomizer;
        private readonly IMailSender _mailSender;
        private readonly ISchedulerFactory _scheduler;
        private readonly SignInManager<Player> _signInManager;
        private readonly UserManager<Player> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper,
            ISecurityUtils securityUtils, ITokenUtils tokenUtils,
            IBasicUtils basicUtils, IPlayerValidator playerValidator,
            ISchedulerFactory factory, IMapUtils mapUtils, IMailSender mailSender,
            ISchedulerFactory scheduler, ILogger<PlayerService> logger, IRandomizer randomizer,
            SignInManager<Player> signInManager, UserManager<Player> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
            _tokenUtils = tokenUtils;
            _basicUtils = basicUtils;
            _playerValidator = playerValidator;
            _factory = factory;
            _mapUtils = mapUtils;
            _mailSender = mailSender;
            _scheduler = scheduler;
            _logger = logger;
            _randomizer = randomizer;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Returns tokens if login datas are correct
        /// </summary>
        public async Task<Tokens> Login(LoginRequest request)
        {
            await _playerValidator.ValidateLogin(request);
            Player player = await _userManager.FindByNameAsync(request.Nick);
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

        /// <summary>
        /// Creates an account (player, boss, agents instances)
        /// </summary>
        public async Task Register(RegisterRequest request)
        {
            await _playerValidator.ValidateRegister(request);

            //boss creation
            Boss boss = new Boss()
            {
                FirstName = _basicUtils.UppercaseFirst(request.BossFirstName),
                LastName = _basicUtils.UppercaseFirst(request.BossLastName),
                Money = 5000
            };
            _unitOfWork.Bosses.Create(boss);

            //player creation
            Player player = new Player()
            {
                UserName = request.Nick,
                Email = request.Email
            };

            player.Boss = boss;


            //agents creation
            foreach (var agentName in request.AgentNames)
            {
                var newAgent = new Agent()
                {
                    FirstName = _basicUtils.UppercaseFirst(agentName),
                    LastName = _basicUtils.UppercaseFirst(request.BossLastName),
                    Strength = _randomizer.Next(2, 5),
                    Intelligence = _randomizer.Next(2, 5),
                    Dexterity = _randomizer.Next(2, 5),
                    Upkeep = _randomizer.Next(2, 5) * 10,
                    Boss = boss,
                    State = AgentState.Active,
                    IsFromBossFamily = true
                };
                _unitOfWork.Agents.Create(newAgent);
            }


            //headquarters creation
            var newPosition = await _mapUtils.GetNewHeadquartersPosition();

            var headquarter = new Headquarters() { Name = request.HeadquartersName, Boss = boss };
            var mapElement = new MapElement() { X = newPosition.X, Y = newPosition.Y, Type = MapElementType.Headquarters, Headquarters = headquarter, Boss = boss };
            _unitOfWork.MapElements.Create(mapElement);

            await _userManager.CreateAsync(player);

            _unitOfWork.Commit();
            await _userManager.AddToRoleAsync(player, RoleConsts.Player);
            await CreateAndSendActivationLink(player, request.ApiUrl);
        }

        public async Task CreateAndSendActivationLink(Player player, string apiUrl)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(player);
            token = Base64UrlEncoder.Encode(token);
            ////mail with activation link
            var link = apiUrl + $"/activate?token={token}&email={player.Email}";

            var subject = "Mafia Online - activation link";


            var htmlLink = $"<a href = \"{link}\">{link}</a>";

            var content = "Click this link to activate your Mafia Online account.<br>" + htmlLink;
            _mailSender.SendEmail(subject, content, player.Email);
        }

        /// <summary>
        /// Changes password if the old one is entered correctly
        /// </summary>
        public async Task ChangePassword(ChangePasswordRequest request)
        {
            await _playerValidator.ValidateChangePassword(request);
            Player player = await _userManager.FindByNameAsync(request.UserName);

            await _userManager.ChangePasswordAsync(player, request.OldPassword, request.NewPassword);

            _unitOfWork.Commit();
        }

        /// <summary>
        /// Deletes an account (player, boss, agents instances)
        /// </summary>
        public async Task DeleteAccount(DeleteAccountRequest request, bool withoutValidation = false)
        {
            if(withoutValidation == false)
                await _playerValidator.ValidateDeleteAccount(request);
            var player = await _userManager.FindByNameAsync(request.UserName);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(player.BossId);
            var agents = await _unitOfWork.Agents.GetBossAgents(boss.Id);
            var headquarters = await _unitOfWork.Headquarters.GetByBossId(boss.Id);

            //removing PerformingMission instances and mission jobs
            var agentsOnMission = agents.Where(x => x.State == AgentState.OnMission);
            var performingMissions = await _unitOfWork.PerformingMissions.GetByAgentIds(agentsOnMission.Select(x => x.Id).ToArray());

            IScheduler scheduler = await _factory.GetScheduler();

            foreach (var performingMission in performingMissions)
            {
                var jobKey = new JobKey($"missionJob{performingMission.Id}", "group1");

                if (await scheduler.CheckExists(jobKey))
                {
                    await scheduler.DeleteJob(jobKey);
                }
            }

            _unitOfWork.PerformingMissions.DeleteByIds(performingMissions.Select(x => x.Id).ToArray());

            //Agents from family are removed, others become renegades
            var familyAgents = agents.Where(x => x.IsFromBossFamily == true);
            _unitOfWork.Agents.DeleteByIds(familyAgents.Select(x => x.Id).ToArray());

            var othersAgents  = agents.Where(x => x.IsFromBossFamily == false);
            foreach(var agent in othersAgents)
            {
                agent.BossId = null;
                agent.Boss = null;
                agent.State = AgentState.Renegate;
            }

            _unitOfWork.MapElements.DeleteById(headquarters.MapElementId);
            _unitOfWork.Bosses.DeleteById(boss.Id);
            await _userManager.DeleteAsync(player);


            _unitOfWork.Agents.UpdateRange(othersAgents.ToArray());
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Activates not activated player account
        /// </summary>
        public async Task<string> Activate(ActivationToken activationToken)
        {
            if(activationToken?.Email == null)
                throw new Exception("Email not provided");
            if (activationToken?.Token == null)
                throw new Exception("Token not provided");

            var token = Base64UrlEncoder.Decode(activationToken.Token.Trim());
            var email = activationToken.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("Player not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if(result.Succeeded)
                return "Your account has been successfully confirmed. Enjoy your game!";
            else
                return "There are some problems.\n" + string.Join(Environment.NewLine, result.Errors.ToList().Select(x => x.Description));
        }

        /// <summary>
        /// Creates and sends code to reset password
        /// </summary>
        public async Task CreateAndSendResetPasswordCode(CreateResetPasswordCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new Exception("Email not provided");

            var player = await _userManager.FindByEmailAsync(request.Email);
            if (player == null)
                throw new Exception("Player with a such email not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(player);

            token = Base64UrlEncoder.Encode(token);

            ////reset code
            var link = $"That's the reset password token:\n{token}";

            var subject = "Mafia Online - reset password";

            var content = link;
            _mailSender.SendEmail(subject, content, player.Email);
        }

        /// <summary>
        /// Resets password (using token)
        /// </summary>
        public async Task ResetPassword(ResetPasswordRequest request)
        {
            await _playerValidator.ValidateResetPassword(request);
            var player = await _userManager.FindByEmailAsync(request.Email);
            var token = Base64UrlEncoder.Decode(request.Token.Trim());

            var result = await _userManager.ResetPasswordAsync(player, token, request.Password);

            if (result.Succeeded)
                return;
            else
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }
    }
}
