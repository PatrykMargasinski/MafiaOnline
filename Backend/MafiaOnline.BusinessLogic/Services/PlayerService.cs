using AutoMapper;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
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
        Task DeleteAccount(DeleteAccountRequest user);
    }

    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurityUtils _securityUtils;
        private readonly ITokenUtils _tokenUtils;
        private readonly IBasicUtils _basicUtils;
        private readonly IPlayerValidator _playerValidator;
        private readonly ISchedulerFactory _factory;

        public PlayerService(IUnitOfWork unitOfWork, IMapper mapper, 
            ISecurityUtils securityUtils, ITokenUtils tokenUtils, 
            IBasicUtils basicUtils, IPlayerValidator playerValidator,
            ISchedulerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityUtils = securityUtils;
            _tokenUtils = tokenUtils;
            _basicUtils = basicUtils;
            _playerValidator = playerValidator;
            _factory = factory;
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
        public async Task DeleteAccount(DeleteAccountRequest request)
        {
            await _playerValidator.ValidateDeleteAccount(request);
            var player = await _unitOfWork.Players.GetByIdAsync(request.PlayerId);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(player.BossId);
            var agents = await _unitOfWork.Agents.GetBossAgents(boss.Id);

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
            _unitOfWork.Agents.UpdateRange(othersAgents.ToArray());
        }
    }
}
