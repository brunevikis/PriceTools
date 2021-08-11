namespace Compass.DecompTools.Forms {
    partial class FormPrevsDecksSensibilidade {
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnCriar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.selectFolderTextBox2 = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.selectFolderTextBox1 = new Compass.DecompTools.Forms.Componentes.SelectFolderTextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(339, 80);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(85, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Criar Vazões";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnCriar
            // 
            this.btnCriar.Location = new System.Drawing.Point(399, 238);
            this.btnCriar.Name = "btnCriar";
            this.btnCriar.Size = new System.Drawing.Size(75, 23);
            this.btnCriar.TabIndex = 3;
            this.btnCriar.Text = "Criar";
            this.btnCriar.UseVisualStyleBackColor = true;
            this.btnCriar.Click += new System.EventHandler(this.btnCriar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(318, 238);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // selectFolderTextBox2
            // 
            this.selectFolderTextBox2.AllowDrop = true;
            this.selectFolderTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFolderTextBox2.Description = "";
            this.selectFolderTextBox2.Location = new System.Drawing.Point(12, 46);
            this.selectFolderTextBox2.Name = "selectFolderTextBox2";
            this.selectFolderTextBox2.OwnerIWin32Window = null;
            this.selectFolderTextBox2.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.selectFolderTextBox2.ShowNewFolderButton = false;
            this.selectFolderTextBox2.Size = new System.Drawing.Size(462, 28);
            this.selectFolderTextBox2.TabIndex = 1;
            this.selectFolderTextBox2.Title = "Sensibilidades";
            // 
            // selectFolderTextBox1
            // 
            this.selectFolderTextBox1.AllowDrop = true;
            this.selectFolderTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFolderTextBox1.Description = "";
            this.selectFolderTextBox1.Location = new System.Drawing.Point(12, 12);
            this.selectFolderTextBox1.Name = "selectFolderTextBox1";
            this.selectFolderTextBox1.OwnerIWin32Window = null;
            this.selectFolderTextBox1.RootFolder = System.Environment.SpecialFolder.Desktop;
            this.selectFolderTextBox1.ShowNewFolderButton = false;
            this.selectFolderTextBox1.Size = new System.Drawing.Size(462, 28);
            this.selectFolderTextBox1.TabIndex = 0;
            this.selectFolderTextBox1.Title = "Decomp Base";
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(339, 103);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(120, 17);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "Excluir Arq Previvaz";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // FormPrevsDecksSensibilidade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(486, 273);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnCriar);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.selectFolderTextBox2);
            this.Controls.Add(this.selectFolderTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPrevsDecksSensibilidade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormPrevsDecksSensibilidade";
            this.Load += new System.EventHandler(this.FormPrevsDecksSensibilidade_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Componentes.SelectFolderTextBox selectFolderTextBox1;
        private Componentes.SelectFolderTextBox selectFolderTextBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnCriar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}