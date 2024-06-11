using System;
using System.Collections.Generic;
using System.Linq;

namespace CatanM_S.Models
{
    public class Board
    {
        public List<Tile> Tiles { get; set; }
        public List<Intersection> Intersections { get; set; }

        public Board()
        {
            Tiles = new List<Tile>();
            Intersections = new List<Intersection>();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Posiciones para 38 hexágonos
            int[,] positions = new int[,]
            {
                { 0, 0}, { 1, 0}, { 1, -1}, { 0, -1}, {-1, 0}, {-1, 1}, { 0, 1},
                { 2, 0}, { 2, -1}, { 2, -2}, { 1, -2}, { 0, -2}, {-1, -1}, {-2, 0},
                {-2, 1}, {-2, 2}, {-1, 2}, { 0, 2}, { 1, 1}, { 3, 0}, { 3, -1},
                { 3, -2}, { 3, -3}, { 2, -3}, { 1, -3}, { 0, -3}, {-1, -2}, {-2, -1},
                {-3, 0}, {-3, 1}, {-3, 2}, {-2, 3}, {-1, 3}, { 0, 3}, { 1, 2},
                { 2, 1}, { 3, 1}
            };

            // Distribuir aleatoriamente los recursos y los números
            var resources = new List<ResourceType>
            {
                ResourceType.Brick, ResourceType.Wood, ResourceType.Wheat, ResourceType.Wheat, ResourceType.Sheep,
                ResourceType.Ore, ResourceType.Brick, ResourceType.Wood, ResourceType.Brick, ResourceType.Sheep,
                ResourceType.Ore, ResourceType.Desert, ResourceType.Wheat, ResourceType.Wood, ResourceType.Sheep,
                ResourceType.Ore, ResourceType.Wood, ResourceType.Wheat, ResourceType.Sheep, ResourceType.Brick,
                ResourceType.Wood, ResourceType.Wheat, ResourceType.Wheat, ResourceType.Sheep, ResourceType.Ore,
                ResourceType.Brick, ResourceType.Wood, ResourceType.Brick, ResourceType.Sheep, ResourceType.Ore,
                ResourceType.Desert, ResourceType.Wheat, ResourceType.Wood, ResourceType.Sheep, ResourceType.Ore,
                ResourceType.Wood, ResourceType.Wheat, ResourceType.Sheep
            };
            var numbers = new List<int>
            {
                5, 2, 6, 3, 8,
                10, 9, 12, 11, 4,
                8, 10, 9, 4, 5, 6, 3, 11, 5, 2, 6, 3, 8,
                10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11
            };

            var random = new Random();
            resources = resources.OrderBy(r => random.Next()).ToList();
            numbers = numbers.OrderBy(n => random.Next()).ToList();

            int numberIndex = 0;
            for (int i = 0; i < positions.GetLength(0); i++)
            {
                var resource = resources[i];
                int number = resource == ResourceType.Desert ? 0 : numbers[numberIndex++];
                Tiles.Add(new Tile(resource, number, positions[i, 0], positions[i, 1]));
            }

            InitializeIntersections();
        }

        private void InitializeIntersections()
        {
            var intersectionPositions = new HashSet<(int, int)>
            {
                (0, 0), (1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1),
                (2, 0), (2, -1), (2, -2), (1, -2), (0, -2), (-1, -1), (-2, 0),
                (-2, 1), (-2, 2), (-1, 2), (0, 2), (1, 1), (3, 0), (3, -1),
                (3, -2), (3, -3), (2, -3), (1, -3), (0, -3), (-1, -2), (-2, -1),
                (-3, 0), (-3, 1), (-3, 2), (-2, 3), (-1, 3), (0, 3), (1, 2),
                (2, 1), (3, 1)
            };

            foreach (var pos in intersectionPositions)
            {
                Intersections.Add(new Intersection(pos.Item1, pos.Item2));
            }

            foreach (var intersection in Intersections)
            {
                foreach (var tile in Tiles)
                {
                    if (IsTileAdjacentToIntersection(tile, intersection))
                    {
                        intersection.AddAdjacentTile(tile);
                    }
                }
            }
        }

        private bool IsTileAdjacentToIntersection(Tile tile, Intersection intersection)
        {
            var tileCoords = new (int, int)[]
            {
                (tile.Q, tile.R),
                (tile.Q + 1, tile.R),
                (tile.Q - 1, tile.R),
                (tile.Q, tile.R + 1),
                (tile.Q, tile.R - 1),
                (tile.Q + 1, tile.R - 1),
                (tile.Q - 1, tile.R + 1)
            };

            return tileCoords.Contains((intersection.Q, intersection.R));
        }

        public Intersection GetIntersectionAt(int q, int r)
        {
            return Intersections.FirstOrDefault(intersection => intersection.Q == q && intersection.R == r);
        }
    }

    public class Intersection
    {
        public int Q { get; set; }
        public int R { get; set; }
        public List<Tile> AdjacentTiles { get; set; }

        public Intersection(int q, int r)
        {
            Q = q;
            R = r;
            AdjacentTiles = new List<Tile>();
        }

        public void AddAdjacentTile(Tile tile)
        {
            AdjacentTiles.Add(tile);
        }
    }
}
