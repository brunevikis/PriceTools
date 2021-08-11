using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX {
    public partial class FrmTendenciaHidr : Form {

        public string VazpastDat { get { return fileVazpast.Text; } set { fileVazpast.Text = value; } }
        public string VazoesDat { get { return fileVazoesDat.Text; } set { fileVazoesDat.Text = value; } }

        public FrmTendenciaHidr() {
            InitializeComponent();
        }

        private void FrmTendenciaHidr_Load(object sender, EventArgs e) {
            txtMes.Value = DateTime.Today.AddMonths(1).Month;
            txtAno.Value = DateTime.Today.AddMonths(1).Year;
        }

        private void btnSalvar_Click(object sender, EventArgs e) {



            Compass.CommomLibrary.VazoesC.VazoesC vazoes = null;

            if (System.IO.File.Exists(VazoesDat))
                vazoes = Compass.CommomLibrary.DocumentFactory.Create(VazoesDat) as Compass.CommomLibrary.VazoesC.VazoesC;


            Compass.CommomLibrary.Vazpast.Vazpast vazpast = null;

            if (System.IO.File.Exists(VazpastDat))
                vazpast = Compass.CommomLibrary.DocumentFactory.Create(VazpastDat) as Compass.CommomLibrary.Vazpast.Vazpast;



            if (vazpast != null && vazoes != null) {
                Compass.Services.Vazoes6.IncorporarVazpast(vazoes, vazpast, new DateTime((int)txtAno.Value, (int)txtMes.Value, 1));


                SaveFileDialog sfv = new SaveFileDialog();

                sfv.InitialDirectory = System.IO.Path.GetDirectoryName(VazoesDat);
                sfv.FileName = "vazoes.dat";

                sfv.Title = "Salvar vazoes.dat em ...";

                sfv.OverwritePrompt = true;

                if (sfv.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                    vazoes.SaveToFile(sfv.FileName);

                    MessageBox.Show("Gravado com sucesso em : \r\n" + sfv.FileName, "Decomp Crébinho Tools", MessageBoxButtons.OK, MessageBoxIcon.None);

                }

            }



        }
    }
}
