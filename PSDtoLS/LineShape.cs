using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSDtoLS
{
    public class LineShape
    {
        int length; 
        double j, e0;
        int i;
        double[,] S, g;
        double[] lims;

        public void Initialize(double[,] s, double[,] gs, double e0 )
        {
            length = gs.Length/2;
            S = s;
            lims = new double[2] { gs[0, 0], gs[length - 1, 0] };
            this.g = gs;
            this.e0 = e0;
        }
        public double Evaluate(double dnu)
        {
            j = 0;
            for (i = 0; i < length; i++)
            {
                j += Math.Cos(2 * Math.PI * dnu * g[i, 0]) * g[i, 1];
            }
            return 4 * e0 * e0 * (lims[1] - lims[0]) / length * j;
        }
    }
}
