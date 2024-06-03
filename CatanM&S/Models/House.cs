using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanM_S.Models
{
    public class House
    {
        public int Q { get; set; }  // Coordenada axial Q
        public int R { get; set; }  // Coordenada axial R

        public House(int q, int r)
        {
            Q = q;
            R = r;
        }
    }

}
