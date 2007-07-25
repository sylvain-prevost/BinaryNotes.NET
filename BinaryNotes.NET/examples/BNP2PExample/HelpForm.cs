using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BNP2PExample
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            this.textBox1.Select(0, 0);
        }

        private void HelpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void btnFontPlus_Click(object sender, EventArgs e)
        {
            if (textBox1.Font.Size > 99) return;
            textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size + 1);
        }

        private void btnFontMinus_Click(object sender, EventArgs e)
        {
            if (textBox1.Font.Size < 2) return;
            textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size - 1);
        }
    }
}