using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class SeriesSum : Series
    {
        public SeriesSum(Node element, int from, int to, string iterator) : base(element, from, to, iterator)
        {
            this.Operation = (x, y) => x + y;
        }

        public override MultiNode Iterate
        {
            get
            {
                List<Node> elements = new List<Node>();
                for(int i = this.From; i <= this.To; i++)
                {
                    elements.Add(this.Element.Simplify(new Dictionary<string, double>() { { this.Iterator, i } }));
                }
                return new MultiSum(elements);
            }
        }

        public override string Signature => "#Sigma";
    }
}
