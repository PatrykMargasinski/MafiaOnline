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
    public interface INewsValidator
    {
        void ValidateNews(News news);
    }

    public class NewsValidator : INewsValidator
    {
        public NewsValidator()
        {

        }

        public void ValidateNews(News news)
        {
            if (string.IsNullOrEmpty(news.HTMLContent))
                throw new Exception("Content cannot be empty");

            if (string.IsNullOrEmpty(news.Subject))
                throw new Exception("Subject cannot be empty");

            if (news.Priority<1||news.Priority>10)
                throw new Exception("Priority must be a number between 1 and 10");
        }
    }
}