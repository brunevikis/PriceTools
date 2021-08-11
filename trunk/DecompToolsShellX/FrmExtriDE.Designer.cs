
namespace Compass.DecompToolsShellX
{
    partial class FrmExtriDE
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
            this.dateIniPicker = new System.Windows.Forms.DateTimePicker();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.textDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.searchDir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dateFimPicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // dateIniPicker
            // 
            this.dateIniPicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateIniPicker.Location = new System.Drawing.Point(136, 108);
            this.dateIniPicker.Name = "dateIniPicker";
            this.dateIniPicker.Size = new System.Drawing.Size(103, 20);
            this.dateIniPicker.TabIndex = 0;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(136, 170);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 1;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(249, 170);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancelar";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // textDir
            // 
            this.textDir.Location = new System.Drawing.Point(136, 41);
            this.textDir.Name = "textDir";
            this.textDir.Size = new System.Drawing.Size(302, 20);
            this.textDir.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Salvar em:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(38, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Data Inicial:";
            // 
            // searchDir
            // 
            this.searchDir.Location = new System.Drawing.Point(461, 41);
            this.searchDir.Name = "searchDir";
            this.searchDir.Size = new System.Drawing.Size(75, 23);
            this.searchDir.TabIndex = 15;
            this.searchDir.Text = "Procurar";
            this.searchDir.UseVisualStyleBackColor = true;
            this.searchDir.Click += new System.EventHandler(this.searchDir_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(261, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Data Final:";
            // 
            // dateFimPicker
            // 
            this.dateFimPicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFimPicker.Location = new System.Drawing.Point(353, 108);
            this.dateFimPicker.Name = "dateFimPicker";
            this.dateFimPicker.Size = new System.Drawing.Size(103, 20);
            this.dateFimPicker.TabIndex = 17;
            // 
            // FrmExtriDE
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 234);
            this.ControlBox = false;
            this.Controls.Add(this.dateFimPicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.searchDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textDir);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.dateIniPicker);
            this.Name = "FrmExtriDE";
            this.Text = "Extrair BlocoDE";
            this.Load += new System.EventHandler(this.FrmExtriDE_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateIniPicker;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TextBox textDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button searchDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateFimPicker;
    }
}