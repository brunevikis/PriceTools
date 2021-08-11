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
    public partial class FrmPldDessem : Form
    {
        public FrmPldDessem(string path)
        {
            InitializeComponent();
            this.textDir.Text = path;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FrmPldDessem_Load(object sender, EventArgs e)
        {
            var ano = DateTime.Today.Year;

            var pldLimitesLines = File.ReadAllLines(@"H:\TI - Sistemas\UAT\PricingExcelTools\files\PLD_SEMI_HORA.txt").Skip(1).ToList();
            foreach (var line in pldLimitesLines)
            {
                var dados = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (Convert.ToInt32(dados[0]) == ano)
                {
                    this.txtAno.Text = dados[0];
                    this.textPLDMIN.Text = dados[1];
                    this.textPLDMAX.Text = dados[2];
                    this.textPLDMAXEST.Text = dados[3];
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var ano = Convert.ToInt32(this.txtAno.Text);
            var pldMin = Convert.ToDouble(this.textPLDMIN.Text.Replace('.',','));
            var pldMax = Convert.ToDouble(this.textPLDMAX.Text.Replace('.', ','));
            var pldMaxEst = Convert.ToDouble(this.textPLDMAXEST.Text.Replace('.', ','));
            var dir = this.textDir.Text;
            Program.AtualizarCadastroPLD(dir, ano, pldMin, pldMax, pldMaxEst);
            this.Close();
        }
    }
}
