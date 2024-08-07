
namespace Compass.DecompToolsShellX
{
    partial class FrmAtualizaCarga
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
            this.TextBoxDeckAtualiza = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.TextBoxPlan = new Compass.DecompTools.Forms.Componentes.SelectFileTextBox();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxDeckAtualiza
            // 
            this.TextBoxDeckAtualiza.AllowDrop = true;
            this.TextBoxDeckAtualiza.Description = "";
            this.TextBoxDeckAtualiza.Location = new System.Drawing.Point(34, 12);
            this.TextBoxDeckAtualiza.Name = "TextBoxDeckAtualiza";
            this.TextBoxDeckAtualiza.OwnerIWin32Window = null;
            this.TextBoxDeckAtualiza.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.TextBoxDeckAtualiza.ShowNewFolderButton = true;
            this.TextBoxDeckAtualiza.Size = new System.Drawing.Size(497, 28);
            this.TextBoxDeckAtualiza.TabIndex = 0;
            this.TextBoxDeckAtualiza.Title = "Deck";
            // 
            // TextBoxPlan
            // 
            this.TextBoxPlan.AcceptedExtensions = null;
            this.TextBoxPlan.AllowDrop = true;
            this.TextBoxPlan.DialogTitle = "";
            this.TextBoxPlan.Location = new System.Drawing.Point(34, 46);
            this.TextBoxPlan.Name = "TextBoxPlan";
            this.TextBoxPlan.OwnerIWin32Window = null;
            this.TextBoxPlan.RootFolder = "";
            this.TextBoxPlan.Size = new System.Drawing.Size(497, 28);
            this.TextBoxPlan.TabIndex = 1;
            this.TextBoxPlan.Title = "Planilha";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.Location = new System.Drawing.Point(297, 93);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(234, 23);
            this.btnAtualizar.TabIndex = 18;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // FrmAtualizaCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 144);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.TextBoxPlan);
            this.Controls.Add(this.TextBoxDeckAtualiza);
            this.Name = "FrmAtualizaCarga";
            this.Text = "Atualizar Carga";
            this.Load += new System.EventHandler(this.FrmAtualizaCarga_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxDeckAtualiza;
        private DecompTools.Forms.Componentes.SelectFileTextBox TextBoxPlan;
        private System.Windows.Forms.Button btnAtualizar;
    }
}