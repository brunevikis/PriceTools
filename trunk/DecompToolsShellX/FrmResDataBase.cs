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
    }
}
