
namespace Compass.DecompToolsShellX
{
    partial class FrmUpdateConfhd
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
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.TextBoxDC = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.TextBoxNW = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.SuspendLayout();
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.Location = new System.Drawing.Point(331, 115);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(234, 23);
            this.btnAtualizar.TabIndex = 22;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // TextBoxDC
            // 
            this.TextBoxDC.AllowDrop = true;
            this.TextBoxDC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDC.Description = "";
            this.TextBoxDC.Location = new System.Drawing.Point(29, 66);
            this.TextBoxDC.Name = "TextBoxDC";
            this.TextBoxDC.OwnerIWin32Window = null;
            this.TextBoxDC.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.TextBoxDC.ShowNewFolderButton = true;
            this.TextBoxDC.Size = new System.Drawing.Size(536, 28);
            this.TextBoxDC.TabIndex = 23;
            this.TextBoxDC.Title = "DeckDC";
            this.TextBoxDC.Load += new System.EventHandler(this.TextBoxDC_Load);
            // 
            // TextBoxNW
            // 
            this.TextBoxNW.AllowDrop = true;
            this.TextBoxNW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxNW.Description = "";
            this.TextBoxNW.Location = new System.Drawing.Point(29, 32);
            this.TextBoxNW.Name = "TextBoxNW";
            this.TextBoxNW.OwnerIWin32Window = null;
            this.TextBoxNW.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.TextBoxNW.ShowNewFolderButton = true;
            this.TextBoxNW.Size = new System.Drawing.Size(536, 28);
            this.TextBoxNW.TabIndex = 21;
            this.TextBoxNW.Title = "DeckNW";
            // 
            // FrmUpdateConfhd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 173);
            this.Controls.Add(this.TextBoxDC);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.TextBoxNW);
            this.Name = "FrmUpdateConfhd";
            this.Text = "Atualizar Confhd";
            this.Load += new System.EventHandler(this.FrmUpdateConfhd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxNW;
        private System.Windows.Forms.Button btnAtualizar;
        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxDC;
    }
}