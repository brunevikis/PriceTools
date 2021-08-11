
namespace Compass.DecompToolsShellX
{
    partial class FrmPldDessem
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAno = new System.Windows.Forms.TextBox();
            this.textPLDMIN = new System.Windows.Forms.TextBox();
            this.textPLDMAXEST = new System.Windows.Forms.TextBox();
            this.textPLDMAX = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textDir = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ANO";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "PLDMIN";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "PLDMAX";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "PLDMAXEST";
            // 
            // txtAno
            // 
            this.txtAno.Location = new System.Drawing.Point(143, 43);
            this.txtAno.Name = "txtAno";
            this.txtAno.Size = new System.Drawing.Size(113, 20);
            this.txtAno.TabIndex = 4;
            // 
            // textPLDMIN
            // 
            this.textPLDMIN.Location = new System.Drawing.Point(143, 74);
            this.textPLDMIN.Name = "textPLDMIN";
            this.textPLDMIN.Size = new System.Drawing.Size(113, 20);
            this.textPLDMIN.TabIndex = 5;
            // 
            // textPLDMAXEST
            // 
            this.textPLDMAXEST.Location = new System.Drawing.Point(143, 141);
            this.textPLDMAXEST.Name = "textPLDMAXEST";
            this.textPLDMAXEST.Size = new System.Drawing.Size(113, 20);
            this.textPLDMAXEST.TabIndex = 6;
            // 
            // textPLDMAX
            // 
            this.textPLDMAX.Location = new System.Drawing.Point(143, 110);
            this.textPLDMAX.Name = "textPLDMAX";
            this.textPLDMAX.Size = new System.Drawing.Size(113, 20);
            this.textPLDMAX.TabIndex = 7;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(456, 178);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(370, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "DIRETÓRIO";
            // 
            // textDir
            // 
            this.textDir.Location = new System.Drawing.Point(143, 14);
            this.textDir.Name = "textDir";
            this.textDir.Size = new System.Drawing.Size(302, 20);
            this.textDir.TabIndex = 11;
            // 
            // FrmPldDessem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 228);
            this.ControlBox = false;
            this.Controls.Add(this.textDir);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.textPLDMAX);
            this.Controls.Add(this.textPLDMAXEST);
            this.Controls.Add(this.textPLDMIN);
            this.Controls.Add(this.txtAno);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrmPldDessem";
            this.Text = "PLD Dessem";
            this.Load += new System.EventHandler(this.FrmPldDessem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAno;
        private System.Windows.Forms.TextBox textPLDMIN;
        private System.Windows.Forms.TextBox textPLDMAXEST;
        private System.Windows.Forms.TextBox textPLDMAX;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textDir;
    }
}