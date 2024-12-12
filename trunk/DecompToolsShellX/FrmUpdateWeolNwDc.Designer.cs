
namespace Compass.DecompToolsShellX
{
    partial class FrmUpdateWeolNwDc
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
            this.TextBoxDeck = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.TextBoxWEOL = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxDeck
            // 
            this.TextBoxDeck.AllowDrop = true;
            this.TextBoxDeck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDeck.Description = "";
            this.TextBoxDeck.Location = new System.Drawing.Point(36, 21);
            this.TextBoxDeck.Name = "TextBoxDeck";
            this.TextBoxDeck.OwnerIWin32Window = null;
            this.TextBoxDeck.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.TextBoxDeck.ShowNewFolderButton = true;
            this.TextBoxDeck.Size = new System.Drawing.Size(623, 28);
            this.TextBoxDeck.TabIndex = 22;
            this.TextBoxDeck.Title = "Deck NW DC";
            // 
            // TextBoxWEOL
            // 
            this.TextBoxWEOL.AllowDrop = true;
            this.TextBoxWEOL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxWEOL.Description = "";
            this.TextBoxWEOL.Location = new System.Drawing.Point(36, 55);
            this.TextBoxWEOL.Name = "TextBoxWEOL";
            this.TextBoxWEOL.OwnerIWin32Window = null;
            this.TextBoxWEOL.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.TextBoxWEOL.ShowNewFolderButton = true;
            this.TextBoxWEOL.Size = new System.Drawing.Size(623, 28);
            this.TextBoxWEOL.TabIndex = 24;
            this.TextBoxWEOL.Title = "Dir WEOL";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.Location = new System.Drawing.Point(425, 120);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(234, 23);
            this.btnAtualizar.TabIndex = 25;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // FrmUpdateWeolNwDc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 175);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.TextBoxWEOL);
            this.Controls.Add(this.TextBoxDeck);
            this.Name = "FrmUpdateWeolNwDc";
            this.Text = "Atualizar Weol decks NW DC";
            this.Load += new System.EventHandler(this.FrmUpdateWeolNwDc_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxDeck;
        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxWEOL;
        private System.Windows.Forms.Button btnAtualizar;
    }
}