using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompTools.Forms {
    public partial class FormRange : Form {

        public DateTime Inicio { get { return dateTimePicker1.Value.Date; } }
        public DateTime Fim { get { return dateTimePicker2.Value.Date; } }

        public FormRange() {
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
