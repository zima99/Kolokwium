using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolokwium.Siec
{
    public class Synapse
    {
        //neurony wyjsciowy i wejsciowy
        private Neuron fromNeuron; 
        private Neuron toNeuron; 

        //waga i informacja wyjsciowa
        public double Weight { get; set; }
        public double Output { get; set; }

        private static readonly Random random = new Random();

        public Synapse() { }
        public Synapse(Neuron fromNeuron, Neuron toNeuron)
        {
            this.fromNeuron = fromNeuron;
            this.toNeuron = toNeuron;
            Weight = random.NextDouble() - 0.5; //waga jest generowana losowo
        }

        //aktualizacja wagi w zależności od delty i szybkości nauczania
        public void UpdateWeight(double learnRate, double delta)
        {
            Weight += learnRate * delta; 
        }
    }
}
