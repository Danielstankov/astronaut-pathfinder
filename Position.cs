using System;
using System.Collections.Generic;
using System.Text;

namespace Astronauts
{
    public struct Position
    {
        private int row;
        private int col;
        public int Row
        {
            get { return row; }
        }

        public int Col 
        {
            get { return col; }
        }

        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public override bool Equals(object? obj)
        {
            return obj is Position other && Row == other.Row && Col == other.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }
    }
}
