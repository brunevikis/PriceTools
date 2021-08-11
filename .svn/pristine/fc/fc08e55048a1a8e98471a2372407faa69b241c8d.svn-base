using Compass.CommomLibrary.Dadger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX {
    public partial class FrmDcOns2Ccee : Form {

        public FrmDcOns2Ccee() {
            InitializeComponent();
        }
        private void FrmDcOns2Ccee_Load(object sender, EventArgs e) {

        }
        private void btnSalvar_Click(object sender, EventArgs e) {


            var deckONS = Compass.CommomLibrary.DeckFactory.CreateDeck(selectFolderTextBoxB.Text);
            var deckCCEE = Compass.CommomLibrary.DeckFactory.CreateDeck(selectFolderTextBoxC.Text);


            if (!(deckONS is Compass.CommomLibrary.Decomp.Deck) || !(deckCCEE is Compass.CommomLibrary.Decomp.Deck)) {
                MessageBox.Show("Os decks escolhidos não correspondem à decks DECOMP.", "ONS -> CCEE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dagerONS = ((Compass.CommomLibrary.Decomp.Deck)deckONS)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
            var dagerCCEE = ((Compass.CommomLibrary.Decomp.Deck)deckCCEE)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;
            var dadgerBase = ((Compass.CommomLibrary.Decomp.Deck)deck)[CommomLibrary.Decomp.DeckDocument.dadger].Document as Compass.CommomLibrary.Dadger.Dadger;

            //buscar restrições retiradas.
            var resONS = dagerONS.BlocoRhe.RheGrouped;
            var resCCEE = dagerCCEE.BlocoRhe.RheGrouped;

            var resDeckBase = dadgerBase.BlocoRhe.RheGrouped;

            foreach (var reONS in resONS) {

                // if (reONS.Key.Comment.ToUpperInvariant().Contains("RESTRIÇÕES DE INTERCÂMBIO CONJUNTURAIS")) conjuntural = true;

                // if (!conjuntural) {
                var fsONS = reONS.Value.Where(y => (y is FuLine) || (y is FiLine) || (y is FtLine));

                var restsCCEE = resCCEE
                    .Where(x => {
                        var fs = x.Value.Where(y => (y is FuLine) || (y is FiLine) || (y is FtLine));

                        var ok = fs.Count() == fsONS.Count();
                        if (ok) {

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

                if (restsCCEE.Count() == 0) {
                    var restsToRemove = resDeckBase
                   .Where(x => {
                       var fs = x.Value.Where(y => (y is FuLine) || (y is FiLine) || (y is FtLine));

                       var ok = fs.Count() == fsONS.Count();
                       if (ok) {

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

            foreach (var key in resDeckBase.Keys) {

                if (key.Comment.ToUpperInvariant().Contains("MBIO CONJUNTURAIS")) conjuntural = true;
                if (!conjuntural) continue;


                {
                    var fs = resDeckBase[key].Where(y => (y is Compass.CommomLibrary.Dadger.FuLine)
                        || (y is Compass.CommomLibrary.Dadger.FiLine)
                        || (y is Compass.CommomLibrary.Dadger.FtLine));

                    var ok = false;

                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FtLine && x[3] > 320); // intercambio internacional
                    ok |= fs.All(x => x is Compass.CommomLibrary.Dadger.FuLine && x[3] == 139);

                    if (!ok) {
                        resDeckBase[key].ForEach(x => x[0] = "&" + x[0]);
                    }
                }
            }


            dadgerBase.SaveToFile(createBackup: true);
            MessageBox.Show("Dadger alterados.");


            //COMENTAR DESPACHO POR RAZAO ELETRICA
            var dadgnlBase = ((Compass.CommomLibrary.Decomp.Deck)deck)[CommomLibrary.Decomp.DeckDocument.dadgnl].Document as Compass.CommomLibrary.Dadgnl.Dadgnl;

            bool eletrica = false;
            bool aviso = false;
            foreach (var gl in dadgnlBase.BlocoGL) {


                if (gl.Comment != null && gl.Comment.ToUpperInvariant().Contains("DESPACHO POR RAZ")
                    && gl.Comment.ToUpperInvariant().Contains("TRICA") && gl.Comment.ToUpperInvariant().Contains("ORDEM")) {
                    eletrica = false;
                    aviso = true;
                } else if (gl.Comment != null && gl.Comment.ToUpperInvariant().Contains("DESPACHO POR RAZ")
     && gl.Comment.ToUpperInvariant().Contains("TRICA")) eletrica = true;
                else if (gl.Comment != null) eletrica = false;

                if (eletrica) gl.GeracaoPat1 = gl.GeracaoPat2 = gl.GeracaoPat3 = 0;
            }


            dadgnlBase.SaveToFile(createBackup: true);
            if (aviso) {

                MessageBox.Show("VERIFICAR MANUALMENTE DADGNL, despacho por mais de uma razão encontrado!", "ONS -> CCEE", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            } else MessageBox.Show("Dadgnl alterados.");

            this.Close();
        }

        Compass.CommomLibrary.Decomp.Deck deck = null;
        public Compass.CommomLibrary.Decomp.Deck Deck { get { return deck; } set { deck = value; textBox1.Text = deck.BaseFolder; } }
    }
}
