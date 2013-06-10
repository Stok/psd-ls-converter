using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSDtoLS
{
    public class AutoCorrelationFnTerm2
    {
        double[,] S;
        int length;
        double[] lims;
        double e, r;

        public void Initialize(double[,] s)
        {
            S = s;
            length = S.Length/2;
            lims = new double[2] { S[0, 0], S[length - 1, 0] };
        }

        public double Evaluate(double tau)
        {
            e = 0;            
            for (int i = 0; i < length-1; i++)
            {
                r = S[i + 1, 0] - S[i, 0];
                e += r * S[i, 1] * Math.Sin(Math.PI * tau * S[i, 0]) * Math.Sin(Math.PI * tau * S[i, 0]) / (S[i, 0] * S[i, 0]);
            }
            return Math.Exp(-2 * e);
        }

    }
}
