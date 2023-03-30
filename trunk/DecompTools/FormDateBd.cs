using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DecompTools
{
    public partial class FormDateBd : Form
    {
        public DateTime dataIni = new DateTime();
        public DateTime dataFim = new DateTime();
        public FormDateBd()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            dataIni = dateTimePicker1.Value;
            dataFim = dateTimePicker2.Value;

            Tuple<DateTime, DateTime> datas = new Tuple<DateTime, DateTime>(dataIni, dataFim);
            this.Close();
        }
    }
}
