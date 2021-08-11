namespace Compass.DecompToolsShellX {
    partial class FrmDcOns2Ccee {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnSalvar = new System.Windows.Forms.Button();
            this.selectFolderTextBoxB = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.selectFolderTextBoxC = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalvar.Location = new System.Drawing.Point(554, 117);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(134, 23);
            this.btnSalvar.TabIndex = 13;
            this.btnSalvar.Text = "Converter";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // selectFolderTextBoxB
            // 
            this.selectFolderTextBoxB.AllowDrop = true;
            this.selectFolderTextBoxB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFolderTextBoxB.Description = "";
            this.selectFolderTextBoxB.Location = new System.Drawing.Point(96, 39);
            this.selectFolderTextBoxB.Name = "selectFolderTextBoxB";
            this.selectFolderTextBoxB.OwnerIWin32Window = null;
            this.selectFolderTextBoxB.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.selectFolderTextBoxB.ShowNewFolderButton = true;
            this.selectFolderTextBoxB.Size = new System.Drawing.Size(592, 28);
            this.selectFolderTextBoxB.TabIndex = 15;
            this.selectFolderTextBoxB.Title = "ONS BASE";
            // 
            // selectFolderTextBoxC
            // 
            this.selectFolderTextBoxC.AllowDrop = true;
            this.selectFolderTextBoxC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFolderTextBoxC.Description = "";
            this.selectFolderTextBoxC.Location = new System.Drawing.Point(91, 73);
            this.selectFolderTextBoxC.Name = "selectFolderTextBoxC";
            this.selectFolderTextBoxC.OwnerIWin32Window = null;
            this.selectFolderTextBoxC.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.selectFolderTextBoxC.ShowNewFolderButton = true;
            this.selectFolderTextBoxC.Size = new System.Drawing.Size(597, 28);
            this.selectFolderTextBoxC.TabIndex = 14;
            this.selectFolderTextBoxC.Title = "CCEE BASE";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(676, 23);
            this.textBox1.TabIndex = 16;
            // 
            // FrmDcOns2Ccee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 152);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.selectFolderTextBoxB);
            this.Controls.Add(this.selectFolderTextBoxC);
            this.Controls.Add(this.btnSalvar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDcOns2Ccee";
            this.ShowIcon = false;
            this.Text = "Decomp ONS -> CCEE";
            this.Load += new System.EventHandler(this.FrmDcOns2Ccee_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSalvar;
        private DecompTools.Forms.Componentes.SelectFolderTextBox selectFolderTextBoxC;
        private DecompTools.Forms.Componentes.SelectFolderTextBox selectFolderTextBoxB;
        private System.Windows.Forms.TextBox textBox1;
    }
}