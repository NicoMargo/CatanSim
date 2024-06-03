using System;
using System.Collections.Generic;
using System.Linq;

namespace CatanM_S.Models
{
    public class Game
    {
        public Board Board { get; set; }
        public List<Player> Players { get; set; }
        public int DiceRolls { get; set; }
        public List<int> DiceResults { get; set; }

        public Game()
        {
            Board = new Board();
            Players = new List<Player> { new Player(), new Player(), new Player(), new Player() };
            DiceRolls = 0;
            DiceResults = new List<int>();
        }

        public void PlaceInitialHouses()
        {
            // Esta lógica ahora está en las estrategias
        }

        public void RollDice()
        {
            DiceRolls++;
            Random rnd = new Random();
            int diceRoll = rnd.Next(1, 7) + rnd.Next(1, 7);
            DiceResults.Add(diceRoll);

            foreach (var player in Players)
            {
                foreach (var house in player.Houses)
                {
                    foreach (var tile in house.AdjacentTiles)
                    {
                        if (tile.Number == diceRoll)
                        {
                            player.CollectResources(tile);
                        }
                    }
                }
            }
        }

        public bool CheckVictory(Player player)
        {
            return player.Resources.All(resource => resource.Value >= 10);
        }
    }
}
