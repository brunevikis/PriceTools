using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compass.DecompTools.Forms {
    public partial class FormConfig : Form {
        DataTable dt;
        private Version GetRunningVersion() {
            try {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
            } catch {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public FormConfig() {
            InitializeComponent();
        }

        private void FormConfig_Load(object sender, EventArgs e) {

            var appSettings = ConfigurationManager.AppSettings;


            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{

                new DataColumn(){ ColumnName = "Key"},
                new DataColumn(){ ColumnName = "Value"},                     

            });

            dataGridView1.DataSource = dt;


            foreach (string item in appSettings) {

                dt.Rows.Add(item, appSettings[item]);
            }

            dt.AcceptChanges();


            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(asm.Location);

            lblVersion.Text = GetRunningVersion().ToString();
        }

        private void button1_Click(object sender, EventArgs e) {
            SaveChanges();
            this.Close();
        }

        private void SaveChanges() {
            var chs = dt.GetChanges();

            if (chs == null) return;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            foreach (var ch in chs.AsEnumerable()) {
                var key = ch[0].ToString();
                var value = ch[1].ToString();

                if (config.AppSettings.Settings[key] == null) {
                    config.AppSettings.Settings.Add(key, value);
                } else {
                    config.AppSettings.Settings[key].Value = value;
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
