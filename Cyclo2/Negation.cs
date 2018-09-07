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

        public override string Signature => " - ";

        public override List<string> ContainedVariables => negated.ContainedVariables;

        public override string Display => "( - " + negated.Display + " )";

        public override Node ParseWith(Func<Node, Node> parser)
        {
            negated = parser(negated);
            return parser(this);
        }

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            negated = negated.Simplify(evaluations);
            Value value = negated.TryToGetAsValue();
            if (value == null) return this;
            else return new Value(0 - value.GetValue);
        }
    }
}
