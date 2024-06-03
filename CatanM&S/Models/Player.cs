using System.Collections.Generic;

namespace CatanM_S.Models
{
    public class Player
    {
        public List<Intersection> Houses { get; set; }
        public Dictionary<ResourceType, int> Resources { get; set; }

        public Player()
        {
            Houses = new List<Intersection>();
            Resources = new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 0 },
                { ResourceType.Brick, 0 },
                { ResourceType.Sheep, 0 },
                { ResourceType.Wheat, 0 },
                { ResourceType.Ore, 0 }
            };
        }

        public void AddHouse(Intersection intersection)
        {
            Houses.Add(intersection);
        }

        public void CollectResources(Tile tile)
        {
            if (tile.Resource != ResourceType.Desert)
            {
                Resources[tile.Resource]++;
            }
        }
    }
}
