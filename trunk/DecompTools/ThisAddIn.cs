using Compass.ExcelTools;

using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace Compass.DecompTools
{
    public partial class ThisAddIn {


        public string ResourcesPath { get; private set; }

        public object rlock = new object();
        public List<System.Threading.Tasks.Task> tasks = new List<System.Threading.Tasks.Task>();

        Office.CommandBarButton _saveAsButton;
        Office.CommandBarButton _selectFolderButton;
        Office.CommandBarButton _selectFileButton;

        private void ThisAddIn_Startup(object sender, System.EventArgs e) {

            LoadConfig();

            SetupCommandBars();

            SetupEventHandlers();

        }

        private void LoadConfig() {
            var app = System.Configuration.ConfigurationManager.AppSettings;

            if (app != null) {
                foreach (var k in app.AllKeys) {
                    if (k == "resourcesPath") {
                        ResourcesPath = app[k];
                    }
                }
            }
        }

        private void SetupCommandBars() {
            {
                var oleCb = Application.CommandBars["OLE Object"];
                //oleCb.Reset();
                var ctrl = oleCb.FindControl(Tag: "Save as...");
                if (ctrl != null) {
                    ctrl.Delete();
                }

                object missing = Type.Missing;
                _saveAsButton = (Office.CommandBarButton)oleCb.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                _saveAsButton.Style = Office.MsoButtonStyle.msoButtonCaption;
                _saveAsButton.Caption = "Save as...";
                _saveAsButton.Tag = "Save as...";
                _saveAsButton.Visible = true;

            }
            /////////////////////////////////////////////
            {
                var cellCb = Application.CommandBars["Cell"];
                //cellCb.Reset();
                var ctrl = cellCb.FindControl(Tag: "Select Folder");
                if (ctrl != null) {
                    ctrl.Delete();
                }

                object missing = Type.Missing;
                _selectFolderButton = (Office.CommandBarButton)cellCb.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                _selectFolderButton.Style = Office.MsoButtonStyle.msoButtonCaption;
                _selectFolderButton.Caption = "Select Folder";
                _selectFolderButton.Tag = "Select Folder";
                _selectFolderButton.Visible = true;

                _selectFileButton = (Office.CommandBarButton)cellCb.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                _selectFileButton.Style = Office.MsoButtonStyle.msoButtonCaption;
                _selectFileButton.Caption = "Select File";
                _selectFileButton.Tag = "Select File";
                _selectFileButton.Visible = true;
            }
        }

        private void SetupEventHandlers() {

            _saveAsButton.Click += _wimsBtn_Click;
            _selectFolderButton.Click += _selectFolderButton_Click;
            _selectFileButton.Click += _selectFileButton_Click;

            // Application.SheetActivate += Application_SheetActivate;

            // Application.WorkbookActivate += Application_WorkbookActivate;

            // Application.WorkbookDeactivate += Application_WorkbookDeactivate;            

            Globals.Ribbons.Ribbon1.btnPrevsCenariosProcess.Enabled = true;
            Globals.Ribbons.Ribbon1.menuReservatorio.Enabled = true;
        }

        void Application_WorkbookDeactivate(Excel.Workbook Wb) {
            Globals.Ribbons.Ribbon1.btnPrevsCenariosProcess.Enabled = false;
            Globals.Ribbons.Ribbon1.menuReservatorio.Enabled = false;
        }

        void Application_WorkbookActivate(Excel.Workbook Wb) {
            ReloadMenus(Wb);

        }

        void Application_SheetActivate(object Sh) {
            if (Sh is Excel.Worksheet) {


                var config = ((Excel.Worksheet)Sh).ToRvxPlus1Configsheet();
                if (config != null) {
                    Globals.Ribbons.Ribbon1.btnRvxSave.Enabled = true;
                } else
                    Globals.Ribbons.Ribbon1.btnRvxSave.Enabled = false;
            }
        }

        public void ReloadMenus(Excel.Workbook Wb) {

            return;

            //if (Wb.IsWorkbookPrevsCenarios()) {
            //    Globals.Ribbons.Ribbon1.btnPrevsCenariosProcess.Enabled = true;
            //} else {
            //    Globals.Ribbons.Ribbon1.btnPrevsCenariosProcess.Enabled = false;
            //}
            //var info = Globals.ThisAddIn.Application.ActiveWorkbook.GetInfosheet();
            //if (info != null && info.DocType == typeof(Compass.CommomLibrary.Dadger.Dadger).Name) {
            //    Globals.Ribbons.Ribbon1.menuReservatorio.Enabled = true;
            //} else {
            //    Globals.Ribbons.Ribbon1.menuReservatorio.Enabled = false;
            //}
        }

        void _selectFileButton_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault) {
            System.Windows.Forms.OpenFileDialog fbd = new System.Windows.Forms.OpenFileDialog();

            if (Application.Selection is Excel.Range) {

                var currentvalue = (Application.Selection as Excel.Range).Cells[1].Text;


                try {
                    var currentFile = System.IO.Path.GetFileName(currentvalue);
                    var currentDirectory = System.IO.Path.GetDirectoryName(currentvalue);

                    if (System.IO.Directory.Exists(currentDirectory)) {
                        fbd.InitialDirectory = currentDirectory;
                    }
                    fbd.FileName = currentFile;
                } catch { }

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    (Application.Selection as Excel.Range).Value = fbd.FileName;
                }
            }
        }

        void _selectFolderButton_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault) {

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            if (Application.Selection is Excel.Range) {

                var currentvalue = (Application.Selection as Excel.Range).Cells[1].Text;
                if (System.IO.Directory.Exists(currentvalue)) {
                    fbd.SelectedPath = currentvalue;
                }

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    (Application.Selection as Excel.Range).Value = fbd.SelectedPath;
                }
            }
        }

        void _wimsBtn_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault) {

            var selected = Application.Selection;

            if (selected is Microsoft.Office.Interop.Excel.OLEObject) {

                var oleSel = selected as Microsoft.Office.Interop.Excel.OLEObject;


                var content = oleSel.GetContent();

                System.Windows.Forms.SaveFileDialog s = new System.Windows.Forms.SaveFileDialog();
                s.FileName = content.Name;
                s.OverwritePrompt = true;


                if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                    System.IO.File.WriteAllBytes(s.FileName, content.Content);

                }
            } else
                System.Windows.Forms.MessageBox.Show("Invalid Selection");
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup() {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
