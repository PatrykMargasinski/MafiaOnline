﻿using AutoMapper;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using NUnit.Framework;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface INewsService
    {
        Task<IList<NewsDTO>> GetLastNews();
        void CreateNews(News news);
    }

    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INewsValidator _newsValidator;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper, INewsValidator newsValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _newsValidator = newsValidator;
        }

        public void CreateNews(News news)
        {
            _newsValidator.ValidateNews(news);
            _unitOfWork.News.Create(news);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Returns tokens if login datas are correct
        /// </summary>
        public async Task<IList<NewsDTO>> GetLastNews()
        {
            var news = await _unitOfWork.News.GetLastNews(NewsConsts.NUMBER_OF_NEWS);
            return _mapper.Map<IList<NewsDTO>>(news);
        }
    }
}