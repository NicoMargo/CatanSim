using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanM_S.Models
{
   public enum ResourceType { Wood, Brick, Sheep, Wheat, Ore, Desert }

public class Tile
{
    public ResourceType Resource { get; set; }
    public int Number { get; set; }
    public int Q { get; set; }  // Coordenada axial Q
    public int R { get; set; }  // Coordenada axial R

    public Tile(ResourceType resource, int number, int q, int r)
    {
        Resource = resource;
        Number = number;
        Q = q;
        R = r;
    }
}
}
