using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IMovingAgentUtils
    {
        string GetDatas(long movingAgentId);
        Task SetJobKey(long movingAgentId, string jobKey);
        void RemoveMovingAgent(long movingAgentId);
        Task ExposeMapElement(long mapElementId, long bossId);
    }

    public class MovingAgentUtils : IMovingAgentUtils
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovingAgentUtils(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string GetDatas(long movingAgentId)
        {
            return _unitOfWork.MovingAgents.GetDatas(movingAgentId);
        }

        public void RemoveMovingAgent(long movingAgentId)
        {
            _unitOfWork.MovingAgents.DeleteById(movingAgentId);
            _unitOfWork.Commit();
        }

        public async Task SetJobKey(long movingAgentId, string jobKey)
        {
            var movingAgent = await _unitOfWork.MovingAgents.GetByIdAsync(movingAgentId);
            movingAgent.JobKey = jobKey;
            _unitOfWork.Commit();
        }


        public async Task ExposeMapElement(long mapElementId, long bossId)
        {
            var mapElement = await _unitOfWork.MapElements.GetByIdAsync(mapElementId);
            if (mapElement == null)
                throw new Exception("There is no map element with id: " + mapElementId);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(bossId);
            if (boss == null)
                throw new Exception("There is no boss with id: " + mapElementId);
            var exposedMapElement = new ExposedMapElement()
            {
                Boss = boss,
                MapElement = mapElement
            };
            _unitOfWork.ExposedMapElements.Create(exposedMapElement);
            _unitOfWork.Commit();
        }
    }
}
