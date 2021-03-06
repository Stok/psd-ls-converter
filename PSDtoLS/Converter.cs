using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using libpsdls;



namespace PSDtoLS
{
    /// <summary>
    /// This is a program for calculating the lineshape of a laser from power spectral density curves.
    /// Inputs should be in tsv format : 
    /// f1  v1
    /// f2  v2
    /// ...
    /// Output will be in this same format.
    /// 
    /// Command line usage: "Usage: PSDToLS.exe input_file.tsv output_file.tsv max_fourier_sample_rate psd_units, lineshape min, max, interval, gamma_file_if_exists");
    /// </summary>
    class Converter
    {
        static void Main(string[] args)
        {
            TSVIOHelper ioHelper;
            Data d;
                if (args.Length > 6)
                {
                    ioHelper = new TSVIOHelper(args);
                    if (double.Parse(args[2], NumberFormatInfo.InvariantInfo) / 2 < Math.Abs(double.Parse(args[5], NumberFormatInfo.InvariantInfo)))
                    {
                        Console.WriteLine("Warning: Niquist rate not met for desired output range. You may get strange output.");
                    }
                    try
                    {
                        d = ioHelper.InitializeDataObject();
                    }
                    catch (TSVIOHelper.InvalidDataFileException)
                    {
						Console.WriteLine ("Invalid Input Data. File either not found or corrupted.");
                        return;
                    }
                }
                else 
                {
                    Console.WriteLine("Usage: PSDToLS.exe input_file.tsv output_file.tsv max_fourier_sample_rate psd_units, lineshape min, max, interval");
                    return;
                }
           
            //-----------------------------------------------------------

            Console.WriteLine("Calculating the autocorrelation function's second term...");
            if (!d.REUSE_GAMMA)
            {
                d.EvaluateACFunction();
            }
            Console.WriteLine("Done");

            //-----------------------------------------------------------
            Console.WriteLine("Evaluating Lineshape");
            d.EvaluateLineShape();
            ioHelper.WriteResultToFile(d);
            Console.WriteLine("Done");
           
            Console.Read();
            
        }

    }
}
