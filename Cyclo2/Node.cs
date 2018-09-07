using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    abstract class Node
    {
        abstract public List<string> ContainedVariables { get; }
        abstract public Node Simplify(Dictionary<String, Double> evaluations);
    }
}
