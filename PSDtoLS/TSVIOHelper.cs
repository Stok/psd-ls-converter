using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace PSDtoLS
{
    public class TSVIOHelper
    {
        string input_file_name, output_file_name, gamma_file_name = null;
        string[] parameters;
        bool gamma_save = true;

        public TSVIOHelper(params string[] p)
        {
            this.input_file_name = p[0];
            this.output_file_name = p[1];
            if (p.Length > 7)
            {
                gamma_file_name = p[7];
                gamma_save = false;
            }
            parameters = new string[p.Length - 2];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = p[i + 2];
            }
        }

        public Data InitializeDataObject()
        {
            Data d = new Data(readFile(input_file_name), parameters);
            if (gamma_file_name != null)
            {
                d.gamma = readFile(gamma_file_name);
                d.REUSE_GAMMA = true;
            }
            return d;
        }

        public void WriteResultToFile(Data d)
        {
            writeFile(output_file_name, d.lineshape_data);
            if (gamma_save == true)
            {
                writeFile("gamma_" + output_file_name, d.gamma);
            }
        }
        private void writeFile(string file_name, double[,] data)
        {
            bool oride = false;
            try
            {
                if (System.IO.File.Exists(file_name))
                {
                    Console.WriteLine("Overwrite existing file called " + file_name.ToString() +"? y/n");
                    ConsoleKeyInfo k = Console.ReadKey(false);
                    Console.WriteLine("");
                    if (k.KeyChar == 'y')
                    {
                        System.IO.File.Delete(file_name);
                        oride = true;
                    }
                }
                else { oride = true; }
               
            }
            catch { }
            if (oride)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_name, true))
                {
                    for (int i = 0; i < data.Length / 2; i++)
                    {
                        file.WriteLine(data[i, 0].ToString() + '\t' + data[i, 1].ToString());
                    }

                }
            }
        }
        private double[,] readFile(string file)
        {
            TextFieldParser parser = new TextFieldParser(file);
            parser.Delimiters = new string[] { "\t" };
            List<string[]> lst = new List<string[]>();
            while (!parser.EndOfData)
            {
                lst.Add(parser.ReadFields());
            }

            double[,] s = new double[lst.Count, 2];
            for (int i = 0; i < lst.Count; i++)
            {

                s[i, 0] = Convert.ToDouble(lst[i][0].Replace(",", "."));
                s[i, 1] = Convert.ToDouble(lst[i][1].Replace(",", "."));
            }
            return s;
        }

    }
}
