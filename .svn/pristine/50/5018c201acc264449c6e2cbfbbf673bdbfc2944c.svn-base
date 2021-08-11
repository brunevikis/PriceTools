using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompTools.Forms
{
    internal partial class FormNewRv0 : Form
    {
        public DateTime DataEstudo
        {
            get => new DateTime((int)numericUpDown2.Value, (int)numericUpDown1.Value, 1);
            set { numericUpDown2.Value = value.Year; numericUpDown1.Value = value.Month; }
        }
        public string CaminhoSaida
        {
            get => selectFolderTextBox1.Text;
            set { selectFolderTextBox1.Text = value; }
        }

        public FormNewRv0()
        {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectFolderTextBox1.Text) || !selectFolderTextBox1.Valid)
            {
                MessageBox.Show("Caminhos inválido");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void FormNewRv0_Load(object sender, EventArgs e)
        {

        }
    }
}
