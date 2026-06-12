using System;
using System.Collections.Generic;
using System.Text;

namespace Astronauts
{
    public class DijkstraPathFinder : IPathFinder
    {
        public PathResult FindPath(CosmicMap map, Position start, Position end)
        {
            Dictionary<Position, int> distances = new Dictionary<Position, int>();
            Dictionary<Position, Position> previous = new Dictionary<Position, Position>();
            HashSet<Position> visited = new HashSet<Position>();
            PriorityQueue<Position, int> queue = new PriorityQueue<Position, int>();

            distances[start] = 0;
            queue.Enqueue(start, 0);

            int[] dRow = { -1, 1, 0, 0 };
            int[] dCol = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                queue.TryDequeue(out Position current, out int currentDistance);

                if (visited.Contains(current))
                {
                    continue;
                }

                visited.Add(current);

                if (current.Equals(end))
                {
                    break;
                }

                for (int i = 0; i < 4; i++)
                {
                    Position neighbor = new Position(current.Row + dRow[i], current.Col + dCol[i]);

                    if (!map.IsInBounds(neighbor) || !map.IsPassable(neighbor) || visited.Contains(neighbor))
                    {
                        continue;
                    }

                    int newDistance = currentDistance + 1;

                    if (!distances.ContainsKey(neighbor) || newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        previous[neighbor] = current;
                        queue.Enqueue(neighbor, newDistance);
                    }
                }
            }

            if (!distances.ContainsKey(end))
            {
                return PathResult.NotFound;
            }

            List<Position> path = new List<Position>();
            Position step = end;

            while (!step.Equals(start))
            {
                path.Add(step);
                step = previous[step];
            }

            path.Add(start);
            path.Reverse();

            return new PathResult(true, distances[end], path);
        }
    }
}
