using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    abstract class Node
    {
        public abstract List<string> ContainedVariables { get; }

        abstract public Node Simplify(Dictionary<String, Double> evaluations);
        public virtual Value TryToGetAsValue() { return null; }
        public abstract Node ParseWith(Func<Node, Node> parser);
        public abstract string Signature { get; }
        public abstract string Display { get; }
    }
}
