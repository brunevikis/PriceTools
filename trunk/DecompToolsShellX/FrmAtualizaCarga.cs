using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Compass.CommomLibrary;
using System.IO;

namespace Compass.DecompToolsShellX
{
    public partial class FrmAtualizaCarga : Form
    {
        public CommomLibrary.Newave.Deck deckNW { get; set; }

        public FrmAtualizaCarga()
        {
            InitializeComponent();
        }

        private void FrmAtualizaCarga_Load(object sender, EventArgs e)
        {
            CarregaInputsPaths();
        }

        public void Atualizar(CommomLibrary.Newave.Deck deck, string Plan)
        {
            try
            {
                Compass.Services.Deck.AtualizaCargaMensal(deck, Plan);
                //MessageBox.Show("Atualização feita com sucesso! ", "Atenção");

                this.Close();

            }
            catch (Exception i)
            {
              MessageBox.Show(i.Message, "Atenção");
                //MessageBox.Show(i.Message);
                this.Close();
            }
        }
        public void Atualizar()
        {
            Atualizar(DeckFactory.CreateDeck(TextBoxDeckAtualiza.Text) as CommomLibrary.Newave.Deck, TextBoxPlan.Text);
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            Atualizar();
        }

        private void CarregaInputsPaths()
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");

            var data = deckNW.Dger.DataEstudo.AddMonths(1);
            // var data = DateTime.Today.AddMonths(-1);
            //var nomeMes = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(data.Month).ToLower();
            //var nomeMes = Culture.DateTimeFormat.GetMonthName(data.Month).ToLower();
            TextBoxDeckAtualiza.Text = deckNW.BaseFolder;
            string plan = "";
            int tentativas = 0;
            while (!System.IO.File.Exists(plan) && tentativas < 5)
            {
                var nomeMes = Compass.CommomLibrary.Tools.GetMonthName(data.Month);

                //TextBoxDeckAtualiza.Text = System.IO.Path.Combine(ConfigurationManager.AppSettings["nvPath"], "CCEE_NW", data.ToString("yyyy"), data.ToString("MM") + "_" + nomeMes, "NW" + data.ToString("yyyyMM"));
                plan = System.IO.Path.Combine(ConfigurationManager.AppSettings["cargaMenPlan"], data.ToString("MM_yyyy") + "_carga_mensal", "CargaMensal_PMO-" + nomeMes + data.ToString("yyyy") + ".xlsx");
                tentativas++;
                data = data.AddMonths(-1);
            }
            if (File.Exists(plan))
            {
                TextBoxPlan.Text = plan;
            }

        }
    }
}
