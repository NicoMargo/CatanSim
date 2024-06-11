namespace CatanM_S.Models
{
    public class StrategySimulator
    {
        private Game _game;

        public StrategySimulator(Game game)
        {
            _game = game;
        }

        public void RunSimulation()
        {
            ApplyStrategies();
            for (int i = 0; i < 110; i++)
            {
                _game.RollDice();
            }
        }

        private void ApplyStrategies()
        {
            var occupiedIntersections = new HashSet<Intersection>();

            ApplyRandomStrategy(_game.Players[0], occupiedIntersections);
            ApplyBestNumberProbabilityStrategy(_game.Players[1], occupiedIntersections);
            ApplyBestResourceStrategy(_game.Players[2], occupiedIntersections);
            ApplyBestResourceAndNumberStrategy(_game.Players[3], occupiedIntersections);
        }

        private void ApplyRandomStrategy(Player player, HashSet<Intersection> occupiedIntersections)
        {
            var random = new Random();
            var intersections = _game.Board.Intersections;

            var house = GetRandomIntersection(random, intersections, occupiedIntersections);
            player.AddHouse(house);
            occupiedIntersections.Add(house);
        }

        private Intersection GetRandomIntersection(Random random, List<Intersection> intersections, HashSet<Intersection> occupiedIntersections)
        {
            Intersection selectedIntersection;
            do
            {
                selectedIntersection = intersections[random.Next(intersections.Count)];
            } while (occupiedIntersections.Contains(selectedIntersection));
            return selectedIntersection;
        }

        private void ApplyBestNumberProbabilityStrategy(Player player, HashSet<Intersection> occupiedIntersections)
        {
            var intersections = _game.Board.Intersections
                .OrderByDescending(i => i.AdjacentTiles.Sum(t => t.Number != 0 ? 1.0 / (6.0 - Math.Abs(t.Number - 7)) : 0))
                .ToList();
            AddUniqueHouse(player, intersections, occupiedIntersections);
        }

        private void ApplyBestResourceStrategy(Player player, HashSet<Intersection> occupiedIntersections)
        {
            var intersections = _game.Board.Intersections
                .OrderByDescending(i => i.AdjacentTiles.Count(t => t.Resource != ResourceType.Desert))
                .ToList();
            AddUniqueHouse(player, intersections, occupiedIntersections);
        }

        private void ApplyBestResourceAndNumberStrategy(Player player, HashSet<Intersection> occupiedIntersections)
        {
            var intersections = _game.Board.Intersections
                .OrderByDescending(i => i.AdjacentTiles.Sum(t => t.Number != 0 ? 1.0 / (6.0 - Math.Abs(t.Number - 7)) : 0)
                                        + i.AdjacentTiles.Count(t => t.Resource != ResourceType.Desert))
                .ToList();
            AddUniqueHouse(player, intersections, occupiedIntersections);
        }

        private void AddUniqueHouse(Player player, List<Intersection> intersections, HashSet<Intersection> occupiedIntersections)
        {
            foreach (var intersection in intersections)
            {
                if (!occupiedIntersections.Contains(intersection))
                {
                    player.AddHouse(intersection);
                    occupiedIntersections.Add(intersection);
                    break;
                }
            }
        }
    }
}
