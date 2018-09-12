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

        public override string Signature => "Value";

        public Value(double value) => this.value = value;

        public override List<string> ContainedVariables => new List<string>();

        public override string Display => value.ToString();

        public override Node Simplify(Dictionary<string, double> evaluations) => this;

        public override Value TryToGetAsValue => this;

        public override Node ParseWith(Func<Node, Node> parser) => parser(this);
    }
}
