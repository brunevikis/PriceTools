namespace Compass.DecompToolsShellX {
    partial class FrmTendenciaHidr {
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
            this.fileVazoesDat = new Compass.DecompTools.Forms.Componentes.SelectFileTextBox();
            this.fileVazpast = new Compass.DecompTools.Forms.Componentes.SelectFileTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMes = new System.Windows.Forms.NumericUpDown();
            this.txtAno = new System.Windows.Forms.NumericUpDown();
            this.btnSalvar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtMes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAno)).BeginInit();
            this.SuspendLayout();
            // 
            // fileVazoesDat
            // 
            this.fileVazoesDat.AcceptedExtensions = null;
            this.fileVazoesDat.AllowDrop = true;
            this.fileVazoesDat.DialogTitle = "";
            this.fileVazoesDat.Location = new System.Drawing.Point(12, 47);
            this.fileVazoesDat.Name = "fileVazoesDat";
            this.fileVazoesDat.OwnerIWin32Window = null;
            this.fileVazoesDat.RootFolder = "";
            this.fileVazoesDat.Size = new System.Drawing.Size(676, 28);
            this.fileVazoesDat.TabIndex = 0;
            this.fileVazoesDat.Title = "VAZOES.DAT";
            // 
            // fileVazpast
            // 
            this.fileVazpast.AcceptedExtensions = null;
            this.fileVazpast.AllowDrop = true;
            this.fileVazpast.DialogTitle = "";
            this.fileVazpast.Location = new System.Drawing.Point(6, 80);
            this.fileVazpast.Name = "fileVazpast";
            this.fileVazpast.OwnerIWin32Window = null;
            this.fileVazpast.RootFolder = "";
            this.fileVazpast.Size = new System.Drawing.Size(682, 28);
            this.fileVazpast.TabIndex = 1;
            this.fileVazpast.Title = "VAZPAST.DAT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Data Estudo";
            // 
            // txtMes
            // 
            this.txtMes.Location = new System.Drawing.Point(94, 21);
            this.txtMes.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.txtMes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMes.Name = "txtMes";
            this.txtMes.Size = new System.Drawing.Size(64, 20);
            this.txtMes.TabIndex = 5;
            this.txtMes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtAno
            // 
            this.txtAno.Location = new System.Drawing.Point(164, 21);
            this.txtAno.Maximum = new decimal(new int[] {
            2026,
            0,
            0,
            0});
            this.txtAno.Minimum = new decimal(new int[] {
            2016,
            0,
            0,
            0});
            this.txtAno.Name = "txtAno";
            this.txtAno.Size = new System.Drawing.Size(120, 20);
            this.txtAno.TabIndex = 6;
            this.txtAno.Value = new decimal(new int[] {
            2016,
            0,
            0,
            0});
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(554, 117);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(134, 23);
            this.btnSalvar.TabIndex = 13;
            this.btnSalvar.Text = "Gravar Vazoes.Dat";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // FrmTendenciaHidr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 152);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.txtAno);
            this.Controls.Add(this.txtMes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileVazpast);
            this.Controls.Add(this.fileVazoesDat);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTendenciaHidr";
            this.ShowIcon = false;
            this.Text = "Tendencia Hidrológica";
            this.Load += new System.EventHandler(this.FrmTendenciaHidr_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtMes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAno)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DecompTools.Forms.Componentes.SelectFileTextBox fileVazoesDat;
        private DecompTools.Forms.Componentes.SelectFileTextBox fileVazpast;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtMes;
        private System.Windows.Forms.NumericUpDown txtAno;
        private System.Windows.Forms.Button btnSalvar;
    }
}