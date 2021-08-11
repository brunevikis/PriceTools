namespace Compass.DecompTools.Forms.Componentes
{
	partial class SelectFileTextBox
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlSelectFileTextBox = new System.Windows.Forms.Panel();
            this.btSelectFile = new System.Windows.Forms.Button();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.lblRotulo = new System.Windows.Forms.Label();
            this.toolTipControl = new System.Windows.Forms.ToolTip(this.components);
            this.pnlSelectFileTextBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSelectFileTextBox
            // 
            this.pnlSelectFileTextBox.BackColor = System.Drawing.Color.White;
            this.pnlSelectFileTextBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSelectFileTextBox.Controls.Add(this.btSelectFile);
            this.pnlSelectFileTextBox.Controls.Add(this.btOpenFile);
            this.pnlSelectFileTextBox.Controls.Add(this.txtFile);
            this.pnlSelectFileTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelectFileTextBox.Location = new System.Drawing.Point(33, 0);
            this.pnlSelectFileTextBox.Name = "pnlSelectFileTextBox";
            this.pnlSelectFileTextBox.Size = new System.Drawing.Size(285, 28);
            this.pnlSelectFileTextBox.TabIndex = 10;
            // 
            // btSelectFile
            // 
            this.btSelectFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.btSelectFile.Image = global::Compass.ExcelTools.Properties.Resources.searchfile;
            this.btSelectFile.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSelectFile.Location = new System.Drawing.Point(233, 0);
            this.btSelectFile.Name = "btSelectFile";
            this.btSelectFile.Size = new System.Drawing.Size(24, 24);
            this.btSelectFile.TabIndex = 8;
            this.toolTipControl.SetToolTip(this.btSelectFile, "Selecionar um arquivo");
            this.btSelectFile.UseVisualStyleBackColor = true;
            this.btSelectFile.Click += new System.EventHandler(this.btSelectFolder_Click);
            // 
            // btOpenFile
            // 
            this.btOpenFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.btOpenFile.Image = global::Compass.ExcelTools.Properties.Resources.go;
            this.btOpenFile.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btOpenFile.Location = new System.Drawing.Point(257, 0);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(24, 24);
            this.btOpenFile.TabIndex = 9;
            this.toolTipControl.SetToolTip(this.btOpenFile, "Executar este arquivo com a aplicação padrão");
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFile.Location = new System.Drawing.Point(5, 6);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(223, 13);
            this.txtFile.TabIndex = 7;
            this.txtFile.TextChanged += new System.EventHandler(this.txtFile_TextChanged);
            this.txtFile.Validated += new System.EventHandler(this.txtFolder_Validated);
            // 
            // lblRotulo
            // 
            this.lblRotulo.AutoSize = true;
            this.lblRotulo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblRotulo.Location = new System.Drawing.Point(0, 0);
            this.lblRotulo.Name = "lblRotulo";
            this.lblRotulo.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblRotulo.Size = new System.Drawing.Size(33, 13);
            this.lblRotulo.TabIndex = 11;
            this.lblRotulo.Text = "Title";
            this.lblRotulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolTipControl
            // 
            this.toolTipControl.ShowAlways = true;
            // 
            // SelectFileTextBox
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSelectFileTextBox);
            this.Controls.Add(this.lblRotulo);
            this.Name = "SelectFileTextBox";
            this.Size = new System.Drawing.Size(318, 28);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.SelectFileTextBox_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SelectFileTextBox_DragEnter);
            this.DragLeave += new System.EventHandler(this.SelectFileTextBox_DragLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectFileTextBox_Paint);
            this.pnlSelectFileTextBox.ResumeLayout(false);
            this.pnlSelectFileTextBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btSelectFile;
		private System.Windows.Forms.Panel pnlSelectFileTextBox;
		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.Label lblRotulo;
		private System.Windows.Forms.ToolTip toolTipControl;
		private System.Windows.Forms.Button btOpenFile;
	}
}
