using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Compass.DecompToolsShellX
{
    public partial class FrmPldAlter : Form
    {
        string arqConfig = @"H:\TI - Sistemas\UAT\PricingExcelTools\files\Config_PLD_Alternativo.csv";
        public bool usar = false;
        public FrmPldAlter()
        {
            InitializeComponent();
            if (File.Exists(arqConfig))
            {
                var dados = File.ReadAllLines(arqConfig).Skip(1).First().Replace('.', ',').Split(';').ToArray();
                numPldMin.Value = Convert.ToDecimal(dados[0]);
                numPldMax.Value = Convert.ToDecimal(dados[1]);


            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show(@"Deseja usar esses valores para o PLD Alternativo?"
                  , "Limites PLD", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                double pldMin = Convert.ToDouble(numPldMin.Value);

                double pldMax = Convert.ToDouble(numPldMax.Value);

                List<string> linhas = new List<string>();
                linhas.Add("PLD Min;PLD Max");
                string lin = string.Join(";", pldMin.ToString("0.00").Replace(',', '.'), pldMax.ToString("0.00").Replace(',', '.'));
                linhas.Add(lin);

                File.WriteAllLines(arqConfig, linhas);
                usar = true;
                this.Close();
            }


        }
    }
}
