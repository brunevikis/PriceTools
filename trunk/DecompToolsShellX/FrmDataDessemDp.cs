using Compass.CommomLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Compass.DecompToolsShellX
{

    public partial class FrmDataDessemDp : Form
    {
        bool atualiza = false;
        string dir = string.Empty;
        public FrmDataDessemDp(bool atualizar = false, string dirAux = "")
        {
            InitializeComponent();
            atualiza = atualizar;
            dir = dirAux;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException("Processo interrompido!");
            //throw new Exception("Processo interrompido!");
            //return;
            var p = Process.GetCurrentProcess();
            p.Kill();
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            List<Tuple<DateTime, bool, float>> dpDados = new List<Tuple<DateTime, bool, float>> {
                new Tuple<DateTime, bool, float>(this.dateDE1.Value, this.checkSab.Checked,float.Parse(this.fator1.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE2.Value, this.checkDom.Checked,float.Parse(this.fator2.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE3.Value, this.checkSeg.Checked,float.Parse(this.fator3.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE4.Value, this.checkTer.Checked,float.Parse(this.fator4.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE5.Value, this.checkQua.Checked,float.Parse(this.fator5.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE6.Value, this.checkQui.Checked,float.Parse(this.fator6.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE7.Value, this.checkSex.Checked,float.Parse(this.fator7.Value.ToString().Replace('.', ',')))
            };

            if (atualiza == true)
            {
                foreach (var dpDad in dpDados)
                {
                    Compass.CommomLibrary.IPDOEntitiesCargaDiaria CargaCtx = new IPDOEntitiesCargaDiaria();
                    var cargas = CargaCtx.Carga_Diaria.Where(x => x.Data == dpDad.Item1.Date).ToList();

                    if (cargas.Count() == 0)
                    {
                        string texto = $"Dados para a data: {dpDad.Item1.Date:dd/MM/yyyy} não encontrados, escolha uma data válida!";
                        MessageBox.Show(texto, "DESSEM-TOOLS");
                        return;
                    }
                }

                List<string> diaAtualizar = new List<string>();
                if (this.checkSab.Checked == true)
                {
                    diaAtualizar.Add("SAB");
                }
                if (this.checkDom.Checked == true)
                {
                    diaAtualizar.Add("DOM");
                }
                if (this.checkSeg.Checked == true)
                {
                    diaAtualizar.Add("SEG");
                }
                if (this.checkTer.Checked == true)
                {
                    diaAtualizar.Add("TER");
                }
                if (this.checkQua.Checked == true)
                {
                    diaAtualizar.Add("QUA");
                }
                if (this.checkQui.Checked == true)
                {
                    diaAtualizar.Add("QUI");
                }
                if (this.checkSex.Checked == true)
                {
                    diaAtualizar.Add("SEX");
                }

                Program.AtualizaDPdessem(dpDados, dir, diaAtualizar);
            }
            else
            {
                Program.CarregaCargaDiaria(dpDados);
            }
            this.Close();
            
        }

        private void FrmDataDessemDp_Load(object sender, EventArgs e)
        {
            this.fator1.Value = 1.00M;
            this.fator2.Value = 1.00M;
            this.fator3.Value = 1.00M;
            this.fator4.Value = 1.00M;
            this.fator5.Value = 1.00M;
            this.fator6.Value = 1.00M;
            this.fator7.Value = 1.00M;
        }
    }
}
