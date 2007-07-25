namespace BNP2PExample
{
    partial class HelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnFontPlus = new System.Windows.Forms.Button();
            this.btnFontMinus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(514, 287);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // btnFontPlus
            // 
            this.btnFontPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontPlus.Location = new System.Drawing.Point(370, 305);
            this.btnFontPlus.Name = "btnFontPlus";
            this.btnFontPlus.Size = new System.Drawing.Size(75, 23);
            this.btnFontPlus.TabIndex = 1;
            this.btnFontPlus.Text = "Font +";
            this.btnFontPlus.UseVisualStyleBackColor = true;
            this.btnFontPlus.Click += new System.EventHandler(this.btnFontPlus_Click);
            // 
            // btnFontMinus
            // 
            this.btnFontMinus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontMinus.Location = new System.Drawing.Point(451, 305);
            this.btnFontMinus.Name = "btnFontMinus";
            this.btnFontMinus.Size = new System.Drawing.Size(75, 23);
            this.btnFontMinus.TabIndex = 2;
            this.btnFontMinus.Text = "Font -";
            this.btnFontMinus.UseVisualStyleBackColor = true;
            this.btnFontMinus.Click += new System.EventHandler(this.btnFontMinus_Click);
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 331);
            this.Controls.Add(this.btnFontMinus);
            this.Controls.Add(this.btnFontPlus);
            this.Controls.Add(this.textBox1);
            this.Name = "HelpForm";
            this.Text = "HelpForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HelpForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnFontPlus;
        private System.Windows.Forms.Button btnFontMinus;
    }
}