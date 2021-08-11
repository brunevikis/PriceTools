using System;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;

namespace Compass.DecompTools.Forms.Componentes
{
	public partial class SelectFileTextBox : UserControl
	{

		#region Atributos Privados

		/// <summary>
		/// Dialog de selecionar pastas
		/// </summary>
		private OpenFileDialog _objFileBrowser = new OpenFileDialog();

		/// <summary>
		/// Janela pai do dialog
		/// </summary>
		private IWin32Window _objOwner = null;

		private string m_strFile;

        private string m_strAcceptedExtensions;

		#endregion

		#region Atributos Públicos

		/// <summary>
		/// Valor do arquivo selecionado
		/// </summary>
		[Browsable(true), Description("Arquivo selecionado")]
		public new string Text
		{
            get { return m_strFile; }
			set
			{
                m_strFile = value;
                txtFile.Text = value;
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
		public String DialogTitle
		{
			get { return _objFileBrowser.Title; }
			set { _objFileBrowser.Title = value; }
		}

		/// <summary>
		/// Pasta inicial
		/// </summary>
		[Browsable(true), Description("Pasta inicial")]
		public string RootFolder
		{
			get { return _objFileBrowser.InitialDirectory; }
			set { _objFileBrowser.InitialDirectory = value; }
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

		/// <summary>
		/// Habilitar
		/// </summary>
		[Browsable(true), Description("Habilitar o componente")]
		public new bool Enabled
		{

			get { return base.Enabled; }

			set
			{
				if (value)
				{
					pnlSelectFileTextBox.BackColor = Color.White;
					txtFile.BackColor = Color.White;
					pnlSelectFileTextBox.ForeColor = Color.Black;
					txtFile.ForeColor = Color.Black;
				}
				else
				{
					pnlSelectFileTextBox.BackColor = Color.WhiteSmoke;
					txtFile.BackColor = Color.WhiteSmoke;
					pnlSelectFileTextBox.ForeColor = Color.Gray;
					txtFile.ForeColor = Color.Gray;
				}

				// Reais alterações de Enabled
				txtFile.ReadOnly = !value;
				btSelectFile.Enabled = value;
				m_blnEnabled = value;

				// Deixa o botão de abrir a pasta sempre habilitado
				// btOpenFolder.Enabled = value;

				base.Enabled = true;
			}
		}

        [Browsable(true), Description("Arquivo selecionado")]
        public String AcceptedExtensions
        {
            get { return m_strAcceptedExtensions; }
            set
            {
                m_strAcceptedExtensions = value;
            }
        }

        private bool CheckExtension(string p_strFilePath)
        {
            //if (string.Format(";{0};", m_strAcceptedExtensions).Contains(Path.GetExtension(p_strFilePath)))
            //{
                return true;
            //}
            //return false;
        }

		#endregion

		#region Construção

		/// <summary>
		/// Construtor
		/// </summary>
		public SelectFileTextBox()
		{
			InitializeComponent();
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

			if (File.Exists(this.Text))
			{
                _objFileBrowser.InitialDirectory = Path.GetDirectoryName(this.Text);
                _objFileBrowser.FileName = Path.GetFileName(this.Text);
			}

			DialogResult _objResult;

			if (this._objOwner == null)
			{
				_objResult = _objFileBrowser.ShowDialog();
			}
			else
			{
				_objResult = _objFileBrowser.ShowDialog(this._objOwner);
			}

			if (_objResult == DialogResult.OK)
			{
				this.Text = _objFileBrowser.FileName;
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
				txtFile.ForeColor = Color.Gray;
				txtFile.BackColor = Color.WhiteSmoke;
				pnlSelectFileTextBox.BackColor = Color.WhiteSmoke;
				btOpenFile.Visible = btSelectFile.Visible = true;
				btOpenFile.Enabled = true;
				btSelectFile.Enabled = false;
				SetTextBoxBold(false);
			}
			else if (m_blnDoingDrag && !m_blnValidDrag)
			{
				// está tentando fazer um drag drop inválido
				txtFile.ForeColor = Color.Black;
				txtFile.BackColor = Color.Yellow;
				pnlSelectFileTextBox.BackColor = Color.Yellow;
				btOpenFile.Visible = btSelectFile.Visible = false;
				btOpenFile.Enabled = false;
				btSelectFile.Enabled = false;
				SetTextBoxBold(true);
			}
			else if (m_blnDoingDrag && m_blnValidDrag)
			{
				// está tentando fazer um drag drop válido
				txtFile.ForeColor = Color.Black;
				txtFile.BackColor = Color.SpringGreen;
				pnlSelectFileTextBox.BackColor = Color.SpringGreen;
				btOpenFile.Visible = btSelectFile.Visible = false;
				btOpenFile.Enabled = false;
				btSelectFile.Enabled = false;
				SetTextBoxBold(true);
			}
            else if (File.Exists(m_strFile))
			{
				// sem interação, populado com um diretório válido
				txtFile.ForeColor = Color.Black;
				txtFile.BackColor = Color.White;
				pnlSelectFileTextBox.BackColor = Color.White;
				btOpenFile.Visible = btSelectFile.Visible = true;
				btOpenFile.Enabled = true;
				btSelectFile.Enabled = true;
				SetTextBoxBold(false);
			}
			else if (!File.Exists(m_strFile))
			{
				// sem interação, populado com um diretório inválido
				txtFile.ForeColor = Color.White;
				txtFile.BackColor = Color.Firebrick;
				pnlSelectFileTextBox.BackColor = Color.Firebrick;
				btOpenFile.Visible = btSelectFile.Visible = true;
				btOpenFile.Enabled = false;
				btSelectFile.Enabled = true;
				SetTextBoxBold(true);
			}

			SetTextBoxSize();

		}

		private void SetTextBoxSize()
		{
			int txtBoxWidth = pnlSelectFileTextBox.Width - 15;
			if (btSelectFile.Visible) txtBoxWidth -= btSelectFile.Width;
			if (btOpenFile.Visible) txtBoxWidth -= btOpenFile.Width;

			if (txtFile.Width == txtBoxWidth)
			{
				return;
			}

			txtFile.Width = txtBoxWidth;
		}

		private void SetTextBoxBold(bool p_blnBold)
		{
			if (txtFile.Font.Bold != p_blnBold)
			{
				FontStyle objStyle = FontStyle.Regular;
				if (p_blnBold) objStyle = FontStyle.Bold;
				txtFile.Font = new Font(txtFile.Font, objStyle);
			}
		}

		private void btOpenFile_Click(object sender, EventArgs e)
		{
			if (File.Exists(txtFile.Text))
			{
				System.Diagnostics.Process.Start("explorer.exe", txtFile.Text);
			}
		}

		private void SelectFileTextBox_Paint(object sender, PaintEventArgs e)
		{
			SetStyle();
		}

		private void SelectFileTextBox_DragEnter(object sender, DragEventArgs e)
		{

			m_blnDoingDrag = true;

			// com o componente desabilitado, não é possível fazer drag drop.
			if (!this.m_blnEnabled)
			{
				e.Effect = DragDropEffects.None;
				m_blnValidDrag = false;
			}

			string[] arrArquivosDrop = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			foreach (string strItem in arrArquivosDrop)
			{
				if (!File.Exists(strItem))
				{
					txtFile.Text = "Este campo somente aceita arquivos";
					m_blnValidDrag = false;
					SetStyle();
					return;
				}
			}

			if (arrArquivosDrop.Length != 1)
			{
				txtFile.Text = "Somente um item por vez. Você está arrastando " + arrArquivosDrop.Length.ToString() + ".";
				m_blnValidDrag = false;
				SetStyle();
				return;
			}

			string arquivo = arrArquivosDrop[0];

			if (!File.Exists(arquivo))
			{
				txtFile.Text = "O item arrastado não é um arquivo válido";
				m_blnValidDrag = false;
				SetStyle();
				return;
			}

            if (!CheckExtension(arquivo))
            {
                txtFile.Text = "Esta extensão não é permitida.";
                m_blnValidDrag = false;
                SetStyle();
                return;
            }

			txtFile.Text = "Solte para usar " + arquivo;
			m_blnValidDrag = true;

			e.Effect = DragDropEffects.Link;

			SetStyle();
		
		}

		private void SelectFileTextBox_DragLeave(object sender, EventArgs e)
		{
			m_blnDoingDrag = false;
			m_blnValidDrag = false;
			txtFile.Text = m_strFile;            
			SetStyle();
		}

		private void SelectFileTextBox_DragDrop(object sender, DragEventArgs e)
		{
			m_blnDoingDrag = false;
			m_blnValidDrag = false;

			string[] arrArquivosDrop = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			foreach (string strItem in arrArquivosDrop)
			{
				// garatia adicional de não ser arrastado uma pasta
				if (Directory.Exists(strItem)) return;
			}

			if (arrArquivosDrop.Length != 1)
			{
				// garantia adicional de que vai pegar um item só
				return;
			}

			string arquivo = arrArquivosDrop[0];

			if (!File.Exists(arquivo))
			{
				// garantia adicional de que o arquivo existe existe
				return;
			}

            m_strFile = arquivo;
			txtFile.Text = arquivo;

			SetStyle();
		}

		private bool m_blnValidDrag;
		private bool m_blnDoingDrag;

		private void txtFile_TextChanged(object sender, EventArgs e)
		{
            //TextBox txtBox = sender as TextBox;
            //if (txtBox == null) return;

            //if (this.Text != txtBox.Text)
            //{
            //    this.Text = txtBox.Text;
            //}
		}
	}
}
