namespace Cyclo2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TestEquationDisplay = new System.Windows.Forms.RichTextBox();
            this.EquationSolvingButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TestEquationDisplay
            // 
            this.TestEquationDisplay.Location = new System.Drawing.Point(294, 86);
            this.TestEquationDisplay.Name = "TestEquationDisplay";
            this.TestEquationDisplay.Size = new System.Drawing.Size(220, 259);
            this.TestEquationDisplay.TabIndex = 0;
            this.TestEquationDisplay.Text = "";
            this.TestEquationDisplay.TextChanged += new System.EventHandler(this.TestEquationDisplay_TextChanged);
            // 
            // EquationSolvingButton
            // 
            this.EquationSolvingButton.Location = new System.Drawing.Point(608, 386);
            this.EquationSolvingButton.Name = "EquationSolvingButton";
            this.EquationSolvingButton.Size = new System.Drawing.Size(75, 23);
            this.EquationSolvingButton.TabIndex = 1;
            this.EquationSolvingButton.Text = "Solve";
            this.EquationSolvingButton.UseVisualStyleBackColor = true;
            this.EquationSolvingButton.Click += new System.EventHandler(this.EquationSolvingButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EquationSolvingButton);
            this.Controls.Add(this.TestEquationDisplay);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox TestEquationDisplay;
        private System.Windows.Forms.Button EquationSolvingButton;
    }
}

