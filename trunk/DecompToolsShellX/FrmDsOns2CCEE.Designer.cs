
namespace Compass.DecompToolsShellX
{
    partial class FrmDsOns2CCEE
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
            this.TextBoxDcRef = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.TextBoxDsRef = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxDcRef
            // 
            this.TextBoxDcRef.AllowDrop = true;
            this.TextBoxDcRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDcRef.Description = "";
            this.TextBoxDcRef.Location = new System.Drawing.Point(91, 28);
            this.TextBoxDcRef.Name = "TextBoxDcRef";
            this.TextBoxDcRef.OwnerIWin32Window = null;
            this.TextBoxDcRef.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.TextBoxDcRef.ShowNewFolderButton = true;
            this.TextBoxDcRef.Size = new System.Drawing.Size(748, 28);
            this.TextBoxDcRef.TabIndex = 17;
            this.TextBoxDcRef.Title = "DECOMP REF";
            // 
            // TextBoxDsRef
            // 
            this.TextBoxDsRef.AllowDrop = true;
            this.TextBoxDsRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDsRef.Description = "";
            this.TextBoxDsRef.Location = new System.Drawing.Point(91, 62);
            this.TextBoxDsRef.Name = "TextBoxDsRef";
            this.TextBoxDsRef.OwnerIWin32Window = null;
            this.TextBoxDsRef.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.TextBoxDsRef.ShowNewFolderButton = true;
            this.TextBoxDsRef.Size = new System.Drawing.Size(748, 28);
            this.TextBoxDsRef.TabIndex = 18;
            this.TextBoxDsRef.Title = "DESSEM REF";
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalvar.Location = new System.Drawing.Point(705, 128);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(134, 23);
            this.btnSalvar.TabIndex = 19;
            this.btnSalvar.Text = "Converter";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // FrmDsOns2CCEE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 189);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.TextBoxDsRef);
            this.Controls.Add(this.TextBoxDcRef);
            this.Name = "FrmDsOns2CCEE";
            this.Text = "Dessem ONS -> CCEE";
            this.ResumeLayout(false);

        }

        #endregion
        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxDcRef;
        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxDsRef;
        private System.Windows.Forms.Button btnSalvar;
    }
}