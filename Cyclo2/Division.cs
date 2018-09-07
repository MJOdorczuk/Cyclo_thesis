using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Division : BiNode
    {
        public Division(Node left, Node right) : base(left, right)
        {
            this.operation = (x, y) => x / y;
        }

        public override string Signature => " / ";
    }
}
