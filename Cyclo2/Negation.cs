using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Negation : UniNode
    {
        public Negation(Node under) : base(under)
        {
            operation = x => 0 - x;
        }
        public override string Signature => " - ";

        public override string Display => "( - " + Under.Display + " )";
    }
}
