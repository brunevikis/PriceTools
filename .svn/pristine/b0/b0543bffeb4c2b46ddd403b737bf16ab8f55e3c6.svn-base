using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX
{
    public partial class FrmExtriDE : Form
    {
        public string dir;
        public FrmExtriDE(string diretorio)
        {
            InitializeComponent();
            this.textDir.Text = diretorio;
        }

       

        private void FrmExtriDE_Load(object sender, EventArgs e)
        {

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            Program.ExportExcel(this.textDir.Text, this.dateIniPicker.Value, this.dateFimPicker.Value);
            this.Close();
        }

        private void searchDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = this.textDir.Text ;
            fbd.Description = "SELECIONE O DIRETÓRIO";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textDir.Text = fbd.SelectedPath;
            }
        }
    }
}
