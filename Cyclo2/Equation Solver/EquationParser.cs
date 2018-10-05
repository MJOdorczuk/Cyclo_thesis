using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    static class EquationParser
    {
        private static readonly List<string> specialSigns = new List<string>()
        {
            new Sum(null,null).Signature,
            new Multiplication(null,null).Signature,
            new Division(null,null).Signature,
            new Power(null,null).Signature,
            new Negation(null).Signature,
            new SeriesSum(null,0,0,"").Signature,
            new SeriesMultiplication(null,0,0,"").Signature,
            new Equation(null,null).Signature
        };
        private static readonly Dictionary<string, Node> operations = new Dictionary<string, Node>()
        {
            {new Sum(null,null).Signature, new Sum(null,null) },
            {new Multiplication(null,null).Signature, new Multiplication(null,null) },
            {new Division(null, null).Signature, new Division(null,null) },
            {new Power(null,null).Signature, new Power(null,null) },
            {new Negation(null).Signature, new Negation(null) },
            {new SeriesSum(null,0,0,"").Signature, new SeriesSum(null,0,0,"") },
            {new SeriesMultiplication(null,0,0,"").Signature, new SeriesMultiplication(null,0,0,"") }
        };
        private static readonly Dictionary<string, uint> allowedPhrases = AllowedPhrases();
        private static readonly List<Func<List<string>, List<string>>> parseGramatic = new List<Func<List<string>, List<string>>>()
        {

        };
        static public Equation Read(string equation)
        {
            List<string> separated = BasicSeparator(equation);
            if (!BasicAllowedPhrasesChecker(separated)) return null;

            return null;
        }
        /*
         * "abcd" => { "a", "b", "c", "d" }
         */
        private static List<string> BasicSeparator(string equation)
        {
            return equation.ToList().ConvertAll(new Converter<char, string>((x) => x.ToString()));
        }
        private static bool BasicAllowedPhrasesChecker(List<string> equation)
        {
            foreach(KeyValuePair<string, uint> allowedphrase in allowedPhrases)
            {
                if (equation.FindAll((x) => x == allowedphrase.Key).Count > allowedphrase.Value) return false;
            }
            return false;
        }
        private static Dictionary<string,uint> AllowedPhrases()
        {
            Dictionary<string, uint> ret = new Dictionary<string, uint>();
            foreach(string sign in specialSigns)
            {
                if (sign != new Equation(null, null).Signature) ret.Add(sign, uint.MaxValue);
                else ret.Add(sign, 1);
            }
            for(char i = 'a'; i <= 'z'; i++)
            {
                ret.Add(i.ToString(), uint.MaxValue);
            }
            for(char i = '0'; i <= '9'; i++)
            {
                ret.Add(i.ToString(), uint.MaxValue);
            }
            for(char i = 'A'; i <= 'Z'; i++)
            {
                ret.Add(i.ToString(), uint.MaxValue);
            }
            for(char i = char.MinValue; i <= char.MaxValue; i++)
            {
                if(!ret.ContainsKey(i.ToString()))
                {
                    ret.Add(i.ToString(), 0);
                }
            }
            return ret;
        }
        public static Equation BasicEquationParser(string input)
        {
            string[] tokens = input.Split('=');
            if(tokens.Length == 2)
            {
                Node left = BasicSubParser(tokens[0]);
                Node right = BasicSubParser(tokens[1]);
                if (left == null && right == null) return null;
                else return new Equation(left, right);
            }
            else return null;
        }

        private static Node BasicSubParser(string v)
        {
            string[] tokens = TokenDigger(v);
            if (tokens == null) return null;
            else
            {
                Node parent = operations[tokens[1]];
                if (parent == null) return null;
                else return parent.
            }
        }

        private static string[] TokenDigger(string v)
        {
            throw new NotImplementedException();
        }
    }
}
