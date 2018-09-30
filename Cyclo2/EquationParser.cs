using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class EquationParser
    {
        private readonly Division equation;
        private readonly List<Func<Node, Node>> parseGramatic;
        public EquationParser(Node left, Node right)
        {
            equation = new Division(left, right);
            parseGramatic = new List<Func<Node, Node>>
            {
                (x) => BasicNegationParser(x),
                (x) => BasicMultiplicationParser(x),
                (x) => BasicSimplificationParser(x),
                (x) => BasicDivisionParser(x),
                (x) => BasicPowerParser(x),
                (x) => BasicMultiNodeParser(x),
                (x) => BasicMultiMultiplicationParser(x)
            };

        }
        public Node Parse(Node node)
        {
            Node ret = node;
            foreach(Func<Node,Node> parser in parseGramatic)
            {
                ret = ret.ParseWith(parser);
            }
            return ret;
        }
        /*
         * (- - a) => a
         * (-(a + b)) => (-a) + (-b)
         * (-a) => (-1) * a
         */
        private Node BasicNegationParser(Node node)
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
                    if (uni2 != null)
                    {
                        if (uni2.Signature == new Negation(null).Signature)
                        {
                            return uni2.Under;
                        }
                    }
                    else if (bin != null)
                    {
                        if (bin.Signature == new Sum(null, null).Signature)
                        {
                            return new Sum(new Negation(bin.Left), new Negation(bin.Right));
                        }
                        else if (bin.Signature == new Multiplication(null, null).Signature)
                        {
                            return new Multiplication(new Value(-1), bin);
                        }
                    }
                    else if (val != null)
                    {
                        return new Value(val.GetValue * -1);
                    }
                    else return new Multiplication(new Value(-1), uni.Under);
                }
                else return node;
            }
            return node;
        }
        /*
         * (a + b) * c => a * c + b * c
         */
        private Node BasicMultiplicationParser(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if (bin != null)
            {
                if(bin.Signature == new Multiplication(null,null).Signature)
                {
                    BiNode sumLeft = bin.Left.TryToGetAsBiNode;
                    BiNode sumRight = bin.Right.TryToGetAsBiNode;
                    if (sumRight != null)
                    {
                        if (sumRight.Signature == new Sum(null, null).Signature)
                        {
                            return new Sum(BasicMultiplicationParser(new Multiplication(bin.Left, sumRight.Left)), BasicMultiplicationParser(new Multiplication(bin.Left, sumRight.Right)));
                        }
                    }
                    if (sumLeft != null)
                    {
                        if(sumLeft.Signature == new Sum(null,null).Signature)
                        {
                            return new Sum(BasicMultiplicationParser(new Multiplication(sumLeft.Left, bin.Right)), BasicMultiplicationParser(new Multiplication(sumLeft.Right, bin.Right)));
                        }
                    }
                }
            }
            return node;
        }
        /*
         * (4 * 5) => 20 (for all values and operations)
         * ((a @ b) @ c) => (a @ (b @ c)) (for associative)
         * (a @ 4 @ b) => (4 @ a @ b) (for associative and commutative)
         */
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
                        if(val != null && bin.Signature == binRight.Signature && bin.IsAssociative && bin.IsCommutative)
                        {
                            return bin.Clone(val, bin.Clone(bin.Left, binRight.Right));
                        }
                    }
                }
            }
            return node;
        }
        /*
         * a / b => a * (b ^ -1)
         */
        private Node BasicDivisionParser(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if(bin != null)
            {
                if(bin.Signature == new Division(null,null).Signature)
                {
                    return new Multiplication(bin.Left, new Power(bin.Right, new Value(-1)));
                }
            }
            return node;
        }
        /*
         * (a * b) ^ c => (a ^ c) * (b ^ c)
         * (a ^ b) ^ c => a ^ (b * c)
         */
        private Node BasicPowerParser(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if(bin != null)
            {
                if(bin.Signature == new Power(null,null).Signature)
                {
                    BiNode left = bin.Left.TryToGetAsBiNode;
                    if(left != null)
                    {
                        if(left.Signature == new Multiplication(null,null).Signature)
                        {
                            return new Multiplication(BasicPowerParser(new Power(left.Left, bin.Right)), BasicPowerParser(new Power(left.Right, bin.Right)));
                        }
                        else if(left.Signature == new Power(null,null).Signature)
                        {
                            Value a = bin.Right.TryToGetAsValue;
                            Value b = left.Right.TryToGetAsValue;
                            if (a != null && b != null)
                            {
                                if (a.GetValue * b.GetValue != 1)
                                    return new Power(left.Left, new Value(a.GetValue * b.GetValue));
                                else return left.Left;
                            }
                            else return new Power(left.Left, new Multiplication(bin.Right, left.Right));
                        }

                    }
                }
            }
            return node;
        }
        /*
         * (a @ (b @ c)) => (a @@ b @@ c)
         * (a @@ (b @ c)) => (a @@ b @@ c)
         * (a @@ (b @@ c)) => (a @@ b @@ c)
         * Same with flatten function.
         */
        private Node BasicMultiNodeParser(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if(bin != null)
            {
                if(bin.IsAssociative)
                {
                    return BasicMultiNodeParser(bin.ToMultiNode);
                }
            }
            MultiNode mul = node.TryToGetAsMultiNode;
            if(mul != null)
            {
                foreach(Node element in mul.Elements.ToArray())
                {
                    bin = element.TryToGetAsBiNode;
                    if(bin != null)
                    {
                        if(bin.Signature == mul.Signature)
                        {
                            mul.Elements.Remove(element);
                            mul.Elements.AddRange(new List<Node>() { bin.Left, bin.Right });
                        }
                    }
                    MultiNode eMul = element.TryToGetAsMultiNode;
                    if(eMul != null)
                    {
                        if(eMul.Signature == mul.Signature)
                        {
                            mul.Elements.Remove(element);
                            mul.Elements.AddRange(eMul.Elements);
                        }
                    }
                }
            }
            return node;
        }
        /*
         * (5 * 4 * a * 3) => (60 * a)
         */
         private Node BasicMultiMultiplicationParser(Node node)
        {
            MultiNode mul = node.TryToGetAsMultiNode;
            if(mul != null)
            {
                if(mul.Signature == new MultiMultiplication(null).Signature)
                {
                    List<Node> elements = new List<Node>(mul.Elements);
                    double multiplier = 1;
                    foreach(Node element in elements)
                    {
                        Value val = element.TryToGetAsValue;
                        if(val != null)
                        {
                            multiplier *= val.GetValue;
                            elements.Remove(element);
                        }
                    }
                    if (multiplier == 0) return new Value(0);
                    else if (multiplier != 1) elements.Add(new Value(multiplier));
                    return new MultiMultiplication(elements);
                }
            }
            return node;
        }
        /*
         * ((2 * a) + (3 * a) + (4 * a) + b) => ((9 * a) + b) (for all values and any number of elements)
         */
         private Node BasicMultiplicatorComposeParser(Node node)
        {
            MultiNode mul = node.TryToGetAsMultiNode;
            if (mul.Signature == new MultiSum(null).Signature)
            {
                List<Node> components = new List<Node>();
                List<Node> elements = new List<Node>(mul.Elements);
                foreach (Node element in elements)
                {
                    elements.Remove(element);
                    Value val = element.TryToGetAsValue;
                    if (val != null)
                    {
                        components.Add(element);
                    }
                    else
                    {
                        MultiNode submul = element.TryToGetAsMultiNode;
                        double multiplier = 1;
                        Node value;
                        if (submul != null)
                        {
                            List<Node> submul_elements = new List<Node>(submul.Elements);
                            if (submul.Signature == new MultiMultiplication(null).Signature)
                            {
                                value = submul_elements.Find((x) => x.TryToGetAsValue != null);
                                if (value != null)
                                {
                                    submul_elements.Remove(value);
                                    multiplier = value.TryToGetAsValue.GetValue;
                                    submul = new MultiMultiplication(submul_elements);
                                }
                            }
                        }
                        foreach (Node relement in elements)
                        {
                            MultiNode rsubmul = relement.TryToGetAsMultiNode;
                            double mult = 1;
                            if (rsubmul != null)
                            {
                                List<Node> rsubmul_elements = new List<Node>(rsubmul.Elements);
                                if (rsubmul.Signature == new MultiMultiplication(null).Signature)
                                {
                                    value = rsubmul_elements.Find((x) => x.TryToGetAsValue != null);
                                    if (value != null)
                                    {
                                        rsubmul_elements.Remove(value);
                                        mult = value.TryToGetAsValue.GetValue;
                                        rsubmul = new MultiMultiplication(rsubmul_elements);
                                    }
                                }
                            }
                            if (submul.Compare(rsubmul))
                            {
                                multiplier += mult;
                                elements.Remove(relement);
                            }
                        }
                        if (multiplier != 1) submul.Elements.Add(new Value(multiplier));
                        components.Add(submul);
                    }
                }
                return node;
            }
        }
        // Not complete
        /*private Func<Node,Node> BasicSeparationParserCreator(string name)
        {
            return (node) =>
            {
                Variable var = node.TryToGetAsVariable;
                BiNode bin = node.TryToGetAsBiNode;
                return null;
            };
        }*/

    }
}
