
namespace Compass.DecompToolsShellX
{
    partial class FrmPldAlter
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
            this.numPldMin = new System.Windows.Forms.NumericUpDown();
            this.numPldMax = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Ok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPldMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPldMax)).BeginInit();
            this.SuspendLayout();
            // 
            // numPldMin
            // 
            this.numPldMin.DecimalPlaces = 2;
            this.numPldMin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numPldMin.Location = new System.Drawing.Point(94, 47);
            this.numPldMin.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPldMin.Name = "numPldMin";
            this.numPldMin.Size = new System.Drawing.Size(120, 20);
            this.numPldMin.TabIndex = 12;
            // 
            // numPldMax
            // 
            this.numPldMax.DecimalPlaces = 2;
            this.numPldMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numPldMax.Location = new System.Drawing.Point(94, 103);
            this.numPldMax.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPldMax.Name = "numPldMax";
            this.numPldMax.Size = new System.Drawing.Size(120, 20);
            this.numPldMax.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "PLD MIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "PLD MAX";
            // 
            // btn_Ok
            // 
            this.btn_Ok.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Ok.Location = new System.Drawing.Point(31, 164);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(75, 23);
            this.btn_Ok.TabIndex = 16;
            this.btn_Ok.Text = "OK";
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // FrmPldAlter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 232);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numPldMax);
            this.Controls.Add(this.numPldMin);
            this.Name = "FrmPldAlter";
            this.Text = "PLD ALTERNATIVO";
            ((System.ComponentModel.ISupportInitialize)(this.numPldMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPldMax)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numPldMin;
        private System.Windows.Forms.NumericUpDown numPldMax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Ok;
    }
}