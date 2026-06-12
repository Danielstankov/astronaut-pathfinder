namespace Astronauts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CosmicMap map = CosmicMap.Parse();

                IPathFinder pathFinder = new DijkstraPathFinder();
                MissionControl missionControl = new MissionControl(map, pathFinder);

                missionControl.RunMissions();
                missionControl.PrintResults();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
