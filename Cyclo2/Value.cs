using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Value : Node
    {
        private readonly double value;

        public double GetValue => value;

        public Value(double value) => this.value = value;

        public override List<string> GetContainedVariables()
        {
            return new List<string>();
        }

        public override Node Simplify(Dictionary<string, double> evaluations) => this;

        public override Value TryToGetAsValue() => this;
    }
}
