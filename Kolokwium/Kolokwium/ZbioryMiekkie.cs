using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kolokwium
{
    static class ZbioryMiekkie
    {
        public static void DoSoftSets (double[][] data, double[] wagi)
        {
            double[] suma = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                double x = 0.0;
                for (int j = 0; j < wagi.Length; j++)
                {
                    x += wagi[j] * data[i][j];
                }
                suma[i] = x;
            }
            double max = suma.Max();
            
            for (int i = 0; i < suma.Length; i++)
            {
                if (max == suma[i])
                {
                    Console.WriteLine($"Dopasowana osoba to: osoba nr {i + 1}");
                }
            }
        }
        public static void Norm(this double[][] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                double nmin = 0;
                double nmax = 1;
                double min = data[i][0];
                double max = data[i][0];
                for (int j = 0; j < data[0].Length - 1; j++)
                {
                    if (data[i][j] < min)
                    {
                        min = data[i][j];
                    }
                    else if (data[i][j] > max)
                    {
                        max = data[i][j];
                    }

                }
                for (int j = 0; j < data[0].Length - 1; j++)
                {
                    //działanie normalizacji z pominięciem ostatniej kolmumny
                    data[i][j] = (data[i][j] - min) / (max - min) * (nmax - nmin);
                    data[i][j] = Math.Round(data[i][j], MidpointRounding.AwayFromZero);
                }
            }
        }

        public static double[][] Load(string p)
        {
            //czytanie kolejnch linii i zapis do tablicy
            string[] lines = File.ReadAllLines(p);
            lines = lines.Skip(1).ToArray(); //pominięcie pierwszego wiersza z nazwami kolumn
            double[][] data = new double[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');
                data[i] = new double[tmp.Length];
                for (int j = 0; j < tmp.Length; j++)
                {
                    data[i][j] = Convert.ToDouble(tmp[j].Replace('.', ','));

                }
            }
            return data;

        }
    }
}
