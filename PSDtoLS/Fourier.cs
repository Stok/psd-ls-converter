using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSDtoLS
{
    public class Fourier
    {
        int length; 
        double j, e0;
        int i;
        double[,] g;
        double[] lims;

        public void Initialize(double[,] gs, double e0)
        {
            length = gs.Length / 2;
            lims = new double[2] { gs[0, 0], gs[length - 1, 0] };
            this.g = gs;
            this.e0 = e0;
        }
        public double Evaluate(double dnu)
        {
            j = 0;
            for (i = 0; i < length; i++)
            {
                j += (1 - (g[i, 0] / g[length - 1, 0])) * Math.Cos(2 * Math.PI * dnu * g[i, 0]) * g[i, 1];
            }
            return  4 * e0 * e0 * g[length - 1, 0] / length * j;
        }
    }
}
