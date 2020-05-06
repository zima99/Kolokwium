using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Kolokwium.Siec;
using Kolokwium.Siec.ActivationFunctions;
using Kolokwium.Siec.InputFunctions;

namespace Kolokwium
{
    class Program
    {
        static void Main(string[] args)
        {
            //zadanie 1
            //wczytanie danych z bazy
            double[][] data = LoadData.Load("diabetes.csv");
            data.Shuffle(); //normalizacja i tasowanie
            data.Normalize();
            

            //zadanie 2
            //wczytanie danych z bazy
            double[][] data2 = ZbioryMiekkie.Load("diabetes.csv");
            data2.Norm(); //normalizacja (z przybliżeniem do 0 i 1)
            //dopasowanie odpowiedniej osoby na podstawie wag
            double[] weights = new double[] { 0.4, 0.7, 0.3, 0.5, 0.2, 0.4, 0.6, 0.5, 0};
            ZbioryMiekkie.DoSoftSets(data2, weights);

            //zadanie 3

            double[][][] part = data.Part(); //podzial na 70% i 30%
            //dane treningowe na wejscie i oczekiwane wartosci
            double[][] trainInput = LoadData.GetInputs(part[1]);
            double[] trainExpected = LoadData.GetOutputs(part[1]);
            //dane testowe na wejscie i oczekiwane wartosci
            double[][] testInput = LoadData.GetInputs(part[0]);
            double[] testExpected = LoadData.GetOutputs(part[0]);
            //Siec neuronowa 
            Network network = new Network(0.01, new WeightedSumFunction(), new SigmoidActivationFunction(), 8, 5, 5, 5, 3, 1);
            Console.WriteLine("Trenowanie...");
            network.LoadWeights(); //odczyt pliku
            network.Train(trainInput, trainExpected, 10000); //trenowanie sieci
            network.SaveWeights(); //zapis do pliku
            //testowanie dokladnosci sieci na 30% danych; dokladnosc powinna wynosic mniej wiecej 60-70%
            Console.WriteLine("Dokładność " + (network.Test(testInput, testExpected) * 100) + "%");
            //Druga Siec z takimi samymi wagami; dokladnosc powinna byc taka sama jak dla sieci wyzej
            Network network2 = new Network(0.01, new WeightedSumFunction(), new SigmoidActivationFunction(), 8, 5, 5, 5, 3, 1);
            network2.LoadWeights();
            Console.WriteLine("Dokładność " + (network2.Test(testInput, testExpected) * 100) + "%");
            Console.WriteLine("Koniec");
            Console.ReadKey();
        }
    }
}
