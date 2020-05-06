using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kolokwium
{
    static class LoadData
    {
        //wczytanie danych z pliku
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
            }return data;

        }   
        //Normalizacja dla tablicy dwuwymiarowej 
        public static void Normalize(this double[][] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                double nmin = 0;
                double nmax = 1;
                //szukanie wartosci minimalnej i maksymalnej
                double min = data[i][0];
                double max = data[i][0];
                for (int j = 0; j < data[0].Length-1; j++)
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
                for (int j = 0; j < data[0].Length-1; j++)
                {
                    //działanie normalizacji z pominięciem ostatniej kolmumny
                    data[i][j] = (data[i][j] - min) / (max - min) * (nmax - nmin);
                }
            }
        }
        //Normalizacja dla pojedynczego wejscia
        public static void Normalize(this double[] data)
        {
            double nmin = 0;
            double nmax = 1;
            double min = data[0];
            double max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < min)
                {
                    min = data[i];
                }
                else if (data[i] > max)
                {
                    max = data[i];
                }
            }
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (data[i] - min) / (max - min) * (nmax - nmin);
            }
        }
        //Tasowanie danych w tabeli
        public static void Shuffle(this double[][] data)
        {
            Random random = new Random();
            for (int i = 0; i < data.Length; i++)
            {
                double[] tmp = data[i];
                int a = random.Next(i, data.Length);
                data[i] = data[a];
                data[a] = tmp;
            }
        }

        //podzielenie danych na dwie czesci
        public static double[][][] Part(this double[][] data) 
        {
            double[][][] outArray = new double[2][][];
            //ustalanie wielkosci tablic
            int part = (int)(data.Length * 0.3);
            outArray[0] = new double[part][];
            outArray[1] = new double[data.Length - part][];

            //30% danych
            for (int i = 0; i < part; i++)
            {
                outArray[0][i] = new double[data[i].Length];
                for (int j = 0; j < data[i].Length; j++)
                {
                    outArray[0][i][j] = data[i][j];
                }
            }

            //70% danych
            for (int i = part; i < data.Length; i++)
            {
                outArray[1][i - part] = new double[data[i].Length];
                for (int j = 0; j < data[i].Length; j++)
                {
                    outArray[1][i - part][j] = data[i][j];
                }
            }

            return outArray;
        }

        //Pobranie wartosci z tabeli bez ostatniej kolumny
        public static double[][] GetInputs(this double[][] data)
        {
            double[][] outArray = (double[][])data.Clone();
            for (int i = 0; i < data.Length; i++)
            {
                Array.Resize(ref outArray[i], outArray[i].Length - 1); //bez ostatniej kolumny
            }

            return outArray;
        }

        //pobranie wartosci z tabeli z ostatniej kolumny
        public static double[] GetOutputs(this double[][] data)
        {
            double[] outArray = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                outArray[i] = data[i][8];
                
            }

            return outArray;
        }
    }

}
