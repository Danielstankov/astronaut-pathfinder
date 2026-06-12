using System;
using System.Collections.Generic;
using System.Text;

namespace Astronauts
{
    public class Astronaut
    {
        public string Id { get; }
        public Position StartPosition { get; }
        public PathResult? Result { get; set; }

        public Astronaut(string id, Position startPosition)
        {
            Id = id;
            StartPosition = startPosition;
        }
    }
}
