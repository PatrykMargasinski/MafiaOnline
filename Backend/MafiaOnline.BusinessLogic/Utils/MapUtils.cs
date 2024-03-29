﻿using MafiaOnline.DataAccess.Database;
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
        bool IsCrossroad(long x, long y);
        bool IsAdjacent(Point p1, Point p2);
        Task<Point> GetNewHeadquartersPosition();
        Task<Point> GetNewMissionPosition();
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

        public bool IsCrossroad(long x, long y)
        {
            return x % 6 == 0 && y % 6 == 0;
        }

        public bool IsAdjacent(Point p1, Point p2)
        {
            return (p1.X == p2.X &&
                (
                    p1.Y == p2.Y - 1 || p1.Y == p2.Y + 1
                )) ||
                (p1.Y == p2.Y &&
                (
                    p1.X == p2.X - 1 || p1.X == p2.X + 1
                ));
        }

        private List<Point> GetElementsAroundThePoint(long x0, long y0, long size)
        {
            var elements = new List<Point>();
            for (long i = x0 - size; i <= x0 + size; i++)
                for (long j = y0 - size; j <= y0 + size; j++)
                    elements.Add(new Point(i, j));

            return elements;
        }

        public async Task<Point> GetNewHeadquartersPosition()
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
                .Except(allMapElements.Select(x => new Point(x.X, x.Y)))
                .Where(x => IsStreet(x.X, x.Y) && !IsCorner(x.X, x.Y))
                .ToList();

            var newPosition = allPossiblePositions[_randomizer.Next(allPossiblePositions.Count)];
            return newPosition;
        }

        public async Task<Point> GetNewMissionPosition()
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
                .Except(allMapElements.Select(x => new Point(x.X, x.Y)))
                .Where(x => IsStreet(x.X, x.Y) && !IsCorner(x.X, x.Y))
                .ToList();

            var newPosition = allPossiblePositions[_randomizer.Next(allPossiblePositions.Count)];
            return newPosition;
        }
    }
}
