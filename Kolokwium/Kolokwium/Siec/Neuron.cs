using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kolokwium.Siec.ActivationFunctions;
using Kolokwium.Siec.InputFunctions;

namespace Kolokwium.Siec
{
    public class Neuron
    {
        //listy synaps wejsciowe i wyjsciowe
        public List<Synapse> Inputs { get; set; } = new List<Synapse>();
        public List<Synapse> Outputs { get; set; } = new List<Synapse>();

        //funkcje
        public IActivationFunction ActivationFunction { get; set; }
        public IInputFunction InputFunction { get; set; }
        
        //wartosci wejscia, wyjscia
        public double OutputValue { get; set; }
        public double InputValue { get; set; }
        public double Gradient { get; set; }

        public Neuron(IActivationFunction activationFunction, IInputFunction inputFunction)
        {
            //Konstruktor Neurona zawierajacy funkcje aktywacyjne i wejsciowe
            ActivationFunction = activationFunction;
            InputFunction = inputFunction;
        }
        public double CalculateOutput()
        {
            if (Inputs.Count == 0) 
            {
                return InputValue;
            }
            InputValue = InputFunction.CalculateInput(Inputs); //liczone wejscie za pomoca danych wyjsciowych z poprzedniego neurona i funkcji Input
            OutputValue = ActivationFunction.Calculate(InputValue); // liczone wyjscie za pomocja danych wejsciowych i funkcji aktywacyjnej
            return OutputValue;
        }
        public void PushValueOnOutput(double outputValue)
        {
            Outputs.ForEach(output => output.Output = outputValue); //synapsy wyjsciowe
        }
    }
}
