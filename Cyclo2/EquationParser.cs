using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class EquationParser
    {
        private readonly Node left, right;
        private readonly List<Func<Node, Node>> parseGramatic;
        public EquationParser(Node left, Node right)
        {
            this.left = left;
            this.right = right;
            parseGramatic = new List<Func<Node, Node>>();
            parseGramatic.Add((x) => NegationParser(x));

        }
        private Node NegationParser(Node node)
        {
            UniNode uni = node.TryToGetAsUniNode;
            if (uni == null) return node;
            else
            {
                if (uni.Signature == new Negation(null).Signature)
                {
                    UniNode uni2 = uni.Under.TryToGetAsUniNode;
                    BiNode bin = uni.Under.TryToGetAsBiNode;
                    Value val = uni.Under.TryToGetAsValue;
                    if(uni2 != null)
                    {
                        if(uni2.Signature == new Negation(null).Signature)
                        {
                            return uni2.Under;
                        }
                    }
                    else if(bin != null)
                    {
                        if(bin.Signature == new Sum(null,null).Signature)
                        {
                            return new Sum(new Negation(bin.Left), new Negation(bin.Right));
                        }
                        else if(bin.Signature == new Multiplication(null,null).Signature)
                        {
                            return new Multiplication(new Value(-1), bin);
                        }
                    }
                    else if(val != null)
                    {
                        return new Value(val.GetValue * -1);
                    }
                }
                else return node;
            }
            return node;
        }
        private Node BasicMultiplicationParser(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if (bin != null)
            {
                if(bin.Signature == new Multiplication(null,null).Signature)
                {
                    BiNode sumLeft = bin.Left.TryToGetAsBiNode;
                    BiNode sumRight = bin.Right.TryToGetAsBiNode;
                    if(sumLeft != null)
                    {
                        if(sumLeft.Signature == new Sum(null,null).Signature)
                        {
                            return new Sum(new Multiplication(sumLeft.Left, bin.Right), new Multiplication(sumLeft.Right, bin.Right));
                        }
                    }
                    else if(sumRight != null)
                    {
                        if(sumRight.Signature == new Sum(null,null).Signature)
                        {
                            return new Sum(new Multiplication(bin.Left, sumRight.Left), new Multiplication(bin.Left, sumRight.Right));
                        }
                    }
                }
            }
            return node;
        }
        private Node BasicSimplificationParser(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if(bin != null)
            {
                Value valLeft = bin.Left.TryToGetAsValue;
                Value valRight = bin.Right.TryToGetAsValue;
                if (valLeft != null && valRight != null)
                {
                    return new Value(bin.Operation(valLeft.GetValue, valRight.GetValue));
                }
                else if (valLeft != null) return node;
                BiNode binLeft = bin.Left.TryToGetAsBiNode;
                if (binLeft != null)
                {
                    if(bin.Signature == binLeft.Signature && bin.IsAssociative)
                    {
                        return BasicSimplificationParser(bin.Clone(binLeft.Left, bin.Clone(binLeft.Right, bin.Right)));
                    }
                }
                else
                {
                    Node right = BasicSimplificationParser(bin.Right);
                    BiNode binRight = right.TryToGetAsBiNode;
                    if(binRight != null)
                    {
                        Value val = binRight.Left.TryToGetAsValue;
                        if(val != null && bin.Signature == binRight.Signature)
                        {
                            return bin.Clone(val, bin.Clone(bin.Left, binRight.Right));
                        }
                    }
                }
            }
            return node;
        }
    }
}
