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
            TestEquation = new Sum(new Power(new Variable("x"), new Negation(new Negation(new Value(5)))), new Multiplication(new Division(new Value(1), new Variable("y")), new Value(20)));
            TestEquationDisplay.Text = TestEquation.Display + "\n";
        }

        private void TestEquationDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void EquationSolvingButton_Click(object sender, EventArgs e)
        {
            TestEquationDisplay.Text += TestEquation.Simplify(new Dictionary<string, double>()).Display + "\n";
            TestEquationDisplay.Text += string.Join(",",TestEquation.ContainedVariables.ToArray()) + "\n";
            Dictionary<string, double> tempdic = new Dictionary<string, double>();
            tempdic.Add("x", 5);
            TestEquationDisplay.Text += TestEquation.Simplify(tempdic).Display + "\n";
        }
    }
}
