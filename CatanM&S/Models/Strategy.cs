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
            for (int i = 0; i < 25; i++)
            {
                _game.RollDice();
            }
        }

        private void ApplyStrategies()
        {
            ApplyRandomStrategy(_game.Players[0]);
            ApplyBestNumberProbabilityStrategy(_game.Players[1]);
            ApplyBestResourceStrategy(_game.Players[2]);
            ApplyBestResourceAndNumberStrategy(_game.Players[3]);
        }

        private void ApplyRandomStrategy(Player player)
        {
            var random = new Random();
            var intersections = _game.Board.Intersections;
            var firstHouse = intersections[random.Next(intersections.Count)];
            player.AddHouse(firstHouse);
            Intersection secondHouse;
            do
            {
                secondHouse = intersections[random.Next(intersections.Count)];
            } while (secondHouse == firstHouse);
            player.AddHouse(secondHouse);
        }

        private void ApplyBestNumberProbabilityStrategy(Player player)
        {
            var intersections = _game.Board.Intersections
                .OrderByDescending(i => i.AdjacentTiles.Sum(t => t.Number != 0 ? 1.0 / (6.0 - Math.Abs(t.Number - 7)) : 0))
                .Take(2).ToList();
            player.AddHouse(intersections[0]);
            player.AddHouse(intersections[1]);
        }

        private void ApplyBestResourceStrategy(Player player)
        {
            var intersections = _game.Board.Intersections
                .OrderByDescending(i => i.AdjacentTiles.Count(t => t.Resource != ResourceType.Desert))
                .Take(2).ToList();
            player.AddHouse(intersections[0]);
            player.AddHouse(intersections[1]);
        }

        private void ApplyBestResourceAndNumberStrategy(Player player)
        {
            var intersections = _game.Board.Intersections
                .OrderByDescending(i => i.AdjacentTiles.Sum(t => t.Number != 0 ? 1.0 / (6.0 - Math.Abs(t.Number - 7)) : 0)
                                        + i.AdjacentTiles.Count(t => t.Resource != ResourceType.Desert))
                .Take(2).ToList();
            player.AddHouse(intersections[0]);
            player.AddHouse(intersections[1]);
        }
    }
}