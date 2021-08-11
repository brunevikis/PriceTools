using System;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Drawing;

namespace Compass.DecompTools.Forms.Componentes
{
    public partial class SelectFolderTextBox : UserControl
    {

        #region Atributos Privados

        /// <summary>
        /// Dialog de selecionar pastas
        /// </summary>
        private FolderBrowserDialog _objFolderBrowser = new FolderBrowserDialog();

        /// <summary>
        /// Janela pai do dialog
        /// </summary>
        private IWin32Window _objOwner = null;

        //        private string m_strPath;
        private bool m_blnValidDrag;
        private bool m_blnDoingDrag;

        #endregion

        #region Atributos Públicos

        /// <summary>
        /// Valor do diretório selecionado
        /// </summary>
        [Browsable(true), Description("Diretório selecionado")]
        public new string Text
        {
            get { return txtFolder.Text; }
            set
            {
                //m_strPath = value;
                txtFolder.Text = value;
            }
        }

        /// <summary>
        /// Rótulo descritivo
        /// </summary>
        [Browsable(true), Description("Rótulo descritivo")]
        public String Title
        {
            get { return lblRotulo.Text; }
            set
            {
                if (value == "")
                {
                    lblRotulo.Visible = false;
                }
                else
                {
                    lblRotulo.Text = value;
                    lblRotulo.Visible = true;
                    lblRotulo.AutoSize = true;
                    int intPaddingTop = (this.Height - lblRotulo.Height) / 2;
                    if (lblRotulo.Padding.Top != intPaddingTop)
                    {
                        lblRotulo.Padding = new Padding(3, intPaddingTop, 3, 0);
                    }

                }
            }
        }

        /// <summary>
        /// Descrição da caixa de diálogo
        /// </summary>
        [Browsable(true), Description("Descrição da caixa de diálogo")]
        public String Description
        {
            get { return _objFolderBrowser.Description; }
            set { _objFolderBrowser.Description = value; }
        }

        /// <summary>
        /// Pasta inicial
        /// </summary>
        [Browsable(true), Description("Pasta inicial")]
        public Environment.SpecialFolder RootFolder
        {
            get { return _objFolderBrowser.RootFolder; }
            set { _objFolderBrowser.RootFolder = value; }
        }

        /// <summary>
        /// Exibir botão de Nova Pasta
        /// </summary>
        [Browsable(true), Description("Exibir botão de Nova Pasta")]
        public bool ShowNewFolderButton
        {
            get { return _objFolderBrowser.ShowNewFolderButton; }
            set { _objFolderBrowser.ShowNewFolderButton = value; }
        }

        /// <summary>
        /// Owner da caixa de diálogo
        /// </summary>
        [Browsable(false)]
        public IWin32Window OwnerIWin32Window
        {
            get { return this._objOwner; }
            set { this._objOwner = value; }
        }

        private bool m_blnEnabled = false;
        private bool _valid = false;

        /// <summary>
        /// Exibir botão de Nova Pasta
        /// </summary>
        [Browsable(true), Description("Habilitar o componente")]
        public new bool Enabled
        {

            get { return base.Enabled; }

            set
            {
                if (value)
                {
                    pnlSelectFolderTextBox.BackColor = Color.White;
                    txtFolder.BackColor = Color.White;
                    pnlSelectFolderTextBox.ForeColor = Color.Black;
                    txtFolder.ForeColor = Color.Black;
                }
                else
                {
                    pnlSelectFolderTextBox.BackColor = Color.WhiteSmoke;
                    txtFolder.BackColor = Color.WhiteSmoke;
                    pnlSelectFolderTextBox.ForeColor = Color.Gray;
                    txtFolder.ForeColor = Color.Gray;
                }

                // Reais alterações de Enabled
                txtFolder.ReadOnly = !value;
                btSelectFolder.Enabled = value;
                m_blnEnabled = value;

                // Deixa o botão de abrir a pasta sempre habilitado
                // btOpenFolder.Enabled = value;

                base.Enabled = true;
            }
        }

        public bool Valid { get => this._valid; }

        #endregion

        #region Construção

        /// <summary>
        /// Construtor
        /// </summary>
        public SelectFolderTextBox()
        {
            InitializeComponent();

            this.Enabled = this.Enabled;
        }

        #endregion

        #region Comandos

        /// <summary>
        /// Botão selecionar pasta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">evento</param>
        private void btSelectFolder_Click(object sender, EventArgs e)
        {
            MostrarDialog();
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Mostrar caixa de seleção de diretório.
        /// </summary>
        /// <returns>Path selecionado. "" se cancelar.</returns>
        public string MostrarDialog()
        {

            string retorno = "";

            if (Directory.Exists(this.Text))
            {
                _objFolderBrowser.SelectedPath = this.Text;
            }

            DialogResult _objResult;

            if (this._objOwner == null)
            {
                _objResult = _objFolderBrowser.ShowDialog();
            }
            else
            {
                _objResult = _objFolderBrowser.ShowDialog(this._objOwner);
            }

            if (_objResult == DialogResult.OK)
            {
                this.Text = _objFolderBrowser.SelectedPath;
                retorno = this.Text;
            }

            SetStyle();
            return retorno;

        }

        #endregion

        private void txtFolder_Validated(object sender, EventArgs e)
        {
            SetStyle();
        }

        private void SetStyle()
        {
            if (!this.m_blnEnabled)
            {
                // desabilitado
                txtFolder.ForeColor = Color.Gray;
                txtFolder.BackColor = Color.WhiteSmoke;
                pnlSelectFolderTextBox.BackColor = Color.WhiteSmoke;
                btOpenFolder.Visible = btSelectFolder.Visible = true;
                btOpenFolder.Enabled = true;
                btSelectFolder.Enabled = false;
                SetTextBoxBold(false);
            }
            else if (m_blnDoingDrag && !m_blnValidDrag)
            {
                // está tentando fazer um drag drop inválido
                txtFolder.ForeColor = Color.Black;
                txtFolder.BackColor = Color.Yellow;
                pnlSelectFolderTextBox.BackColor = Color.Yellow;
                btOpenFolder.Visible = btSelectFolder.Visible = false;
                btOpenFolder.Enabled = false;
                btSelectFolder.Enabled = false;
                SetTextBoxBold(true);
            }
            else if (m_blnDoingDrag && m_blnValidDrag)
            {
                // está tentando fazer um drag drop válido
                txtFolder.ForeColor = Color.Black;
                txtFolder.BackColor = Color.SpringGreen;
                pnlSelectFolderTextBox.BackColor = Color.SpringGreen;
                btOpenFolder.Visible = btSelectFolder.Visible = false;
                btOpenFolder.Enabled = false;
                btSelectFolder.Enabled = false;
                SetTextBoxBold(true);
            }
            else if (Directory.Exists(this.Text))
            {
                // sem interação, populado com um diretório válido
                txtFolder.ForeColor = Color.Black;
                txtFolder.BackColor = Color.White;
                pnlSelectFolderTextBox.BackColor = Color.White;
                btOpenFolder.Visible = btSelectFolder.Visible = true;
                btOpenFolder.Enabled = true;
                btSelectFolder.Enabled = true;
                SetTextBoxBold(false);
                _valid = true;
            }
            else
            {
                // sem interação, populado com um diretório inválido
                txtFolder.ForeColor = Color.White;
                txtFolder.BackColor = Color.Firebrick;
                pnlSelectFolderTextBox.BackColor = Color.Firebrick;
                btOpenFolder.Visible = btSelectFolder.Visible = true;
                btOpenFolder.Enabled = false;
                btSelectFolder.Enabled = true;
                SetTextBoxBold(true);
                _valid = false;
            }

            SetTextBoxSize();

        }

        private void SetTextBoxSize()
        {
            int txtBoxWidth = pnlSelectFolderTextBox.Width - 15;
            if (btSelectFolder.Visible) txtBoxWidth -= btSelectFolder.Width;
            if (btOpenFolder.Visible) txtBoxWidth -= btOpenFolder.Width;

            if (txtFolder.Width == txtBoxWidth)
            {
                return;
            }

            txtFolder.Width = txtBoxWidth;
        }

        private void SetTextBoxBold(bool p_blnBold)
        {
            if (txtFolder.Font.Bold != p_blnBold)
            {
                FontStyle objStyle = FontStyle.Regular;
                if (p_blnBold) objStyle = FontStyle.Bold;
                txtFolder.Font = new Font(txtFolder.Font, objStyle);
            }
        }

        private void btOpenFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtFolder.Text))
            {
                if (Environment.OSVersion.ToString().Contains("Unix"))
                    System.Diagnostics.Process.Start("nautilus", txtFolder.Text);
                else
                    System.Diagnostics.Process.Start("explorer.exe", txtFolder.Text);
            }
        }

        private void SelectFolderTextBox_Paint(object sender, PaintEventArgs e)
        {
            SetStyle();
        }

        private void SelectFolderTextBox_DragEnter(object sender, DragEventArgs e)
        {

            m_blnDoingDrag = true;

            // com o componente desabilitado, não é possível fazer drag drop.
            if (!this.m_blnEnabled)
            {
                e.Effect = DragDropEffects.None;
                m_blnValidDrag = false;
            }

            string[] arrArquivosDrop = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (arrArquivosDrop.Length != 1)
            {
                txtFolder.Text = "Somente um item por vez. Você está arrastando " + arrArquivosDrop.Length.ToString();
                m_blnValidDrag = false;
                SetStyle();
                return;
            }


            string pasta = arrArquivosDrop[0];

            if (File.Exists(pasta))
            {
                pasta = System.IO.Path.GetDirectoryName(pasta);
            }

            if (!Directory.Exists(pasta))
            {
                txtFolder.Text = "O item arrastado não é uma pasta válida";
                m_blnValidDrag = false;
                SetStyle();
                return;
            }

            txtFolder.Text = "Solte para usar " + pasta;
            m_blnValidDrag = true;

            e.Effect = DragDropEffects.Link;

            SetStyle();

        }

        private void SelectFolderTextBox_DragLeave(object sender, EventArgs e)
        {
            m_blnDoingDrag = false;
            m_blnValidDrag = false;
            //txtFolder.Text = m_strPath;
            SetStyle();
        }

        private void SelectFolderTextBox_DragDrop(object sender, DragEventArgs e)
        {
            m_blnDoingDrag = false;
            m_blnValidDrag = false;

            string[] arrArquivosDrop = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //foreach (string strItem in arrArquivosDrop) {
            //    // garatia adicional de não ser arrastado um arquivo
            //    if (File.Exists(strItem)) return;
            //}

            if (arrArquivosDrop.Length != 1)
            {
                // garantia adicional de que vai pegar um item só
                return;
            }

            string pasta = arrArquivosDrop[0];

            if (File.Exists(pasta))
            {
                pasta = System.IO.Path.GetDirectoryName(pasta);
            }

            if (!Directory.Exists(pasta))
            {
                // garantia adicional de que a pasta existe
                return;
            }

            this.Text = pasta;

            SetStyle();
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {

            SetStyle();
            //TextBox txtBox = sender as TextBox;
            //if (txtBox == null) return;

            //if (this.Text != txtBox.Text)
            //{
            //    this.Text = txtBox.Text;
            //}

        }

    }
}
