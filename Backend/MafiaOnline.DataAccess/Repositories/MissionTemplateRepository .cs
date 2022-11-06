using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Repositories
{
    public interface IMissionTemplateRepository : ICrudRepository<MissionTemplate>
    {

    }

    public class MissionTemplateRepository : CrudRepository<MissionTemplate>, IMissionTemplateRepository
    {
        public MissionTemplateRepository(DataContext context) : base(context)
        {

        }
    }
}
