using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class MultiMultiplication : MultiNode
    {
        public MultiMultiplication(List<Node> elements) : base(elements)
        {
            this.operation = (x) =>
            {
                double ret = this.NeutralValue;
                foreach (double val in x)
                {
                    ret *= val;
                }
                return ret;
            };
        }

        public override double NeutralValue => 1;

        public override bool IsAssociative => true;

        public override bool IsCommutative => true;

        public override string Signature => " * ";

        public override BiNode ToBiNode
        {
            get
            {
                if (Elements.Count == 0) return new Multiplication(new Value(1), new Value(1));
                if (Elements.Count == 1) return new Multiplication(Elements[0], new Value(1));
                Elements.Reverse();
                Multiplication ret = new Multiplication(Elements[1], Elements[0]);
                for(int i = 2; i < Elements.Count; i++)
                {
                    ret = new Multiplication(Elements[i], ret);
                }
                Elements.Reverse();
                return ret;
            }
        }

        public override MultiNode Clone(List<Node> elements)
        {
            return new MultiMultiplication(elements);
        }
    }
}
