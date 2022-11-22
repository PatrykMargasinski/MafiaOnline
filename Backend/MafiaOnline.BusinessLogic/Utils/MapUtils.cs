using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IMapUtils
    {
        bool IsCorner(long x, long y);
        bool IsStreet(long x, long y);
        bool IsRoad(long x, long y);
        Task<(long, long)> GetNewHeadquartersPosition();
        Task<(long, long)> GetNewMissionPosition();
    }

    public class MapUtils : IMapUtils
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRandomizer _randomizer;
        public MapUtils(IUnitOfWork unitOfWork, IRandomizer randomizer)
        {
            _unitOfWork = unitOfWork;
            _randomizer = randomizer;
        }

        public bool IsCorner(long x, long y)
        {
            return ((x % 6 == 1) && (x % 6 == 5)) || ((y % 6 == 1) && (y % 6 == 5));
        }

        public bool IsStreet(long x, long y)
        {
            return
            ((x % 6 == 1 || x % 6 == 5) && y % 6 != 0) ||
            ((y % 6 == 1 || y % 6 == 5) && x % 6 != 0);
        }

        public bool IsRoad(long x, long y)
        {
            return x % 6 == 0 || y % 6 == 0;
        }

        private List<(long, long)> GetElementsAroundThePoint(long x0, long y0, long size)
        {
            var elements = new List<(long, long)>();
            for (long i = x0 - size; i <= x0 + size; i++)
                for (long j = y0 - size; j <= y0 + size; j++)
                    elements.Add((i, j));

            return elements;
        }

        public async Task<(long, long)> GetNewHeadquartersPosition()
        {
            var allMapElements = await _unitOfWork.MapElements.GetAllAsync();
            var allHeadquarters = allMapElements.Where(x => x.Type == MapElementType.Headquarters);
            var tempNewHeadquartersPosition = allHeadquarters
                .SelectMany(x => GetElementsAroundThePoint(x.X, x.Y, 15))
                .Distinct();

            var positionsToRemove = allHeadquarters
                .SelectMany(x => GetElementsAroundThePoint(x.X, x.Y, 10))
                .Distinct();

            var allPossiblePositions =
                tempNewHeadquartersPosition
                .Except(positionsToRemove)
                .Except(allMapElements.Select(x => (x.X, x.Y)))
                .Where(x => IsStreet(x.Item1, x.Item2) && !IsCorner(x.Item1, x.Item2))
                .ToList();

            var newPosition = allPossiblePositions[_randomizer.Next(allPossiblePositions.Count)];
            return newPosition;
        }

        public async Task<(long, long)> GetNewMissionPosition()
        {
            var allMapElements = await _unitOfWork.MapElements.GetAllAsync();
            var allHeadquarters = allMapElements.Where(x => x.Type == MapElementType.Headquarters);
            var tempNewHeadquartersPosition = allHeadquarters
                .SelectMany(x => GetElementsAroundThePoint(x.X, x.Y, 10))
                .Distinct();

            var positionsToRemove = allHeadquarters
                .SelectMany(x => GetElementsAroundThePoint(x.X, x.Y, 2))
                .Distinct();

            var allPossiblePositions =
                tempNewHeadquartersPosition
                .Except(positionsToRemove)
                .Except(allMapElements.Select(x => (x.X, x.Y)))
                .Where(x => IsStreet(x.Item1, x.Item2) && !IsCorner(x.Item1, x.Item2))
                .ToList();

            var newPosition = allPossiblePositions[_randomizer.Next(allPossiblePositions.Count)];
            return newPosition;
        }
    }
}
