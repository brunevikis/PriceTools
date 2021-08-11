using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompTools.Forms {
    public partial class FormPrevsDecksSensibilidade : Form {


        public string DeckBase {
            get {
                return selectFolderTextBox1.Text;
            }
        }
        public string PastaSensibilidades {
            get {
                return selectFolderTextBox2.Text;
            }
        }
        public bool RodarVazoes {
            get {
                return checkBox1.Checked;
            }
        }

        public bool ExcluirArquivosPrevivaz {
            get {
                return checkBox2.Checked;
            }
        }

        public FormPrevsDecksSensibilidade() {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            selectFolderTextBox1.Enabled = true;
            selectFolderTextBox2.Enabled = true;
        }

        private void FormPrevsDecksSensibilidade_Load(object sender, EventArgs e) {

        }

        private void btnCriar_Click(object sender, EventArgs e) {

            if (
                !string.IsNullOrWhiteSpace(DeckBase) && !string.IsNullOrWhiteSpace(PastaSensibilidades) &&
                System.IO.Directory.Exists(DeckBase) && System.IO.Directory.Exists(PastaSensibilidades)) {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            } else
                MessageBox.Show("Pastas Inválidas");

        }
    }
}
