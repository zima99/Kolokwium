using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Kolokwium.Siec.ActivationFunctions;
using Kolokwium.Siec.InputFunctions;

namespace Kolokwium.Siec
{
    public class Network
    {
        public double LearningRate { get; set; } // szybkosc nauczania
        public List<Layer> Layers { get; set; } = new List<Layer>(); // lsita warstw danej sieci

        //konstruktor sieci generujacy warstwy w zaleznosci od ilosci neuronow
        public Network(double learningRate, IInputFunction inputFunction, IActivationFunction activationFunction, params int[] numberOfNeurons)
        {
            LearningRate = learningRate;
            

            foreach(int number in numberOfNeurons)
            {
                Layer layer = new Layer();
                Layers.Add(layer);

                for (int i = 0; i < number; i++)
                {
                    layer.Neurons.Add(new Neuron(activationFunction, inputFunction));
                    //neurony maja te same funkcje aktywacyjne i wejsciowe
                }
            }
            ConnectLayers();
        }

        //trenowanie sieci; obliczanie bledu na podstawie wartosci wejsciowych i oczekiwanych; wagi sa modyfikowane na podstawie bledu; epochs to liczba powtorzen; 
        public void Train(double[][] inputValues, double[] expectedValues,int epochs)
        {
            for (int i = 0; i < epochs; i++)
            {
                for (int j = 0; j < inputValues.Length; j++)
                {
                    double[] outputs = Calculate(inputValues[j]);

                    CalculateErrors(outputs, expectedValues);

                    UpdateWeights();
                }
            }
        }

        //funkcja Calculate zwraca output
        public double[] Calculate(double[] input)
        {
            PushInputValues(input);

            double[] outputs = new double[Layers.Last().Neurons.Count];
            for (int i = 0; i < outputs.Length; i++)
            {
                Neuron currNeuron = Layers.Last().Neurons[i];
                outputs[i] = currNeuron.OutputValue;
            }
            return outputs;
        }

        //Funkcja Test sprawdza poprawnosc sieci
        public double Test(double[][] testInput, double[] testExpected)
        {
            double accuracy = 0;

            for (int i = 0; i < 1; i++)
            {
                double[] output = new double[1];
                for (int j = 0; j < testInput.Length; j++)
                {
                    //siec zwieksza wartosc dokladnosci jesli odniadnie wynik
                    output = Calculate(testInput[j]);
                    if(testExpected[j]==1 && output[i] > 0.95)
                    {
                        accuracy += 1;
                    }
                    else if (testExpected[j] == 0 && output[i] < 0.05)
                    {
                        accuracy += 1;
                    }
                }
            }
            return accuracy / testInput.Length; //dokladnosc dzielimy przez ilosc prob
        }

        //Wprowadzany jest input z pierwszej warstwy; output wyliczany z wszystkich neuronow w warstwach ukrytych
        public void PushInputValues(double[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                Neuron currNeuron = Layers.First().Neurons[i];
                currNeuron.OutputValue = currNeuron.InputValue = inputs[i];
            }
            for (int i = 0; i < Layers.Count; i++)
            {
                foreach (Neuron neuron in Layers[i].Neurons)
                {
                    neuron.PushValueOnOutput(neuron.CalculateOutput());
                }
            }
        }

        //Łaczenie kazdego z neuronow z poprzedniej warstwy z kazdym neuronem z nastepnej warstwy
        private void ConnectLayers()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                foreach (Neuron neuron in Layers[i].Neurons)
                {
                    foreach (Neuron prevNeuron in Layers[i-1].Neurons)
                    {
                        Synapse synapse = new Synapse(prevNeuron, neuron);
                        neuron.Inputs.Add(synapse);
                        prevNeuron.Outputs.Add(synapse);
                    }
                }
            }
        }
        private void CalculateErrors(double[] outputs, double[] expectedValues)
        {
            for (int i = 0; i < outputs.Length; i++) // błąd sieci dla ostatniej warstwy
            {
                Neuron currNeuron = Layers.Last().Neurons[i];
                currNeuron.Gradient = currNeuron.ActivationFunction.Derivative(currNeuron.InputValue) * (expectedValues[i]-outputs[i]);
                //obliczanie bledu na podstawie funkcji aktywacyjnej; zapisanie bledu jako gradientu dla kazdego neurona
            }
            //obliczany blad dla wszystkich warstw oprocz pierwszej i ostatniej na podstawie danych wyjsciowych z neurona z wczesniejszej warstwy
            for (int i = Layers.Count-2;i>0; i--)
            {
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                {
                    double d = 0;
                    for (int k = 0; k < Layers[i+1].Neurons.Count; k++)
                    {
                        d += Layers[i + 1].Neurons[k].Gradient * Layers[i].Neurons[j].Outputs[k].Weight;
                    }
                    Neuron currNeuron = Layers[i].Neurons[j];
                    currNeuron.Gradient = d * currNeuron.ActivationFunction.Derivative(currNeuron.InputValue);
                }
            }
        }
        private void UpdateWeights() //aktualizacja wag na podstawie wyliczonej delty(zaleznej od bledu i szybkosci nauczania)
        {
            for (int i = Layers.Count-1; i > 0; i--)
            {
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                {
                    for (int k = 0; k < Layers[i].Neurons.Count; k++)
                    {
                        double delta = Layers[i].Neurons[j].Gradient * Layers[i - 1].Neurons[k].OutputValue;
                        Layers[i].Neurons[j].Inputs[k].UpdateWeight(LearningRate, delta);
                    }
                }
            }
        }
        public void SaveWeights() //zapisanie wag
        {
            List<string> weights = new List<string>();
            foreach (Layer layer in Layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    foreach (Synapse synapse in neuron.Inputs)
                    {
                        weights.Add(synapse.Weight.ToString());
                    }
                }
            }
            File.WriteAllLines("weights.txt", weights);
        }
        public void LoadWeights() //zaladowanie wag
        {
            if (File.Exists("weights.txt"))
            {
                int i = 0;
                string[] lines = File.ReadAllLines("weights.txt");
                foreach (Layer layer in Layers)
                {
                    foreach (Neuron neuron in layer.Neurons)
                    {
                        foreach (Synapse synapse in neuron.Inputs)
                        {
                            synapse.Weight = Double.Parse(lines[i++]);
                        }
                    }
                }
            }
        }
    }


}
