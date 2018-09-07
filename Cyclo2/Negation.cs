using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Negation : Node
    {
        private Node negated;
        public Negation(Node negated) => this.negated = negated;
        public override List<string> GetContainedVariables() => negated.GetContainedVariables();

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            negated = negated.Simplify(evaluations);
            Value value = negated.TryToGetAsValue();
            if (value == null) return this;
            else return new Value(0 - value.GetValue);
        }
    }
}
