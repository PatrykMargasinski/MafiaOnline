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
    public interface IExposedMapElementRepository : ICrudRepository<ExposedMapElement>
    {

    }

    public class ExposedMapElementRepository : CrudRepository<ExposedMapElement>, IExposedMapElementRepository
    {
        public ExposedMapElementRepository(DataContext context) : base(context)
        {

        }
    }
}
