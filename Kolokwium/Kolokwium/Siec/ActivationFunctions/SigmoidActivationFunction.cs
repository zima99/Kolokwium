using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolokwium.Siec.ActivationFunctions
{
    public class SigmoidActivationFunction : IActivationFunction
    {
        public double Calculate(double input) => 1 / (1 + Math.Exp(-input));
        public double Derivative(double input) => Calculate(input) * (1 - Calculate(input));
    }
}
