using Compass.CommomLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX
{
    public partial class FrmResDataBase : Form
    {
        public FrmResDataBase(string dir)
        {
            InitializeComponent();
        }

        private void textPrevs_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textPrevs.Text = files.First();

            }
        }

        private void textPrevs_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void btn_searchPrevs_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "prevs*|prevs*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textPrevs.Text = ofd.FileName;
            }
        }

        private void btn_CarregarPrevs_Click(object sender, EventArgs e)
        {
            if (textPrevs.Text == "" || !System.IO.File.Exists(textPrevs.Text) || !Path.GetFileName(textPrevs.Text).ToLower().Contains("prevs"))
            {
                string aviso = "Caminho inválido ou inexistente!";
                MessageBox.Show(aviso, "Atenção");
                textPrevs.Focus();
                return;
            }
            if (Path.GetFileName(textPrevs.Text).ToLower().Contains("prevs"))
            {
                string oficial = check_Oficial.Checked ? "Oficial" : "Não Oficial";
                string oficialCod = check_Oficial.Checked ? "1" : "0";



                if (System.Windows.Forms.MessageBox.Show($"Deseja carregar o Prevs como RV{Rv_Num.Value} de {dt_Prevs.Value:MM/yyyy} ?", "ATENÇÃO", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {

                    string comando = textPrevs.Text;
                    comando = comando + "|" + Rv_Num.Value.ToString() + "|" + dt_Prevs.Value.ToString("MM/yyyy") + "|" + oficialCod;

                    Thread nthread = new Thread(Program.carregaPrevs);
                    nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    nthread.Start(comando);
                    nthread.Join();

                    System.Windows.Forms.MessageBox.Show("Processo finalizado!");

                }

            }

        }

        private void btn_searchEna_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ena*|ena*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textEna.Text = ofd.FileName;
            }
        }

        private void btn_carregaEna_Click(object sender, EventArgs e)
        {
            if (textEna.Text == "" || !System.IO.File.Exists(textEna.Text) || !Path.GetFileName(textEna.Text).ToLower().Contains("ena"))
            {
                string aviso = "Caminho inválido ou inexistente!";
                MessageBox.Show(aviso, "Atenção");
                textEna.Focus();
                return;
            }
            if (Path.GetFileName(textEna.Text).ToLower().Contains("ena"))
            {
                string oficial = check_oficialEna.Checked ? "Oficial" : "Não Oficial";
                string oficialCod = check_oficialEna.Checked ? "1" : "0";



                if (System.Windows.Forms.MessageBox.Show($"Deseja carregar a Ena como RV{rvNumEna.Value} de {dt_Ena.Value:MM/yyyy} ?", "ATENÇÃO", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {

                    string comando = textEna.Text;
                    comando = comando + "|" + rvNumEna.Value.ToString() + "|" + dt_Ena.Value.ToString("MM/yyyy") + "|" + oficialCod;

                    Thread nthread = new Thread(Program.carregaEnas);
                    nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    nthread.Start(comando);
                    nthread.Join();

                    System.Windows.Forms.MessageBox.Show("Processo finalizado!");

                }

            }
        }

        private void textEna_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textEna.Text = files.First();

            }
        }

        private void textEna_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void btn_buscarPrevs_Click(object sender, EventArgs e)
        {
            Compass.CommomLibrary.Resultados_CPASEntitiesPrevs prevs_ctx = new Resultados_CPASEntitiesPrevs();
            int rvnum = Convert.ToInt32(this.Rv_numPrevs.Value);

            this.lv_resPrevs.Items.Clear();

            if (lv_resPrevs.Items.Count == 0)
            {
                List<int> dtID = new List<int>();
                List<int> rvID = new List<int>();
                List<int> resultID = new List<int>();

                if (dtp_buscaPrevs.Checked)
                {
                    DateTime dataPrevs = this.dtp_buscaPrevs.Value;
                    dtID = prevs_ctx.PrevsReg.Where(x => x.mes == dataPrevs.Month && x.ano == dataPrevs.Year).Select(x => x.id).ToList();
                }

                rvID = prevs_ctx.PrevsReg.Where(x => x.rev == rvnum).Select(x => x.id).ToList();
                if (dtID.Count() > 0 )
                {
                    if (rvID.Count() > 0 )
                    {
                        foreach (var item in dtID)
                        {
                            if (rvID.Any(x => x == item))
                            {
                                resultID.Add(item);
                            }
                        }
                    }
                    else
                    {
                        resultID = dtID;
                    }
                }
                else if (rvID.Count() > 0 )
                {
                    resultID = rvID;
                }

                if (resultID.Count() > 0)
                {
                    foreach (var num in resultID)
                    {
                        var resultPrevs = prevs_ctx.PrevsReg.Where(x => x.id == num).First();

                        string[] linha = new string[7];
                        linha[0] = resultPrevs.id.ToString();
                        linha[1] = resultPrevs.dt_entrada.ToString();
                        linha[2] = resultPrevs.rev.ToString();
                        linha[3] = resultPrevs.caminho;
                        linha[4] = resultPrevs.mes.ToString();
                        linha[5] = resultPrevs.ano.ToString();
                        linha[6] = resultPrevs.oficial.ToString();
                        ListViewItem l = new ListViewItem(linha);

                        lv_resPrevs.Items.Add(l);

                    }
                    lv_resPrevs.Show();
                }
                else
                {
                    string aviso = "Nenhum dado encontrado!!!";
                    MessageBox.Show(aviso, "Atenção");

                    return;
                }
                
            }

        }

        private void btn_MostrarPrevs_Click(object sender, EventArgs e)
        {

            if (lv_resPrevs.Items.Count == 0)
            {
                string aviso = "Lista vazia, execute uma busca.";
                MessageBox.Show(aviso, "Atenção");
                return;
            }
            else
            {
                List<int> prevsIds = new List<int>();
                foreach (ListViewItem lst in lv_resPrevs.CheckedItems)
                {
                    prevsIds.Add(Convert.ToInt32(lst.Text));
                }

                //Thread nthread = new Thread(Program.DStools_resultado);
                //nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                //nthread.Start(textResul.Text);
            }
            
        }

        private void checkAll_PrevsBusca_CheckedChanged(object sender, EventArgs e)
        {
            if (lv_resPrevs.Items.Count > 0)
            {


                if (checkAll_PrevsBusca.Checked == false)
                {
                    foreach (ListViewItem lst in lv_resPrevs.Items)
                    {
                        lst.Checked = false;
                    }
                }
                else
                {
                    foreach (ListViewItem lst in lv_resPrevs.Items)
                    {
                        lst.Checked = true;
                    }
                }
                lv_resPrevs.Show();
            }
        }
    }
}
