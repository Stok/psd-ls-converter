﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSDtoLS
{
    public class Data
    {
        public readonly double[,] psd_data;
        public readonly double integration_time, maxSampleRate;
        public readonly int psd_length;

        public double[,] gamma;

        public double[,] lineshape_data;
        public double[] lineshape_limits;

        public bool REUSE_GAMMA = false;

        //parameters: max tau sample rate, units, lineshape min, max, interval 
        public Data(double[,] data, params string[] p)
        {
            psd_length = data.Length / 2;
            psd_data = new double[psd_length, 2];
            maxSampleRate = Convert.ToInt32(p[0]);
            if (p[1] == "dB")
            {
                for (int i = 0; i < psd_length; i++)
                {
                    psd_data[i, 0] = data[i, 0];
                    psd_data[i, 1] = Math.Pow(10, data[i, 1] / 10);
                }
            }
            else
            {
                this.psd_data = data;
            }
            lineshape_limits = new double[3] { Convert.ToDouble(p[2]), Convert.ToDouble(p[3]), Convert.ToDouble(p[4]) };
            integration_time = psd_data[0, 0];
            if (integration_time < lineshape_limits[2])
            {
                Console.WriteLine("Warning, you have requested a lineshape with a higher resolution than the integration time of the PSD. Results may be weird.");
            }
            gamma = new double[Convert.ToInt32(integration_time * maxSampleRate), 2];
        }

        public void EvaluateACFunction()
        {
            AutoCorrelationFnTerm2 term2 = new AutoCorrelationFnTerm2();
            term2.Initialize(psd_data);
            double k = (1 / maxSampleRate);
            for (int i = 0; i < (maxSampleRate * integration_time); i++)
            {
                gamma[i, 0] = k;
                gamma[i, 1] = term2.Evaluate(gamma[i, 0]);
                k = k + (1 / maxSampleRate);
            }
        }

        public double[,] EvaluateLineShape()
        {
            Fourier fourier = new Fourier();
            fourier.Initialize(gamma, 1);

            lineshape_data = new double[1 + Convert.ToInt32((lineshape_limits[1] - lineshape_limits[0]) / lineshape_limits[2]), 2];
            double del = lineshape_limits[0];

            try
            {
                System.IO.File.Delete("temp.TSV");
            }
            catch { }

            for (int i = 0; i < lineshape_data.Length / 2; i++)
            {
                lineshape_data[i, 0] = del;
                lineshape_data[i, 1] = fourier.Evaluate(del);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.TSV", true))
                {
                    file.WriteLine(del.ToString() + '\t' + lineshape_data[i, 1].ToString());
                }
                //Console.WriteLine(del.ToString() + '\t' + lineshape_data[i, 1].ToString());
                del = del + lineshape_limits[2];
            }
            return lineshape_data;
        }
    }
}
