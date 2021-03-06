﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    abstract class BiNode : Node
    {
        protected Func<double, double, double> operation;
        private Node right;
        private Node left;

        public BiNode(Node left, Node right)
        {
            this.Left = left;
            this.Right = right;
        }
        public Node Left { get => left; set => left = value; }
        public Node Right { get => right; set => right = value; }
        public Func<double, double, double> Operation { get => operation; }

        public override List<string> ContainedVariables
        {
            get
            {
                List<string> ret = Left.ContainedVariables;
                ret.AddRange(Right.ContainedVariables);
                // Dangerous part, not sure if won't crush. If so, use Hash instead or remove duplicates manualy.
                return ret.Distinct().ToList();
            }
        }

        public override string Display => "( " + Left.Display + this.Signature + Right.Display + " )";

        public override Node Simplify(Dictionary<string, double> evaluations)
        {
            Left = Left.Simplify(evaluations);
            Right = Right.Simplify(evaluations);
            Value leftValue = Left.TryToGetAsValue;
            Value rightValue = Right.TryToGetAsValue;
            if (leftValue == null || rightValue == null) return this;
            else return new Value(operation(leftValue.GetValue,rightValue.GetValue));
        }
        public override Node ConvertWith(Func<Node, Node> converter)
        {
            this.Left = Left.ConvertWith(converter);
            this.Right = Right.ConvertWith(converter);
            return converter(this);
        }
        public override BiNode TryToGetAsBiNode => this;
        public abstract Node Clone(Node left, Node right);

        public override bool Compare(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if(bin != null)
            {
                if(bin.Signature == this.Signature)
                {
                    return this.Left.Compare(bin.Left) && this.Right.Compare(bin.Right);
                }
            }
            return false;
        }

        public abstract bool IsCommutative { get; }
        public abstract bool IsAssociative { get; }
        public abstract MultiNode ToMultiNode { get; }
    }
}
