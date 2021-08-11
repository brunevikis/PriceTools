
namespace Compass.DecompToolsShellX
{
    partial class FrmResDataBase
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_CarregarPrevs = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dt_Prevs = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.Rv_Num = new System.Windows.Forms.NumericUpDown();
            this.check_Oficial = new System.Windows.Forms.CheckBox();
            this.btn_searchPrevs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textPrevs = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btn_carregaEna = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dt_Ena = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.rvNumEna = new System.Windows.Forms.NumericUpDown();
            this.check_oficialEna = new System.Windows.Forms.CheckBox();
            this.btn_searchEna = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textEna = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rv_Num)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rvNumEna)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 421);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Prevs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(786, 415);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_CarregarPrevs);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.dt_Prevs);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.Rv_Num);
            this.tabPage1.Controls.Add(this.check_Oficial);
            this.tabPage1.Controls.Add(this.btn_searchPrevs);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textPrevs);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(778, 386);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Carregar";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_CarregarPrevs
            // 
            this.btn_CarregarPrevs.Location = new System.Drawing.Point(489, 61);
            this.btn_CarregarPrevs.Name = "btn_CarregarPrevs";
            this.btn_CarregarPrevs.Size = new System.Drawing.Size(75, 31);
            this.btn_CarregarPrevs.TabIndex = 8;
            this.btn_CarregarPrevs.Text = "Carregar";
            this.btn_CarregarPrevs.UseVisualStyleBackColor = true;
            this.btn_CarregarPrevs.Click += new System.EventHandler(this.btn_CarregarPrevs_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Mês/Ano";
            // 
            // dt_Prevs
            // 
            this.dt_Prevs.CustomFormat = "MM/yyyy";
            this.dt_Prevs.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_Prevs.Location = new System.Drawing.Point(105, 62);
            this.dt_Prevs.Name = "dt_Prevs";
            this.dt_Prevs.Size = new System.Drawing.Size(81, 20);
            this.dt_Prevs.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RV";
            // 
            // Rv_Num
            // 
            this.Rv_Num.Location = new System.Drawing.Point(245, 62);
            this.Rv_Num.Name = "Rv_Num";
            this.Rv_Num.Size = new System.Drawing.Size(52, 20);
            this.Rv_Num.TabIndex = 4;
            // 
            // check_Oficial
            // 
            this.check_Oficial.AutoSize = true;
            this.check_Oficial.Location = new System.Drawing.Point(324, 62);
            this.check_Oficial.Name = "check_Oficial";
            this.check_Oficial.Size = new System.Drawing.Size(55, 17);
            this.check_Oficial.TabIndex = 3;
            this.check_Oficial.Text = "Oficial";
            this.check_Oficial.UseVisualStyleBackColor = true;
            // 
            // btn_searchPrevs
            // 
            this.btn_searchPrevs.Location = new System.Drawing.Point(489, 20);
            this.btn_searchPrevs.Name = "btn_searchPrevs";
            this.btn_searchPrevs.Size = new System.Drawing.Size(75, 20);
            this.btn_searchPrevs.TabIndex = 2;
            this.btn_searchPrevs.Text = "Procurar";
            this.btn_searchPrevs.UseVisualStyleBackColor = true;
            this.btn_searchPrevs.Click += new System.EventHandler(this.btn_searchPrevs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Prevs";
            // 
            // textPrevs
            // 
            this.textPrevs.AllowDrop = true;
            this.textPrevs.Location = new System.Drawing.Point(105, 20);
            this.textPrevs.Name = "textPrevs";
            this.textPrevs.Size = new System.Drawing.Size(368, 20);
            this.textPrevs.TabIndex = 0;
            this.textPrevs.DragDrop += new System.Windows.Forms.DragEventHandler(this.textPrevs_DragDrop);
            this.textPrevs.DragEnter += new System.Windows.Forms.DragEventHandler(this.textPrevs_DragEnter);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(778, 386);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Buscar";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tabControl3);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(792, 421);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "Enas";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(792, 421);
            this.tabControl3.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btn_carregaEna);
            this.tabPage5.Controls.Add(this.label4);
            this.tabPage5.Controls.Add(this.dt_Ena);
            this.tabPage5.Controls.Add(this.label5);
            this.tabPage5.Controls.Add(this.rvNumEna);
            this.tabPage5.Controls.Add(this.check_oficialEna);
            this.tabPage5.Controls.Add(this.btn_searchEna);
            this.tabPage5.Controls.Add(this.label6);
            this.tabPage5.Controls.Add(this.textEna);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(784, 392);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Carregar";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btn_carregaEna
            // 
            this.btn_carregaEna.Location = new System.Drawing.Point(467, 64);
            this.btn_carregaEna.Name = "btn_carregaEna";
            this.btn_carregaEna.Size = new System.Drawing.Size(75, 31);
            this.btn_carregaEna.TabIndex = 17;
            this.btn_carregaEna.Text = "Carregar";
            this.btn_carregaEna.UseVisualStyleBackColor = true;
            this.btn_carregaEna.Click += new System.EventHandler(this.btn_carregaEna_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Mês/Ano";
            // 
            // dt_Ena
            // 
            this.dt_Ena.CustomFormat = "MM/yyyy";
            this.dt_Ena.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_Ena.Location = new System.Drawing.Point(83, 65);
            this.dt_Ena.Name = "dt_Ena";
            this.dt_Ena.Size = new System.Drawing.Size(81, 20);
            this.dt_Ena.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "RV";
            // 
            // rvNumEna
            // 
            this.rvNumEna.Location = new System.Drawing.Point(223, 65);
            this.rvNumEna.Name = "rvNumEna";
            this.rvNumEna.Size = new System.Drawing.Size(52, 20);
            this.rvNumEna.TabIndex = 13;
            // 
            // check_oficialEna
            // 
            this.check_oficialEna.AutoSize = true;
            this.check_oficialEna.Location = new System.Drawing.Point(302, 65);
            this.check_oficialEna.Name = "check_oficialEna";
            this.check_oficialEna.Size = new System.Drawing.Size(55, 17);
            this.check_oficialEna.TabIndex = 12;
            this.check_oficialEna.Text = "Oficial";
            this.check_oficialEna.UseVisualStyleBackColor = true;
            // 
            // btn_searchEna
            // 
            this.btn_searchEna.Location = new System.Drawing.Point(467, 23);
            this.btn_searchEna.Name = "btn_searchEna";
            this.btn_searchEna.Size = new System.Drawing.Size(75, 20);
            this.btn_searchEna.TabIndex = 11;
            this.btn_searchEna.Text = "Procurar";
            this.btn_searchEna.UseVisualStyleBackColor = true;
            this.btn_searchEna.Click += new System.EventHandler(this.btn_searchEna_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Enas";
            // 
            // textEna
            // 
            this.textEna.AllowDrop = true;
            this.textEna.Location = new System.Drawing.Point(83, 23);
            this.textEna.Name = "textEna";
            this.textEna.Size = new System.Drawing.Size(368, 20);
            this.textEna.TabIndex = 9;
            this.textEna.DragDrop += new System.Windows.Forms.DragEventHandler(this.textEna_DragDrop);
            this.textEna.DragEnter += new System.Windows.Forms.DragEventHandler(this.textEna_DragEnter);
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(784, 392);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Buscar";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // FrmResDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmResDataBase";
            this.Text = "Resultados DataBase";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rv_Num)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rvNumEna)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btn_searchPrevs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textPrevs;
        private System.Windows.Forms.Button btn_CarregarPrevs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dt_Prevs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Rv_Num;
        private System.Windows.Forms.CheckBox check_Oficial;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btn_carregaEna;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dt_Ena;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown rvNumEna;
        private System.Windows.Forms.CheckBox check_oficialEna;
        private System.Windows.Forms.Button btn_searchEna;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textEna;
        private System.Windows.Forms.TabPage tabPage6;
    }
}