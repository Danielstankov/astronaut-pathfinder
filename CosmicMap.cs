using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Astronauts
{
    public class CosmicMap
    {
        public int Rows { get; }
        public int Cols { get; }

        private string[,] _grid;

        private static readonly HashSet<string> ValidTokens = new() { "O", "X", "F", "S1", "S2", "S3" };

        public CosmicMap(int rows, int cols, string[,] grid)
        {
            Rows = rows;
            Cols = cols;
            _grid = grid;
        }

        public bool IsInBounds(Position pos)
        {
            return pos.Row >= 0 && pos.Row < Rows && pos.Col >= 0 && pos.Col < Cols;
        }

        public bool IsPassable(Position pos)
        {
            return _grid[pos.Row, pos.Col] != "X";
        }

        public List<Astronaut> FindAstronauts()
        {
            List<Astronaut> astronauts = new();
            HashSet<string> seenIds = new();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    string cell = _grid[i, j];
                    if (cell == "S1" || cell == "S2" || cell == "S3")
                    {
                        if (!seenIds.Add(cell))
                        {
                            throw new InvalidOperationException($"Duplicate astronaut found: {cell}");
                        }

                        Astronaut myAstronaut = new Astronaut(cell, new Position(i, j));
                        astronauts.Add(myAstronaut);
                    }
                }
            }

            return astronauts;
        }

        public Position FindStation()
        {
            Position? station = null;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (_grid[i, j] == "F")
                    {
                        if (station != null)
                        {
                            throw new InvalidOperationException("Multiple space stations found on the map");
                        }

                        station = new Position(i, j);
                    }
                }
            }

            if (station == null)
            {
                throw new InvalidOperationException("No space station found on the map");
            }

            return station.Value;
        }

        public string RenderWithPath(HashSet<Position> pathCells)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    string cell = _grid[i, j];
                    if (pathCells.Contains(new Position(i, j)) && cell != "F" && !cell.StartsWith("S"))
                    {
                        sb.Append("*");
                    }
                    else
                    {
                        sb.Append(cell);
                    }

                    if (j < Cols - 1)
                    {
                        sb.Append(" ");
                    }

                }

                sb.Append("\n");

            }

            return sb.ToString();
        }

        public static CosmicMap Parse()
        {
            Console.Write("Enter path to map file: ");
            string path = Console.ReadLine() ?? "";

            if (!File.Exists(path))
            {
                string mapsPath = Path.Combine("Maps", path);

                if (!File.Exists(mapsPath))
                {
                    throw new ArgumentException($"File not found: {path}");
                }

                path = mapsPath;
            }

            string[] lines = File.ReadAllLines(path);

            if (lines.Length < 2 || lines.Length > 100)
            {
                throw new ArgumentException("Map must have between 2 and 100 rows");
            }

            int rows = lines.Length;
            int cols = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            if (cols < 2 || cols > 100)
            {
                throw new ArgumentException("Map must have between 2 and 100 columns");
            }

            string[,] grid = new string[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                string[] tokens = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length != cols)
                {
                    throw new ArgumentException($"Row {i + 1} has {tokens.Length} values, expected {cols}");
                }

                for (int j = 0; j < cols; j++)
                {
                    string token = tokens[j];

                    if (!ValidTokens.Contains(token))
                    {
                        throw new ArgumentException($"Row {i + 1} contains unrecognized symbol: '{token}'");
                    }

                    grid[i, j] = token;
                }
            }

            return new CosmicMap(rows, cols, grid);
        }
    }
}