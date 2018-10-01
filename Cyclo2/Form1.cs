using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cyclo2
{
    public partial class Form1 : Form
    {
        private Node TestEquation;
        public Form1()
        {
            InitializeComponent();
            //TestEquation = new Sum(new Power(new Variable("x"), new Negation(new Negation(new Value(5)))), new Multiplication(new Division(new Value(1), new Variable("y")), new Value(20)));
            //TestEquation = new Division(new Variable("a"), new Division(new Variable("b"), new Division(new Variable("c"), new Variable("d"))));
            /*Node TestBranch1 = new Multiplication(new Power(new Sum(new Variable("x"), new Power(new Value(5), new Variable("y"))), new Multiplication(new Variable("x"), new Value(2))),
                new Sum(new Variable("x"), new Division(new Value(1), new Variable("x"))));
            Node TestBranch2 = new Multiplication(new Value(3), new Sum(new Sum(new Sum(new Power(new Variable("z"), new Value(2)), new Multiplication(new Value(3), new Sum(new Variable("x"),
                new Value(5)))), new Multiplication(new Sum(new Value(2), new Variable("y")), new Sum(new Variable("y"), new Variable("z")))), new Variable("x")));
            TestEquation = new Multiplication(TestBranch1, TestBranch2);*/
            Node node1 = new Multiplication(new Sum(new Variable("a"), new Variable("b")), new Sum(new Variable("c"), new Variable("d"))); //((a+b)*(c+d))
            Node node2 = new Multiplication(new Sum(new Variable("c"), new Variable("c")), new Variable("c")); //((c+c)*c)
            Node node3 = new Sum(new Value(2), new Sum(new Variable("e"), new Variable("f"))); //(2+(e+f))
            Node node4 = new Multiplication(new Multiplication(new Multiplication(new Value(3), new Variable("b")), new Multiplication(new Variable("a"), new Variable("a"))),
                new Multiplication(new Multiplication(new Value(5), new Variable("b")), new Multiplication(new Value(4), new Variable("a")))); //(((3*a)*(a*a))*((5*a)*(4*a)))
            Node node5 = new Sum(new Multiplication(new Value(2), new Variable("a")), new Multiplication(new Value(5), new Variable("a")));
            Node node6 = new Sum(new Multiplication(new Variable("a"), new Variable("b")), new Multiplication(new Variable("a"), new Value(10)));
            //TestEquation = new Multiplication(new Sum(node1, node2), node3);
            TestEquation = new Multiplication(node5, node6);
            TestEquationDisplay.Text = TestEquation.Display + "\n";
        }

        private void TestEquationDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void EquationSolvingButton_Click(object sender, EventArgs e)
        {
            TestEquationDisplay.Text += TestEquation.Simplify(new Dictionary<string, double>()).Display + "\n";
            TestEquationDisplay.Text += string.Join(",",TestEquation.ContainedVariables.ToArray()) + "\n";
            Dictionary<string, double> tempdic = new Dictionary<string, double>
            {
                { "x", 5 }
            };
            TestEquationDisplay.Text += TestEquation.Simplify(tempdic).Display + "\n";
            TestEquationDisplay.Text += new EquationConverter(null, null).Convert(TestEquation).Display + "\n";
        }
    }
}
