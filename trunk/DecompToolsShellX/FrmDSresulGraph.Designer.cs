
namespace Compass.DecompToolsShellX
{
    partial class FrmDSresulGraph
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend11 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.check_SE = new System.Windows.Forms.CheckBox();
            this.check_SUL = new System.Windows.Forms.CheckBox();
            this.check_NE = new System.Windows.Forms.CheckBox();
            this.check_N = new System.Windows.Forms.CheckBox();
            this.btn_ToClipBoard = new System.Windows.Forms.Button();
            this.lv_resGraph = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea11.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea11.AxisX.IsLabelAutoFit = false;
            chartArea11.AxisX.LabelStyle.Angle = -45;
            chartArea11.AxisX.MajorGrid.Interval = 0D;
            chartArea11.AxisX.MinorGrid.Enabled = true;
            chartArea11.AxisX.Title = "Estágios";
            chartArea11.AxisY.Minimum = 0D;
            chartArea11.AxisY.Title = "Valores";
            chartArea11.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea11);
            legend11.Name = "Legend1";
            this.chart1.Legends.Add(legend11);
            this.chart1.Location = new System.Drawing.Point(157, 23);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(726, 337);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 23);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // check_SE
            // 
            this.check_SE.AutoSize = true;
            this.check_SE.Location = new System.Drawing.Point(12, 63);
            this.check_SE.Name = "check_SE";
            this.check_SE.Size = new System.Drawing.Size(77, 17);
            this.check_SE.TabIndex = 3;
            this.check_SE.Text = "SUDESTE";
            this.check_SE.UseVisualStyleBackColor = true;
            // 
            // check_SUL
            // 
            this.check_SUL.AutoSize = true;
            this.check_SUL.Location = new System.Drawing.Point(12, 86);
            this.check_SUL.Name = "check_SUL";
            this.check_SUL.Size = new System.Drawing.Size(47, 17);
            this.check_SUL.TabIndex = 4;
            this.check_SUL.Text = "SUL";
            this.check_SUL.UseVisualStyleBackColor = true;
            // 
            // check_NE
            // 
            this.check_NE.AutoSize = true;
            this.check_NE.Location = new System.Drawing.Point(12, 113);
            this.check_NE.Name = "check_NE";
            this.check_NE.Size = new System.Drawing.Size(86, 17);
            this.check_NE.TabIndex = 5;
            this.check_NE.Text = "NORDESTE";
            this.check_NE.UseVisualStyleBackColor = true;
            // 
            // check_N
            // 
            this.check_N.AutoSize = true;
            this.check_N.Location = new System.Drawing.Point(12, 136);
            this.check_N.Name = "check_N";
            this.check_N.Size = new System.Drawing.Size(64, 17);
            this.check_N.TabIndex = 6;
            this.check_N.Text = "NORTE";
            this.check_N.UseVisualStyleBackColor = true;
            // 
            // btn_ToClipBoard
            // 
            this.btn_ToClipBoard.Location = new System.Drawing.Point(12, 159);
            this.btn_ToClipBoard.Name = "btn_ToClipBoard";
            this.btn_ToClipBoard.Size = new System.Drawing.Size(139, 23);
            this.btn_ToClipBoard.TabIndex = 7;
            this.btn_ToClipBoard.Text = "Copiar Para Clipboard";
            this.btn_ToClipBoard.UseVisualStyleBackColor = true;
            this.btn_ToClipBoard.Click += new System.EventHandler(this.btn_ToClipBoard_Click);
            // 
            // lv_resGraph
            // 
            this.lv_resGraph.CheckBoxes = true;
            this.lv_resGraph.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lv_resGraph.GridLines = true;
            this.lv_resGraph.HideSelection = false;
            this.lv_resGraph.Location = new System.Drawing.Point(12, 199);
            this.lv_resGraph.Name = "lv_resGraph";
            this.lv_resGraph.Size = new System.Drawing.Size(139, 161);
            this.lv_resGraph.TabIndex = 8;
            this.lv_resGraph.UseCompatibleStateImageBehavior = false;
            this.lv_resGraph.View = System.Windows.Forms.View.Details;
            this.lv_resGraph.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lv_resGraph_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Deck";
            this.columnHeader1.Width = 100;
            // 
            // FrmDSresulGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 372);
            this.Controls.Add(this.lv_resGraph);
            this.Controls.Add(this.btn_ToClipBoard);
            this.Controls.Add(this.check_N);
            this.Controls.Add(this.check_NE);
            this.Controls.Add(this.check_SUL);
            this.Controls.Add(this.check_SE);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.chart1);
            this.Name = "FrmDSresulGraph";
            this.Text = "FrmDSresulGraph";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox check_SE;
        private System.Windows.Forms.CheckBox check_SUL;
        private System.Windows.Forms.CheckBox check_NE;
        private System.Windows.Forms.CheckBox check_N;
        private System.Windows.Forms.Button btn_ToClipBoard;
        private System.Windows.Forms.ListView lv_resGraph;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}