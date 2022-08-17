using Compass.CommomLibrary;
using Compass.CommomLibrary.Dadger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX
{
    public partial class FrmDcOns2Ccee : Form
    {

        public FrmDcOns2Ccee()
        {
            InitializeComponent();
        }
        
        public FrmDcOns2Ccee(CommomLibrary.Decomp.Deck deckONS) : this()
        {
            this.Deck = deckONS;
            CarregaDecksOld();            
        }

        /*public void Salvar()
        {
            Salvar(DeckFactory.CreateDeck(TextBoxCCEE.Text) as CommomLibrary.Newave.Deck, DeckFactory.CreateDeck(TextBoxONS.Text) as CommomLibrary.Newave.Deck);
        }*/

        private void CarregaDecksOld()
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var dadger = this.Deck[CommomLibrary.Decomp.DeckDocument.dadger].Document as CommomLibrary.Dadger.Dadger;

            var data = dadger.DataEstudo;

            var mesrevAnterior = CommomLibrary.MesOperativo.CreateSemanal(data.AddDays(-1).Year, data.AddDays(-1).Month, true);
            var semAnterior = mesrevAnterior.SemanasOperativas.First(x => x.Fim == data.AddDays(-1));
            var revAnterior = mesrevAnterior.SemanasOperativas.IndexOf(semAnterior);


            var mes = data.AddDays(-1).Month.ToString("00");
            var ano = data.AddDays(-1).Year.ToString("0000");
            var rev = revAnterior.ToString("0");
            var sem = (revAnterior + 1).ToString("0");
            string mesExtenso = Culture.DateTimeFormat.GetMonthName(Convert.ToInt32(data.AddDays(-1).Month)).ToLower().Substring(0, 3);
            //string mesExtenso = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(data.AddDays(-1).Month)).ToLower().Substring(0, 3);

            TextBoxONS.Text = Path.Combine("H:\\Middle - Preço\\Resultados_Modelos\\DECOMP\\ONS_DC", ano, mes + "_" + mesExtenso, "DEC_ONS_" + mes + ano + "_RV" + rev + "_VE"); // mudar o final
            TextBoxCCEE.Text = Path.Combine("H:\\Middle - Preço\\Resultados_Modelos\\DECOMP\\CCEE_DC", ano, mes + "_" + mesExtenso, "DC" + ano + mes + "-sem" + sem);//mudar o final
            
        }

        private void FrmDcOns2Ccee_Load(object sender, EventArgs e)
        {

        }


        public void Salvar()
        {
            var deckONS = DeckFactory.CreateDeck(TextBoxONS.Text);
            var deckCCEE = DeckFactory.CreateDeck(TextBoxCCEE.Text);

            if (!(deckONS is Compass.CommomLibrary.Decomp.Deck) || !(deckCCEE is Compass.CommomLibrary.Decomp.Deck))
            {
                Program.AutoClosingMessageBox.Show("Os decks escolhidos não correspondem à decks DECOMP.", "Caption", 3000);
                return;
            }

            var dagerONS = ((Compass.CommomLibrary.Decomp.Deck)deckONS)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
            var dagerCCEE = ((Compass.CommomLibrary.Decomp.Deck)deckCCEE)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
            var dadgerBase = ((Compass.CommomLibrary.Decomp.Deck)deck)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;

            //buscar restrições retiradas.
            var resONS = dagerONS.BlocoRhe.RheGrouped;
            var resCCEE = dagerCCEE.BlocoRhe.RheGrouped;

            var resDeckBase = dadgerBase.BlocoRhe.RheGrouped;

            //int num = 0;
            //var dadgnlONS = ((Compass.CommomLibrary.Decomp.Deck)deck)[CommomLibrary.Decomp.DeckDocument.dadgnl].Document as Compass.CommomLibrary.Dadgnl.Dadgnl;
            /*foreach(var dadgnl in dadgnlONS.BlocoGL)
            {
                dadgnl.NumeroUsina;
                //foreach (var geracao in dadgnl.NumeroUsina
            }*/

            foreach (var reONS in resONS)
            {

                // if (reONS.Key.Comment.ToUpperInvariant().Contains("RESTRIÇÕES DE INTERCÂMBIO CONJUNTURAIS")) conjuntural = true;

                // if (!conjuntural) {
                var fsONS = reONS.Value.Where(y => (y is FuLine) || (y is FiLine) || (y is FtLine));

                var restsCCEE = resCCEE
                    .Where(x =>
                    {
                        var fs = x.Value.Where(y => (y is FuLine) || (y is FiLine) || (y is FtLine));

                        var ok = fs.Count() == fsONS.Count();
                        if (ok)
                        {

                            ok &= fsONS.All(y => fs.Any(z => (z.GetType() == y.GetType()) && (
                                    (z is FuLine && z[3] == y[3]) ||
                                    (z is FtLine && z[3] == y[3]) ||
                                    (z is FiLine && z[3] == y[3] && z[4] == y[4])
                                )
                                )
                                );
                        }

                        return ok;
                    }).ToList();

                if (restsCCEE.Count() == 0)
                {
                    var restsToRemove = resDeckBase
                   .Where(x =>
                   {
                       var fs = x.Value.Where(y => (y is FuLine) || (y is FiLine) || (y is FtLine));

                       var ok = fs.Count() == fsONS.Count();
                       if (ok)
                       {

                           ok &= fsONS.All(y => fs.Any(z => (z.GetType() == y.GetType()) &&
                               (
                                   (z is FuLine && z[3] == y[3]) ||
                                   (z is FtLine && z[3] == y[3]) ||
                                   (z is FiLine && z[3] == y[3] && z[4] == y[4])
                               )
                               )
                               );
                       }

                       return ok;
                   }).ToList();
                    restsToRemove.ForEach(x =>
                        x.Value.ForEach(y => y[0] = "&" + y[0])
                        );
                }
            }

            bool conjuntural = false;

            foreach (var key in resDeckBase.Keys)
            {
                if(key.Comment.Contains("REPRESENTACAO DAS RESTRICOES DA UHE BELO MONTE"))
                { }

                if (key.Comment.ToUpperInvariant().Contains("MBIO CONJUNTURAIS")) conjuntural = true;
                if (!conjuntural) continue;

                {
                    var fs = resDeckBase[key].Where(y => (y is Compass.CommomLibrary.Dadger.FuLine)
                        || (y is Compass.CommomLibrary.Dadger.FiLine)
                        || (y is Compass.CommomLibrary.Dadger.FtLine));

                    var ok = false;

                    if (key.Comment.Contains("Restricao interna a UHE")) continue;

                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FtLine && x[3] > 320); // intercambio internacional
                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FuLine && x[3] == 139);

                    if (!ok)
                    {
                        resDeckBase[key].ForEach(x => x[0] = "&" + x[0]);
                    }
                }
            }


            dadgerBase.SaveToFile(createBackup: true);
            Program.AutoClosingMessageBox.Show("Dadger alterado!", "Caption", 3000);


            //COMENTAR DESPACHO POR RAZAO ELETRICA
            var dadgnlBase = ((Compass.CommomLibrary.Decomp.Deck)deck)[CommomLibrary.Decomp.DeckDocument.dadgnl].Document as Compass.CommomLibrary.Dadgnl.Dadgnl;

            bool eletrica = false;
            bool aviso = false;
            foreach (var gl in dadgnlBase.BlocoGL)
            {


                if (gl.Comment != null && gl.Comment.ToUpperInvariant().Contains("DESPACHO POR RAZ")
                    && gl.Comment.ToUpperInvariant().Contains("TRICA") && gl.Comment.ToUpperInvariant().Contains("ORDEM"))
                {
                    eletrica = false;
                    aviso = true;
                }
                else if (gl.Comment != null && gl.Comment.ToUpperInvariant().Contains("DESPACHO POR RAZ")
   && gl.Comment.ToUpperInvariant().Contains("TRICA")) eletrica = true;
                else if (gl.Comment != null) eletrica = false;

                if (eletrica) gl.GeracaoPat1 = gl.GeracaoPat2 = gl.GeracaoPat3 = 0;
            }


            dadgnlBase.SaveToFile(createBackup: true);
            if (aviso)
            {
                Program.AutoClosingMessageBox.Show("VERIFICAR MANUALMENTE DADGNL, despacho por mais de uma razão encontrado!", "Caption", 3000);
            }
            else Program.AutoClosingMessageBox.Show("DADGNL alterado!", "Caption", 3000);

            this.Close();
        }
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        Compass.CommomLibrary.Decomp.Deck deck = null;
        public Compass.CommomLibrary.Decomp.Deck Deck { get { return deck; } set { deck = value; textBoxBase.Text = deck.BaseFolder; } }

        private void TextBoxONS_Load(object sender, EventArgs e)
        {

        }
    }
}
