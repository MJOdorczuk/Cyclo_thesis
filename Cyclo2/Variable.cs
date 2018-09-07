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
    }
}
