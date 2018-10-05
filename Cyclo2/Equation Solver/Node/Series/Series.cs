using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    abstract class Series : Node
    {
        private int from, to;
        private string iterator;
        private Node element;
        private Func<double, double, double> operation;
        public Series(Node element, int from, int to, string iterator)
        {
            if (to < from) throw new Exception("Wrong iteration exception");
            this.From = from;
            this.To = to;
            this.Iterator = iterator;
            this.Element = element;
        }

        public override string Display => " ( " + this.Signature + "<" + Iterator + "," + from + ":" + to + ">" + this.Element.Display + " ) ";

        public int From { get => from; set => from = value; }
        public int To { get => to; set => to = value; }
        public string Iterator { get => iterator; set => iterator = value; }

        public override List<string> ContainedVariables
        {
            get
            {
                List<string> ret = this.element.ContainedVariables;
                ret.RemoveAll((x) => x == Iterator);
                return ret;
            }
        }

        public override Series TryToGetAsSeries => this;

        internal Node Element { get => element; set => element = value; }

        public override bool Compare(Node node)
        {
            Series ser = node.TryToGetAsSeries;
            if (ser != null)
            {
                return (this.To == ser.To) && (this.From == ser.From) && (this.iterator == ser.iterator) && this.Compare(ser);
            }
            else return false;
        }

        public override Node ConvertWith(Func<Node, Node> converter)
        {
            this.element.ConvertWith(converter);
            return converter(this);
        }

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            evaluations.Remove(iterator);
            element = element.Simplify(evaluations);
            return element;
        }

        public abstract MultiNode Iterate { get; }
        public Func<double, double, double> Operation { get => operation; set => operation = value; }
    }
}
