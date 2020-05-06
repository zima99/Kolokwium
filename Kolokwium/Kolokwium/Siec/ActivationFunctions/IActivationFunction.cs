using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolokwium.Siec.ActivationFunctions
{
    public interface IActivationFunction
    {
        double Calculate(double input);
        double Derivative(double input);
    }
}
