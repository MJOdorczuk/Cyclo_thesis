﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    abstract class UniNode : Node
    {
        private Node under;
        protected Func<double, double> operation;

        public Node Under { get => under; set => under = value; }

        public override List<string> ContainedVariables => under.ContainedVariables;

        public UniNode(Node under)
        {
            this.Under = under;
        }

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            under = under.Simplify(evaluations);
            Value underVal = under.TryToGetAsValue;
            if (underVal == null) return this;
            else return new Value(operation(underVal.GetValue));
        }

        public override Node ConvertWith(Func<Node, Node> parser)
        {
            under = under.ConvertWith(parser);
            return parser(this);
        }

        public override UniNode TryToGetAsUniNode => this;

        public abstract Node Clone(Node under);

        public override bool Compare(Node node)
        {
            UniNode uni = node.TryToGetAsUniNode;
            if(uni != null)
            {
                if(uni.Signature == this.Signature)
                {
                    return this.under.Compare(uni.under);
                }
            }
            return false;
        }
    }
}
