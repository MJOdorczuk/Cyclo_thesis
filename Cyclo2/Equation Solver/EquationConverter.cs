using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class EquationConverter
    {
        private readonly Equation equation;
        private readonly List<Func<Node, Node>> convertGramatic;
        public EquationConverter(Node left, Node right)
        {
            equation = new Equation(left, right);
            /*
             * Creating gramatic for equations converting.
             * Converters are used in the order of the list.
             */
            convertGramatic = new List<Func<Node, Node>>
            {
                (x) => BasicNegationConverter(x),
                (x) => BasicMultiplicationConverter(x),
                (x) => BasicSimplificationConverter(x),
                (x) => BasicDivisionConverter(x),
                (x) => BasicPowerConverter(x),
                (x) => BasicSeriesConverter(x),
                (x) => BasicMultiNodeConverter(x),
                (x) => BasicMultiMultiplicationConverter(x),
                (x) => BasicMultiplicatorComposeConverter(x)
            };

        }
        public Node Convert(Node node)
        {
            Node ret = node;
            // Applying all converters
            foreach(Func<Node,Node> converter in convertGramatic)
            {
                ret = ret.ConvertWith(converter);
            }
            return ret;
        }
        /*
         * (- - a) => a
         * (-(a + b)) => (-a) + (-b)
         * (-a) => (-1) * a
         */
        private Node BasicNegationConverter(Node node)
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
        private Node BasicMultiplicationConverter(Node node)
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
                            return new Sum(BasicMultiplicationConverter(new Multiplication(bin.Left, sumRight.Left)), BasicMultiplicationConverter(new Multiplication(bin.Left, sumRight.Right)));
                        }
                    }
                    if (sumLeft != null)
                    {
                        if(sumLeft.Signature == new Sum(null,null).Signature)
                        {
                            return new Sum(BasicMultiplicationConverter(new Multiplication(sumLeft.Left, bin.Right)), BasicMultiplicationConverter(new Multiplication(sumLeft.Right, bin.Right)));
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
        private Node BasicSimplificationConverter(Node node)
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
                        return BasicSimplificationConverter(bin.Clone(binLeft.Left, bin.Clone(binLeft.Right, bin.Right)));
                    }
                }
                else
                {
                    Node right = BasicSimplificationConverter(bin.Right);
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
        private Node BasicDivisionConverter(Node node)
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
        private Node BasicPowerConverter(Node node)
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
                            return new Multiplication(BasicPowerConverter(new Power(left.Left, bin.Right)), BasicPowerConverter(new Power(left.Right, bin.Right)));
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
         * (#@<i:j>a) => (a(i) @@ a(i + 1) @@ ... @@ a(j))
         */
        private Node BasicSeriesConverter(Node node)
        {
            Series ser = node.TryToGetAsSeries;
            if (ser != null)
            {
                return ser.Iterate;
            }
            else return node;
        }
        /*
         * (a @ (b @ c)) => (a @@ b @@ c)
         * (a @@ (b @ c)) => (a @@ b @@ c)
         * (a @@ (b @@ c)) => (a @@ b @@ c)
         * Same with flatten function.
         */
        private Node BasicMultiNodeConverter(Node node)
        {
            BiNode bin = node.TryToGetAsBiNode;
            if(bin != null)
            {
                if(bin.IsAssociative)
                {
                    return BasicMultiNodeConverter(bin.ToMultiNode);
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
         private Node BasicMultiMultiplicationConverter(Node node)
        {
            MultiNode mul = node.TryToGetAsMultiNode;
            if(mul != null)
            {
                if(mul.Signature == new MultiMultiplication(null).Signature)
                {
                    List<Node> elements = new List<Node>();
                    double multiplier = 1;

                    foreach (Node element in mul.Elements)
                    {
                        Value val = element.TryToGetAsValue;
                        if (val != null)
                        {
                            multiplier *= val.GetValue;
                        }
                        else elements.Add(element);
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
         private Node BasicMultiplicatorComposeConverter(Node node)
        {
            MultiNode mul = node.TryToGetAsMultiNode;
            // This converter converts only multisums
            if(mul == null)
            {
                return node;
            }
            if (mul.Signature == new MultiSum(null).Signature)
            {
                // List of nodes that will appear in returned node as elements
                List<Node> components = new List<Node>();
                // Assistant list to avoid any changes in original list
                List<Node> elements = new List<Node>(mul.Elements);
                // As the elements list is changed during loop operation it is problematic to use foreach or for loop
                // This loop checks for duplicates and multiplied by some value duplicates in the list
                // The multipliers of the duplicates are sumed so all the duplicates can be replaced by one occurance with sumed multiplier
                // Every found duplicate and original are removed from the list, so, after checking whole list it should be empty
                while(elements.Count > 0)
                {
                    Node element = elements[0];
                    elements.Remove(element);
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
                        element = submul;
                    }
                    for (int i = 0; i < elements.Count; i++)
                    {
                        Node relement = elements[i];
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
                                if(element.Compare(rsubmul))
                                {
                                    multiplier += mult;
                                    elements.Remove(relement);
                                    i--;
                                }
                            }
                        }
                        else if(element.Compare(relement))
                        {
                            multiplier += mult;
                            elements.Remove(relement);
                            i--;
                        }
                    }
                    if (multiplier != 1)
                    {
                        if (submul != null)
                        {
                            submul.Elements.Add(new Value(multiplier));
                            element = submul;
                        }
                        else
                        {
                            element = new MultiMultiplication(new List<Node>() { new Value(multiplier), element });
                        }
                    }
                    components.Add(element);
                }
                return new MultiSum(components);
            }
            return node;
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
