using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompTools.Forms {
    public partial class FormCropVazoesC : Form {

        public DateTime Inicio { get { return new DateTime((int)numericUpDown2.Value, (int)numericUpDown1.Value, 1); } }

        public FormCropVazoesC() {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.None;
        }

        private void btnCarregar_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void FormRange_Load(object sender, EventArgs e) {

        }
    }
}
