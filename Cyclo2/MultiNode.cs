using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    abstract class MultiNode : Node
    {
        private List<Node> elements;
        protected Func<List<double>, double> operation;

        public MultiNode(List<Node> elements) => this.Elements = elements;

        public override List<string> ContainedVariables
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (Node element in elements)
                {
                    ret.AddRange(element.ContainedVariables);
                }
                return ret.Distinct().ToList();
            }
        }

        public override string Display
        {
            get
            {
                string ret = "( ";
                for (int i = 0; i < elements.Count - 1; i++)
                {
                    ret += elements[i].Display + this.Signature;
                }
                if (elements.Count > 0) return ret + elements[elements.Count - 1].Display + " )";
                else return "";
            }
        }

        public Func<List<double>, double> Operation { get => operation; }
        public List<Node> Elements { get => elements; set => elements = value; }

        public override MultiNode TryToGetAsMultiNode => this;

        public override Node ParseWith(Func<Node, Node> parser)
        {
            List<Node> parsed = new List<Node>();
            foreach(Node element in elements)
            {
                parsed.Add(element.ParseWith(parser));
            }
            elements = parsed;
            return parser(this);
        }

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            List<Node> simplified = new List<Node>();
            double value = this.NeutralValue;
            bool valueInARow = false;
            foreach (Node element in elements)
            {
                Node node = element.Simplify(evaluations);
                Value val = node.TryToGetAsValue;
                if (val != null)
                {
                    value = this.operation(new List<double>() { value, val.GetValue });
                    valueInARow = true;
                }
                else
                {
                    if (!this.IsAssociative&& valueInARow && value != this.NeutralValue)
                    {
                        simplified.Add(new Value(value));
                        value = this.NeutralValue;
                    }
                    valueInARow = false;
                    simplified.Add(node);
                }
            }
            this.elements = simplified;
            if (elements.Count == 0) return new Value(value);
            else if (value != this.NeutralValue) elements.Add(new Value(value));
            return this;
        }

        public abstract double NeutralValue { get; }
        public abstract bool IsAssociative { get; }

        public abstract bool IsCommutative { get; }

        public abstract MultiNode Clone(List<Node> elements);
        public abstract BiNode ToBiNode { get; }
    }
}
