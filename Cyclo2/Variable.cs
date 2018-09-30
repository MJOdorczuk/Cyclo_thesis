using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Variable : Node
    {
        private readonly string name;

        public Variable(string name) => this.name = name;

        public override List<string> ContainedVariables => new List<string> { name };

        public override string Signature => "Variable";

        public override string Display => name;

        public override Node ParseWith(Func<Node, Node> parser) => parser(this);

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            double value = new double();
            if (evaluations.TryGetValue(name, out value)) return new Value(value);
            else return this;
        }

        public override bool Compare(Node node)
        {
            Variable var = node.TryToGetAsVariable;
            if (var != null)
            {
                return this.name == var.name;
            }
            else return false;
        }

        public override Variable TryToGetAsVariable => this;
    }
}
