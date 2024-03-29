﻿using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IPlayerUtils
    {
    }

    public class PlayerUtils : IPlayerUtils
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PlayerUtils> _logger;

        public PlayerUtils(IRandomizer randomizer, IUnitOfWork unitOfWork, ILogger<PlayerUtils> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
    }
}
