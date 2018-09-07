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
        public Value(double value) => this.value = value;

        public override List<string> ContainedVariables => new List<string>();
    }
}
