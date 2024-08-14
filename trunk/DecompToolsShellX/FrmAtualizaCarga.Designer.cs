
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
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.textBoxPlanText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_search = new System.Windows.Forms.Button();
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
            // btnAtualizar
            // 
            this.btnAtualizar.Location = new System.Drawing.Point(379, 85);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(152, 23);
            this.btnAtualizar.TabIndex = 18;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // textBoxPlanText
            // 
            this.textBoxPlanText.Location = new System.Drawing.Point(74, 46);
            this.textBoxPlanText.Name = "textBoxPlanText";
            this.textBoxPlanText.Size = new System.Drawing.Size(457, 20);
            this.textBoxPlanText.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Planilha";
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(74, 85);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(128, 23);
            this.btn_search.TabIndex = 21;
            this.btn_search.Text = "Procurar Planilha";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // FrmAtualizaCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 133);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPlanText);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.TextBoxDeckAtualiza);
            this.Name = "FrmAtualizaCarga";
            this.Text = "Atualizar Carga";
            this.Load += new System.EventHandler(this.FrmAtualizaCarga_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DecompTools.Forms.Componentes.SelectFolderTextBox TextBoxDeckAtualiza;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.TextBox textBoxPlanText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_search;
    }
}