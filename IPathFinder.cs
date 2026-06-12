using System;
using System.Collections.Generic;
using System.Text;

namespace Astronauts
{
    public interface IPathFinder
    {
        PathResult FindPath(CosmicMap map, Position start, Position end);
    }
}
