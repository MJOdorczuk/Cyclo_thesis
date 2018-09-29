using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class MultiSum : MultiNode
    {
        public MultiSum(List<Node> elements) : base(elements)
        {
            this.operation = (x) =>
            {
                double ret = this.NeutralValue;
                foreach (double val in x)
                {
                    ret += val;
                }
                return ret;
            };
        }
        public override string Signature => " + ";
        public override MultiNode Clone(List<Node> elements) => new MultiSum(elements);
        public override bool IsAssociative => true;
        public override bool IsCommutative => true;
        public override double NeutralValue => 0;
        public override BiNode ToBiNode
        {
            get
            {
                if (Elements.Count == 0) return new Sum(new Value(0), new Value(0));
                else if (Elements.Count == 1) return new Sum(Elements[0], new Value(0));
                Elements.Reverse();
                Sum ret = new Sum(Elements[1], Elements[0]);
                for(int i = 2; i < Elements.Count; i++)
                {
                    ret = new Sum(Elements[i], ret);
                }
                return ret;
            }
        }
    }
}
