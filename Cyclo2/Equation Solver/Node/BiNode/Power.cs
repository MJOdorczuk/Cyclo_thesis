using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Power : BiNode
    {
        public Power(Node left, Node right) : base(left, right)
        {
            this.operation = (x, y) => Math.Pow(x, y);
        }

        public override string Signature => " ^ ";

        public override bool IsCommutative => false;

        public override bool IsAssociative => false;

        public override MultiNode ToMultiNode => throw new NotImplementedException();

        public override Node Clone(Node left, Node right) => new Power(left, right);
    }
}
