﻿using AutoMapper;
using Castle.Core.Logging;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Quartz;
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
        Task DeleteAccount(DeleteAccountRequest user, bool withoutValidation = false);
        Task<string> Activate(string code);
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
        private readonly IMailSender _mailSender;
        private readonly ISchedulerFactory _scheduler;
        private readonly IRemoveNotActivatedPlayerJobRunner _removeNotActivatedPlayerJobRunner;

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper, 
            ISecurityUtils securityUtils, ITokenUtils tokenUtils,
            IBasicUtils basicUtils, IPlayerValidator playerValidator,
            ISchedulerFactory factory, IMapUtils mapUtils, IMailSender mailSender, 
            IRemoveNotActivatedPlayerJobRunner removeNotActivatedPlayerJobRunner, ISchedulerFactory scheduler,
            ILogger<PlayerService> logger)
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
            _removeNotActivatedPlayerJobRunner = removeNotActivatedPlayerJobRunner;
            _logger = logger;
        }

        /// <summary>
        /// Returns tokens if login datas are correct
        /// </summary>
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

            request.Password = _securityUtils.Hash(request.Password);

            //player creation
            var playerRole = await _unitOfWork.Roles.GetByNameAsync(RoleConsts.Player);
            var activationCode = Guid.NewGuid().ToString();

            Player player = new Player()
            {
                Nick = request.Nick,
                HashedPassword = request.Password,
                Role = playerRole,
                Email = request.Email,
                State = PlayerState.NotActivated
            };
            player.Boss = boss;

            _unitOfWork.Players.Create(player);

            //not activated player instance creation

            Random random = new Random();

            //agents creation
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

            //not activated player instance

            var deletionTime = DateTime.Now.AddMinutes(PlayerConsts.MINUTES_TO_REMOVE_NOT_ACTIVATED_PLAYER);
            NotActivatedPlayer notActivatedPlayer = new NotActivatedPlayer()
            {
                Player = player,
                DateOfDeletion = deletionTime,
                ActivationCode = activationCode
            };

            _unitOfWork.NotActivatedPlayers.Create(notActivatedPlayer);

            _unitOfWork.Commit();

            await _removeNotActivatedPlayerJobRunner.Start(_scheduler, deletionTime, player.Id);


            //mail with activation link
            var link = request.ApiUrl + "/activate?code=" + activationCode;

            var subject = "Mafia Online - activation link";


            var htmlLink = $"<a href = \"{link}\">{link}</a>";

            var content = "Click this link to activate your Mafia Online account.<br>" + htmlLink;
            _mailSender.SendEmail(subject, content, request.Email);
        }

        /// <summary>
        /// Changes password if the old one is entered correctly
        /// </summary>
        public async Task ChangePassword(ChangePasswordRequest request)
        {
            await _playerValidator.ValidateChangePassword(request);
            Player player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId);

            player.HashedPassword = _securityUtils.Hash(request.NewPassword);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Deletes an account (player, boss, agents instances)
        /// </summary>
        public async Task DeleteAccount(DeleteAccountRequest request, bool withoutValidation = false)
        {
            if(withoutValidation == false)
                await _playerValidator.ValidateDeleteAccount(request);
            var player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId);
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
            _unitOfWork.Players.DeleteById(player.Id);


            _unitOfWork.Agents.UpdateRange(othersAgents.ToArray());
            _unitOfWork.Commit();
        }

        public async Task<string> Activate(string code)
        {
            var notActivatedPlayer = await _unitOfWork.NotActivatedPlayers.GetByCode(code);
            if(notActivatedPlayer == null)
                return "User not found. It's possible the account is already active.";
            var player = await _unitOfWork.Players.GetByIdAsync(notActivatedPlayer.PlayerId);
            _unitOfWork.NotActivatedPlayers.DeleteById(notActivatedPlayer.Id);
            player.State = PlayerState.Activated;

            IScheduler scheduler = await _scheduler.GetScheduler();
            //removing job
            JobKey jobKey = new(notActivatedPlayer.JobKey, "group1");
            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
            }
            _unitOfWork.Commit();
            _logger.LogDebug($"Account with id {player.Id} activated");
            return "Account activated";
        }
    }
}
