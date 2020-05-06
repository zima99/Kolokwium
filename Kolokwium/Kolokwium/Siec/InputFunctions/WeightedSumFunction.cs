using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolokwium.Siec.InputFunctions
{
    public class WeightedSumFunction : IInputFunction
    {
        public double CalculateInput(List<Synapse> inputs) => inputs.Select(x => x.Weight * x.Output).Sum();
    }
}
