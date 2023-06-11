using AutoMapper;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Security.Application;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IJobService
    {
        Task<IList<JobDTO>> GetAllActiveJobs();
    }

    public class JobService : IJobService
    {
        private readonly ISchedulerFactory _scheduler;

        public JobService(ISchedulerFactory scheduler)
        {
            _scheduler = scheduler;
        }

        /// <summary>
        /// Returns all active jobs
        /// </summary>
        public async Task<IList<JobDTO>> GetAllActiveJobs()
        {
            var scheduler = await _scheduler.GetScheduler();

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            var jobs = new List<JobDTO>();

            foreach (var jobKey in jobKeys) 
            { 
                var jobDetails = await scheduler.GetJobDetail(jobKey);
                var jobTriggerExecutionTime = (await scheduler.GetTriggersOfJob(jobKey))
                    .Min(x => x.FinalFireTimeUtc);

                jobs.Add(new JobDTO
                {
                    Name = jobKey.Name,
                    Description = jobDetails.Description,
                    ExecutionTime = jobTriggerExecutionTime.HasValue ? jobTriggerExecutionTime.Value.LocalDateTime : null
                });
            }

            return jobs;
        }
    }
}
