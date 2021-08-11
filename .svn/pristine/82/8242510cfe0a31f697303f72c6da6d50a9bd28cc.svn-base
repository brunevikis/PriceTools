using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX
{
    public partial class DecodessCortes : Form
    {
        string caminho;
        public DecodessCortes(string cam)
        {
            InitializeComponent();
            caminho = cam;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "cortesh.*|cortesh.*";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var arqName = ofd.FileName;
                if (arqName != null || arqName!="")
                {
                    File.Copy(ofd.FileName, Path.Combine(caminho, ofd.FileName.Split('\\').Last()));
                }
            }
            
            

            
        }
    }
}
