using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Sum : Node
    {
        private Node left, right;
        public Sum(Node left, Node right)
        {
            this.left = left;
            this.right = right;
        }

        public override List<string> GetContainedVariables()
        {
            List<string> ret = left.GetContainedVariables();
            ret.AddRange(right.GetContainedVariables());
            // Dangerous part, not sure if won't crush. If so, use Hash instead or remove duplicates manualy.
            return ret.Distinct().ToList();
        }

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            left = left.Simplify(evaluations);
            right = right.Simplify(evaluations);
            Value leftValue = left.TryToGetAsValue();
            Value rightValue = right.TryToGetAsValue();
            if (leftValue == null || rightValue == null) return this;
            else return new Value(leftValue.GetValue + rightValue.GetValue);
        }
    }
}
