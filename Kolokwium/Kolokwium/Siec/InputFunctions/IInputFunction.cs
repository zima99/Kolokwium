using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolokwium.Siec.InputFunctions
{
    public interface IInputFunction
    {
        double CalculateInput(List<Synapse> inputs);
    }
}
