using System;
using System.Collections.Generic;
using System.Linq;

namespace Astronauts
{
    public class MissionControl
    {
        private readonly CosmicMap _map;
        private readonly IPathFinder _pathFinder;
        private List<Astronaut> _astronauts;

        public MissionControl(CosmicMap map, IPathFinder pathFinder)
        {
            _map = map;
            _pathFinder = pathFinder;
            _astronauts = map.FindAstronauts();
        }

        public void RunMissions()
        {
            Position station = _map.FindStation();

            foreach (Astronaut astronaut in _astronauts)
            {
                astronaut.Result = _pathFinder.FindPath(_map, astronaut.StartPosition, station);
            }

            _astronauts = _astronauts
                .OrderBy(a => a.Result!.Found)
                .ThenBy(a => a.Result!.Cost)
                .ToList();
        }

        public void PrintResults()
        {
            foreach (Astronaut astronaut in _astronauts)
            {
                PathResult result = astronaut.Result!;

                if (!result.Found)
                {
                    Console.WriteLine($"Mission failed — Astronaut {astronaut.Id} lost in space!");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine($"Astronaut {astronaut.Id}: shortest path length = {result.Cost}");
                Console.WriteLine(_map.RenderWithPath(new HashSet<Position>(result.Path)));
            }
        }
    }
}
