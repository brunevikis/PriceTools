using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompToolsShellX
{
    public partial class FrmDataBaseViewer : Form
    {
        public FrmDataBaseViewer()
        {
            InitializeComponent();
        }

        public void AddInfo(string title, ResultDataSource dataSource)
        {

            this.tabControl2.Controls.Add(
                new InfoTabPage()
                {
                    Title = title,
                    DataSource = dataSource
                });
        }

        public void ClearInfo()
        {
            this.tabControl2.Controls.Clear();
        }
    }

    
}
