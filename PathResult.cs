using System;
using System.Collections.Generic;
using System.Text;

namespace Astronauts
{
    public class PathResult
    {
        public bool Found { get; }
        public int Cost { get; }
        public List<Position> Path { get; }

        public PathResult(bool found, int cost, List<Position> path) 
        {
            Found = found;
            Cost = cost;
            Path = path;

        }

        public static PathResult NotFound => new PathResult ( false, 0, new List<Position>());

       



    }
}
