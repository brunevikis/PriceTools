using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX {
    public class InfoTabPage : TabPage {

        ResultDataSource dataSource = null;
        public ResultDataSource DataSource { get { return dataSource; } set { dataSource = value; dgv.DataSource = value.DataSource; } }

        //public object DataSource { get { return dgv.DataSource; } set { dgv.DataSource = value; } }
        public string Title { get { return this.Text; } set { this.Text = value; } }


        DataGridView dgv = new DataGridView();

        public InfoTabPage()
            : base() {

            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;

            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgv.CellPainting += dgv_CellPainting;

            this.Controls.Add(dgv);
        }


        void dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.Value != null && e.Value.ToString().StartsWith("DC:")) {
                e.CellStyle.BackColor = System.Drawing.Color.LightBlue;
            } else if (e.Value != null && e.Value.ToString().StartsWith("NW:")) {
                e.CellStyle.BackColor = System.Drawing.Color.LightCoral;
            }
        }
    }


    public class ResultDataSource {

        public object DataSource { get; set; }




        public string Title { get; set; }
    }

}
