# Astronaut Pathfinder

This is a console application built for the SPACE 2026 task. It reads a grid containing one to three astronauts (S1, S2, S3) and a space station (F), and works out the shortest path from each astronaut to the station using Dijkstra's algorithm.

## Running it

The project targets .NET 10. Run it with `dotnet run`, and it will ask for the path to a map file. You can enter a full path like `Maps/demo_tall.txt`, or just the file name, e.g. `demo_tall.txt`, and it will be looked up inside the `Maps` folder automatically. Once you enter a valid path, it prints the results straight away.

## Why a map file instead of typing the grid in

The grid can be as large as 100x100, so typing it in cell by cell through the console would be slow and error prone. Reading from a file also makes testing much easier: a map can be written once, saved, and reused across runs without having to re-enter anything, which is useful both for the larger stress-test grids and for the smaller maps used to check specific edge cases.

## Map format

Maps are plain text files. Each line is a row of the grid, and the values in a row are separated by spaces. The symbols are:

- `O` for open space, which can be walked through
- `X` for an asteroid, which blocks movement
- `F` for the space station, the destination for every astronaut
- `S1`, `S2`, `S3` for the astronauts, between one and three of which can appear on a map

## Validation and error handling

Because the map comes from a file rather than being built in code, quite a bit of the program is dedicated to checking that the file actually describes a valid grid before any pathfinding starts. The checks include:

- The file has to exist and be readable
- The grid must have between 2 and 100 rows and between 2 and 100 columns
- Every row must contain the same number of values as the first row
- Every value must be one of the symbols listed above. Anything else, such as a typo, is rejected rather than silently treated as open space
- There must be exactly one space station on the map. If `F` is missing, or if it appears more than once, that's reported as an error
- Astronaut IDs cannot be duplicated, so two `S1`s on the same map is also rejected

Whenever one of these checks fails, the program prints a short `Error: ...` message and stops, rather than crashing or producing a confusing result.

## Output

For each astronaut, the program prints the length of its shortest path to the station, followed by a copy of the grid with that path marked using `*`. The astronaut's starting cell and the station itself are left unchanged so they are still easy to pick out. Astronauts are listed from shortest path to longest, and if an astronaut cannot reach the station at all, it is shown first with the message `Mission failed — Astronaut S1 lost in space!`.

## Sample maps

- `test1.txt`, `test2.txt`, `test3.txt` are small, basic cases
- `test_large.txt` is a 100x100 map used to check performance
- `demo_bait.txt`, `demo_tall.txt`, `demo_wide.txt` are non-square grids that show the path being drawn around obstacles
- `demo_dup_id.txt` has two astronauts sharing the same ID, to check that this is caught as an error
- All of these maps are in the `Maps` folder, and more can be added or current ones can be modified for testing if needed.

## Project structure

- `Position.cs` holds a single (row, column) coordinate on the grid
- `PathResult.cs` holds the result of a pathfinding attempt: whether a path was found, its length, and the cells it passes through
- `Astronaut.cs` holds an astronaut's ID, starting position, and result
- `IPathFinder.cs` is the interface implemented by the pathfinding algorithm
- `DijkstraPathfinder.cs` implements Dijkstra's algorithm using a priority queue
- `CosmicMap.cs` reads and validates the map file, and handles lookups and rendering
- `MissionControl.cs` runs the pathfinding for every astronaut and prints the results in order
- `Program.cs` is the entry point, wiring everything together and handling errors
