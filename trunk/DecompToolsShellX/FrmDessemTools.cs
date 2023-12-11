using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Compass.CommomLibrary;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Runtime.InteropServices;

namespace Compass.DecompToolsShellX
{
    public partial class FrmDessemTools : Form
    {
        public FrmDessemTools(string dir)
        {
           // System.Windows.Forms.Application.EnableVisualStyles();

            InitializeComponent();
            this.textOrigem.Text = dir;
            this.textResul.Text = dir;
            var partsDir = dir.Split('\\').Last();


            this.textSaida.Text = dir.Replace(partsDir, "");
            CarregaDirs();
        }

        public void CarregaDirs()
        {
            DateTime hoje = DateTime.Today;
            DateTime sabAnt = hoje.AddDays(-1);
            while (sabAnt.DayOfWeek != DayOfWeek.Saturday) sabAnt = sabAnt.AddDays(-1);

            dessemBaseRVXbox.Text = Tools.GetDessemRecent(hoje);
            dessemSabRVXbox.Text = Tools.GetDessemRecent(hoje, deckSabado: true);
            decompBaseRVXbox.Text = Tools.GetDecompRecentExec(hoje);

        }

        private void Search_Ori_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = this.textOrigem.Text;
            fbd.Description = "SELECIONE O DIRETÓRIO";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textOrigem.Text = fbd.SelectedPath;
            }
        }

        private void Search_Exit_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = this.textSaida.Text;
            fbd.Description = "SELECIONE O DIRETÓRIO";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textSaida.Text = fbd.SelectedPath;
            }
        }

        private void Search_Meta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = this.textMeta.Text;
            fbd.Description = "SELECIONE O DIRETÓRIO";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textMeta.Text = fbd.SelectedPath;
                var arqs = Directory.GetFiles(this.textMeta.Text).ToList();
                lv_MultiDeck.Items.Clear();

                foreach (var arq in arqs)
                {
                    string[] linha = new string[2];
                    linha[0] = Path.GetFileName(arq);
                    linha[1] = arq;
                    ListViewItem l = new ListViewItem(linha);

                    lv_MultiDeck.Items.Add(l);

                }
                lv_MultiDeck.Show();
            }

        }

        private bool verificaCaminhos()
        {
            if (textMeta.Text == "" || !Directory.Exists(textMeta.Text))
            {
                string aviso = "Campo meta inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textMeta.Focus();
                return true;
            }
            if (textOrigem.Text == "" || !Directory.Exists(textOrigem.Text))
            {
                string aviso = "Campo origem inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textOrigem.Focus();
                return true;
            }
            if (textSaida.Text == "" || !Directory.Exists(textSaida.Text))
            {
                string aviso = "Campo saida inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textSaida.Focus();
                return true;
            }
            return false;
        }


        private void StartMultiDeck_Click(object sender, EventArgs e)
        {
            if (verificaCaminhos())
            {
                return;
            }


            if (lv_MultiDeck.Items.Count == 0)
            {
                var arqs = Directory.GetFiles(this.textMeta.Text).ToList();

                foreach (var arq in arqs)
                {
                    string[] linha = new string[2];
                    linha[0] = Path.GetFileName(arq);
                    linha[1] = arq;
                    ListViewItem l = new ListViewItem(linha);

                    lv_MultiDeck.Items.Add(l);

                }
                lv_MultiDeck.Show();
                return;
            }
            List<string> arqsCopy = new List<string>();
            foreach (ListViewItem lst in lv_MultiDeck.CheckedItems)
            {
                arqsCopy.Add(lst.SubItems[1].Text);
            }

            if (arqsCopy.Count() == 0)
            {
                string aviso = "Arquivos não selecionados!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");

                return;
            }

            if (Agrupar_Check.Checked)
            {
                List<string> arqsAgr = new List<string>();
                arqsAgr.Add(textMeta.Text);
                DateTime dat = DateTime.Now;
                string folder = Path.Combine(textSaida.Text, $@"MultiDeck_{dat:HHmmss}_Agrupado");

                var deck = DeckFactory.CreateDeck(textOrigem.Text);

                deck.CopyFilesToFolder(folder);


                foreach (var arq in arqsCopy)
                {
                    File.Copy(arq, Path.Combine(folder, arq.Split('\\').Last()), true);
                    arqsAgr.Add(arq.Split('\\').Last());
                }
                File.WriteAllLines(Path.Combine(folder, "Arqs.log"), arqsAgr);
            }
            else if (Acu_Check.Checked)
            {
                int qtdeArqs = arqsCopy.Count();
                DateTime dat = DateTime.Now;

                for (int it = 1; it <= qtdeArqs; it++)
                {
                    List<string> arqsAcu = new List<string>();
                    arqsAcu.Add(textMeta.Text);
                    string folder = Path.Combine(textSaida.Text, $@"MultiDeck_{dat:HHmmss}_Acumulado{it}");
                    var deck = DeckFactory.CreateDeck(textOrigem.Text);

                    deck.CopyFilesToFolder(folder);


                    for (int i = 0; i <= it - 1; i++)
                    {
                        File.Copy(arqsCopy[i], Path.Combine(folder, arqsCopy[i].Split('\\').Last()), true);
                        arqsAcu.Add(arqsCopy[i].Split('\\').Last());
                    }

                    File.WriteAllLines(Path.Combine(folder, "Arqs.log"), arqsAcu);
                }

            }
            else
            {
                DateTime dat = DateTime.Now;
                foreach (var arq in arqsCopy)
                {
                    List<string> arqsMulti = new List<string>();
                    arqsMulti.Add(textMeta.Text);

                    string folder = Path.Combine(textSaida.Text, $@"MultiDeck_{dat:HHmmss}_{arq.Split('\\').Last().Split('.').First()}");

                    var deck = DeckFactory.CreateDeck(textOrigem.Text);

                    deck.CopyFilesToFolder(folder);

                    File.Copy(arq, Path.Combine(folder, arq.Split('\\').Last()), true);
                    arqsMulti.Add(arq.Split('\\').Last());
                    File.WriteAllLines(Path.Combine(folder, "Arqs.log"), arqsMulti);

                }
            }

            string texto = "Processo Realizado com sucesso!";
            MessageBox.Show(texto, "DESSEM-TOOLS");
        }

        private void btn_carregar_Click(object sender, EventArgs e)
        {


            if (verificaCaminhos())
            {
                return;
            }

            lv_MultiDeck.Items.Clear();
            if (lv_MultiDeck.Items.Count == 0)
            {
                var arqs = Directory.GetFiles(this.textMeta.Text).ToList();

                foreach (var arq in arqs)
                {
                    string[] linha = new string[2];
                    linha[0] = Path.GetFileName(arq);
                    linha[1] = arq;
                    ListViewItem l = new ListViewItem(linha);

                    lv_MultiDeck.Items.Add(l);

                }
                lv_MultiDeck.Show();

            }
        }

        private DateTime GetDeckDate(string dir)
        {
            var dadvaz = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("dadvaz")).First();

            var dadlinhas = File.ReadAllLines(dadvaz).ToList();
            var dados = dadlinhas[9].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            DateTime dataEstudo = new DateTime(Convert.ToInt32(dados[3]), Convert.ToInt32(dados[2]), Convert.ToInt32(dados[1]));
            return dataEstudo;
        }

        private Compass.CommomLibrary.EntdadosDat.EntdadosDat GetEntdados(string dir)
        {
            var entdadosFile = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).ToLower().Contains("entdados")).First();
            var entdados = DocumentFactory.Create(entdadosFile) as Compass.CommomLibrary.EntdadosDat.EntdadosDat;

            return entdados;
        }

        private void btn_carregaBlocks_Click(object sender, EventArgs e)
        {

            lv_blocks.Items.Clear();
            if (verificaCaminhos())
            {
                return;
            }


            string dirOri = textOrigem.Text;
            string dirMeta = textMeta.Text;

            DateTime dtDeckOri = GetDeckDate(dirOri);
            DateTime dtDeckMeta = GetDeckDate(dirMeta);

            if (dtDeckOri == dtDeckMeta)
            {
                var entdadosMeta = GetEntdados(dirMeta);

                foreach (var chave in entdadosMeta.Blocos.Keys.ToList())
                {
                    if (entdadosMeta.Blocos[chave].Count() > 0)
                    {
                        string[] linha = new string[2];
                        linha[0] = entdadosMeta.Blocos[chave].GetType().Name.ToString();
                        linha[1] = chave;
                        ListViewItem l = new ListViewItem(linha);
                        lv_blocks.Items.Add(l);
                    }
                }
                lv_blocks.Show();
            }
            else
            {
                string aviso = "Decks com datas incompativeis!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                return;
            }


        }

        private void iniciar_blocks_Click(object sender, EventArgs e)
        {
            if (lv_blocks.Items.Count == 0 || lv_blocks.CheckedItems.Count == 0)
            {
                string aviso = "Por favor carregar e selecionar lista de blocos!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                return;
            }

            List<string> listaBlocos = new List<string>();

            foreach (ListViewItem lst in lv_blocks.CheckedItems)
            {
                listaBlocos.Add(lst.SubItems[1].Text);
            }

            if (AgrBlocks.Checked)
            {
                CriarMultiBlock(listaBlocos, true);
            }
            else
            {
                CriarMultiBlock(listaBlocos, false);

            }

        }

        private void CriarMultiBlock(List<string> blocosCod, bool agrupar)
        {
            var entdadosMeta = GetEntdados(textMeta.Text);
            DateTime dat = DateTime.Now;
            if (agrupar)
            {
                string folder = Path.Combine(textSaida.Text, $@"MultiBlock_{dat:HHmmss}_Agrupado");

                var deck = DeckFactory.CreateDeck(textOrigem.Text);

                deck.CopyFilesToFolder(folder);

                var newEntdados = GetEntdados(folder);


                foreach (var cod in blocosCod)
                {
                    newEntdados.Blocos[cod] = entdadosMeta.Blocos[cod];
                }

                newEntdados.SaveToFile();
                blocosCod.Insert(0, textMeta.Text);
                File.WriteAllLines(Path.Combine(folder, "Blocos.log"), blocosCod);

            }
            else
            {
                foreach (var cod in blocosCod)
                {
                    string folder = Path.Combine(textSaida.Text, $@"MultiBlock_{dat:HHmmss}_{entdadosMeta.Blocos[cod].GetType().Name}");

                    var deck = DeckFactory.CreateDeck(textOrigem.Text);

                    deck.CopyFilesToFolder(folder);

                    var newEntdados = GetEntdados(folder);
                    newEntdados.Blocos[cod] = entdadosMeta.Blocos[cod];

                    newEntdados.SaveToFile();
                    List<string> log = new List<string> { textMeta.Text, cod };
                    File.WriteAllLines(Path.Combine(folder, "Blocos.log"), log);

                }
            }

            string texto = "Processo Realizado com sucesso!";
            MessageBox.Show(texto, "DESSEM-TOOLS");
        }

        private void checkAll_multiDeck_CheckedChanged(object sender, EventArgs e)
        {
            if (lv_MultiDeck.Items.Count > 0)
            {


                if (checkAll_multiDeck.Checked == false)
                {
                    foreach (ListViewItem lst in lv_MultiDeck.Items)
                    {
                        lst.Checked = false;
                    }
                }
                else
                {
                    foreach (ListViewItem lst in lv_MultiDeck.Items)
                    {
                        lst.Checked = true;
                    }
                }
                lv_MultiDeck.Show();
            }

        }

        private void checkAll_MultiBlock_CheckedChanged(object sender, EventArgs e)
        {
            if (lv_blocks.Items.Count > 0)
            {


                if (checkAll_MultiBlock.Checked == false)
                {
                    foreach (ListViewItem lst in lv_blocks.Items)
                    {
                        lst.Checked = false;
                    }
                }
                else
                {
                    foreach (ListViewItem lst in lv_blocks.Items)
                    {
                        lst.Checked = true;
                    }
                }
                lv_blocks.Show();
            }
        }





        private void btn_IniciaCarga_Click(object sender, EventArgs e)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            if (textOrigem.Text == "" || !Directory.Exists(textOrigem.Text))
            {
                string aviso = "Campo origem inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textOrigem.Focus();
                return;
            }

            List<Tuple<DateTime, bool, float>> dpDados = new List<Tuple<DateTime, bool, float>> {
                new Tuple<DateTime, bool, float>(this.dateDE1.Value, this.checkSab.Checked,float.Parse(this.fator1.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE2.Value, this.checkDom.Checked,float.Parse(this.fator2.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE3.Value, this.checkSeg.Checked,float.Parse(this.fator3.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE4.Value, this.checkTer.Checked,float.Parse(this.fator4.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE5.Value, this.checkQua.Checked,float.Parse(this.fator5.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE6.Value, this.checkQui.Checked,float.Parse(this.fator6.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE7.Value, this.checkSex.Checked,float.Parse(this.fator7.Value.ToString().Replace('.', ',')))
            };


            List<string> diaAtualizar = new List<string>();
            if (this.checkSab.Checked == true)
            {
                diaAtualizar.Add("SAB");
            }
            if (this.checkDom.Checked == true)
            {
                diaAtualizar.Add("DOM");
            }
            if (this.checkSeg.Checked == true)
            {
                diaAtualizar.Add("SEG");
            }
            if (this.checkTer.Checked == true)
            {
                diaAtualizar.Add("TER");
            }
            if (this.checkQua.Checked == true)
            {
                diaAtualizar.Add("QUA");
            }
            if (this.checkQui.Checked == true)
            {
                diaAtualizar.Add("QUI");
            }
            if (this.checkSex.Checked == true)
            {
                diaAtualizar.Add("SEX");
            }

            if (check_Banco.Checked)
            {
                try
                {
                    ColetaDpBanco(dpDados, textOrigem.Text, diaAtualizar);
                    TrataDpDessemTools(textOrigem.Text);

                }
                catch (Exception ex)
                {
                    return;
                }

            }
            else
            {
                AjustaCarga(dpDados, textOrigem.Text, diaAtualizar);
            }

            string texto = "Processo Realizado com sucesso!";
            MessageBox.Show(texto, "DESSEM-TOOLS");
        }

        private void AjustaCarga(List<Tuple<DateTime, bool, float>> dpDados, string dir, List<string> diaAtualizar)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            DateTime dtDeck = GetDeckDate(dir);
            var entdados = GetEntdados(dir);

            var datarev = dtDeck;
            if (dtDeck.DayOfWeek == DayOfWeek.Friday)
            {
                datarev = datarev.AddDays(-1);
            }

            var revisao = Tools.GetCurrRev(datarev);

            for (DateTime d = dtDeck; d <= revisao.revDate; d = d.AddDays(1))
            {
                string diaAbrev = "";
                float fator = 0f;
                switch (d.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        diaAbrev = "SAB";
                        fator = float.Parse(this.fator1.Value.ToString().Replace('.', ','));
                        break;

                    case DayOfWeek.Sunday:
                        diaAbrev = "DOM";
                        fator = float.Parse(this.fator2.Value.ToString().Replace('.', ','));
                        break;

                    case DayOfWeek.Monday:
                        diaAbrev = "SEG";
                        fator = float.Parse(this.fator3.Value.ToString().Replace('.', ','));
                        break;

                    case DayOfWeek.Tuesday:
                        diaAbrev = "TER";
                        fator = float.Parse(this.fator4.Value.ToString().Replace('.', ','));
                        break;

                    case DayOfWeek.Wednesday:
                        diaAbrev = "QUA";
                        fator = float.Parse(this.fator5.Value.ToString().Replace('.', ','));
                        break;

                    case DayOfWeek.Thursday:
                        diaAbrev = "QUI";
                        fator = float.Parse(this.fator6.Value.ToString().Replace('.', ','));
                        break;

                    case DayOfWeek.Friday:
                        diaAbrev = "SEX";
                        fator = float.Parse(this.fator7.Value.ToString().Replace('.', ','));
                        break;

                    default:
                        diaAbrev = "";
                        break;

                }
                if (diaAtualizar.Any(x => x.Equals(diaAbrev)))
                {
                    foreach (var dpline in entdados.BlocoDp.Where(x => Convert.ToInt32(x.DiaInic) == d.Day && x.Subsist != 11).ToList())
                    {
                        dpline.Demanda = dpline.Demanda * fator;
                    }
                }
            }
            entdados.SaveToFile(createBackup: true);

        }
        private void TrataDpDessemTools(string dir)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            var entdados = GetEntdados(dir);

            DateTime dataDeck = GetDeckDate(dir);
            var diasAtualizar = File.ReadAllLines(Path.Combine(dir, "diaAtualizar.txt")).ToList();

            var datarev = dataDeck;
            if (dataDeck.DayOfWeek == DayOfWeek.Friday)
            {
                datarev = datarev.AddDays(-1);
            }

            var revisao = Tools.GetCurrRev(datarev);

            for (DateTime dt = dataDeck; dt <= revisao.revDate; dt = dt.AddDays(1))
            {
                string diaAbrev = "";
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        diaAbrev = "SAB";
                        break;
                    case DayOfWeek.Sunday:
                        diaAbrev = "DOM";
                        break;
                    case DayOfWeek.Monday:
                        diaAbrev = "SEG";
                        break;
                    case DayOfWeek.Tuesday:
                        diaAbrev = "TER";
                        break;
                    case DayOfWeek.Wednesday:
                        diaAbrev = "QUA";
                        break;
                    case DayOfWeek.Thursday:
                        diaAbrev = "QUI";
                        break;
                    case DayOfWeek.Friday:
                        diaAbrev = "SEX";
                        break;
                    default:
                        diaAbrev = "";
                        break;

                }
                if (diasAtualizar.Any(x => x.Equals(diaAbrev)))
                {
                    var inicioRev = revisao.revDate.AddDays(-6);
                    int index = 0;
                    for (DateTime d = inicioRev; d <= revisao.revDate; d = d.AddDays(1))
                    {
                        if (d <= dt)
                        {
                            index++;
                        }
                    }
                    var dpFileCSV = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).Contains($"blocoDPcarga{index}.csv")).First();

                    var dplines = File.ReadAllLines(dpFileCSV).ToList();

                    List<Tuple<int, int, float>> dadosCarga = new List<Tuple<int, int, float>>();

                    foreach (var dpl in dplines)
                    {
                        var dados = dpl.Split(';').ToList();

                        Tuple<int, int, float> dad = new Tuple<int, int, float>(Convert.ToInt32(dados[0]), Convert.ToInt32(dados[1]), float.Parse(dados[2]));
                        dadosCarga.Add(dad);//submercad,hora,valor
                    }


                    for (int s = 1; s <= 4; s++)//submercado
                    {


                        if (dt == dataDeck)
                        {
                            for (int i = 1; i <= 24; i++)
                            {
                                float valor = dadosCarga.Where(x => x.Item1 == s && x.Item2 == i).Select(x => x.Item3).First();
                                var newDPs = entdados.BlocoDp.Where(x => x.Subsist == s && Convert.ToInt32(x.DiaInic.Trim()) == dt.Day && x.HoraInic == (i - 1)).ToList();
                                foreach (var dp in newDPs)
                                {
                                    dp.Demanda = valor;
                                }

                            }
                        }
                        else//pega os dados do csv dos dias seguintes para o calculo da media por horas agrupadas
                        {
                            index = 0;
                            for (DateTime newd = inicioRev; newd <= dt; newd = newd.AddDays(1))
                            {
                                if (newd <= dt)
                                {
                                    index++;
                                }
                            }
                            var NewdpFileCSV = Directory.GetFiles(dir).Where(x => Path.GetFileName(x).Contains($"blocoDPcarga{index}.csv")).First();
                            //var dplines = File.ReadAllLines(dpFile, Encoding.UTF8);
                            var Newdplines = File.ReadAllLines(NewdpFileCSV).ToList();

                            List<Tuple<int, int, float>> NewdadosCarga = new List<Tuple<int, int, float>>();

                            foreach (var Ndpl in Newdplines)
                            {
                                var Ndados = Ndpl.Split(';').ToList();

                                Tuple<int, int, float> Ndad = new Tuple<int, int, float>(Convert.ToInt32(Ndados[0]), Convert.ToInt32(Ndados[1]), float.Parse(Ndados[2]));
                                NewdadosCarga.Add(Ndad);//submercad,hora,valor
                            }
                            bool pat2023 = dt.Year >= 2023;
                            var intervalosAgruped = Tools.GetIntervalosPatamares(dt, pat2023);

                            foreach (var inter in intervalosAgruped)
                            {
                                var listaValores = NewdadosCarga.Where(x => x.Item1 == s && x.Item2 >= inter.Item1 && x.Item2 <= inter.Item2).Select(x => x.Item3).ToList();

                                float valorMedia = listaValores.Average();
                                var newDpSeguinte = entdados.BlocoDp.Where(x => x.Subsist == s && Convert.ToInt32(x.DiaInic.Trim()) == dt.Day && x.HoraInic == (inter.Item1 - 1)).ToList();

                                foreach (var dpseg in newDpSeguinte)
                                {
                                    dpseg.Demanda = valorMedia;
                                }

                            }


                        }

                    }



                }
            }
            entdados.SaveToFile(createBackup: true);

        }

        private void ColetaDpBanco(List<Tuple<DateTime, bool, float>> dpDados, string dir, List<string> diaAtualizar)
        {
            int index = 0;

            foreach (var dpDad in dpDados)
            {

                string arqBlocoDP = Path.Combine(dir, $"blocoDPcarga{index + 1}.csv");
                List<string> linhas = new List<string>();

                List<Tuple<DateTime, int, int, decimal?>> dadosCarga = new List<Tuple<DateTime, int, int, decimal?>>();
                Compass.CommomLibrary.IPDOEntitiesCargaDiaria CargaCtx = new IPDOEntitiesCargaDiaria();
                var cargas = CargaCtx.Carga_Diaria.Where(x => x.Data == dpDad.Item1.Date).ToList();

                if (cargas.Count() == 0 && dpDad.Item2 == true)
                {
                    string texto = $"Dados para a data: {dpDad.Item1.Date:dd/MM/yyyy} não encontrados, escolha uma data válida!";
                    MessageBox.Show(texto, "DESSEM-TOOLS");
                    throw new NotImplementedException();
                }

                foreach (var cg in cargas)
                {
                    Tuple<DateTime, int, int, decimal?> cgDados = new Tuple<DateTime, int, int, decimal?>(cg.Data, cg.Hora, cg.Submercado, cg.Previsto);
                    dadosCarga.Add(cgDados);
                }

                for (int s = 1; s <= 4; s++)
                {
                    foreach (var car in dadosCarga.Where(x => x.Item3 == s).ToList())
                    {
                        float valor = (float)car.Item4 * dpDad.Item3;
                        string linha = $"{car.Item3};{car.Item2};{valor};";//submercado,hora,valor
                        linhas.Add(linha);
                    }
                }

                File.WriteAllLines(arqBlocoDP, linhas);

                index++;
            }
            File.WriteAllLines(Path.Combine(dir, "diaAtualizar.txt"), diaAtualizar);
        }

        private void FrmDessemTools_Load(object sender, EventArgs e)
        {
            this.fator1.Value = 1.00M;
            this.fator2.Value = 1.00M;
            this.fator3.Value = 1.00M;
            this.fator4.Value = 1.00M;
            this.fator5.Value = 1.00M;
            this.fator6.Value = 1.00M;
            this.fator7.Value = 1.00M;
        }





        private void btn_geraGraph_Click(object sender, EventArgs e)
        {
            var Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
            if (textOrigem.Text == "" || !Directory.Exists(textOrigem.Text))
            {
                string aviso = "Campo origem inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textOrigem.Focus();
                return;
            }

            List<Tuple<DateTime, bool, float>> dpDados = new List<Tuple<DateTime, bool, float>> {
                new Tuple<DateTime, bool, float>(this.dateDE1.Value, this.checkSab.Checked,float.Parse(this.fator1.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE2.Value, this.checkDom.Checked,float.Parse(this.fator2.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE3.Value, this.checkSeg.Checked,float.Parse(this.fator3.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE4.Value, this.checkTer.Checked,float.Parse(this.fator4.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE5.Value, this.checkQua.Checked,float.Parse(this.fator5.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE6.Value, this.checkQui.Checked,float.Parse(this.fator6.Value.ToString().Replace('.', ','))),
                new Tuple<DateTime, bool, float>(this.dateDE7.Value, this.checkSex.Checked,float.Parse(this.fator7.Value.ToString().Replace('.', ',')))
            };

            foreach (var dp in dpDados)
            {
                Compass.CommomLibrary.IPDOEntitiesCargaDiaria CargaCtx = new IPDOEntitiesCargaDiaria();
                var cargas = CargaCtx.Carga_Diaria.Where(x => x.Data == dp.Item1.Date).ToList();

                if (cargas.Count() > 0 && dp.Item2 == true)
                {

                    string comando = $"{textOrigem.Text};{dp.Item1.Date};true;{dp.Item3}";


                    Thread thread = new Thread(Program.graphDp);
                    thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                    thread.Start(comando);
                    //thread.Join(); //Wait for the thread to end    
                    // var frm = new FrmExtriDE(dir);
                    //frm.ShowDialog();
                }


            }
            var data = GetDeckDate(textOrigem.Text);

            string comandoEnt = $"{textOrigem.Text};{data.Date}";
            Thread nthread = new Thread(Program.graphDp);
            nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            nthread.Start(comandoEnt);
            //nthread.Join(); 


        }

        private void Acu_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (Acu_Check.Checked == true)
            {
                Agrupar_Check.Checked = false;
            }
        }

        private void Agrupar_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (Agrupar_Check.Checked == true)
            {
                Acu_Check.Checked = false;
            }
        }

        private void textOrigem_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.Link;
            }
        }
        private void textMeta_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.Link;
            }
        }
        private void textSaida_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.Link;
            }
        }



        private void textOrigem_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textOrigem.Text = files.First();

                //foreach (var file in files)
                //{
                //    if (System.IO.Directory.Exists(file))
                //    {
                //        textBox1.Text += file + "|";
                //    }
                //    else
                //    {
                //        textBox1.Text += System.IO.Path.GetDirectoryName(file) + "|";
                //    }
                //}


            }
        }

        private void textMeta_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textMeta.Text = files.First();

                //foreach (var file in files)
                //{
                //    if (System.IO.Directory.Exists(file))
                //    {
                //        textBox1.Text += file + "|";
                //    }
                //    else
                //    {
                //        textBox1.Text += System.IO.Path.GetDirectoryName(file) + "|";
                //    }
                //}


            }
        }
        private void textSaida_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textSaida.Text = files.First();

                //foreach (var file in files)
                //{
                //    if (System.IO.Directory.Exists(file))
                //    {
                //        textBox1.Text += file + "|";
                //    }
                //    else
                //    {
                //        textBox1.Text += System.IO.Path.GetDirectoryName(file) + "|";
                //    }
                //}


            }
        }



        private void btn_Up_Click(object sender, EventArgs e)
        {
            if (lv_MultiDeck.Items.Count > 0)
            {
                if (lv_MultiDeck.SelectedItems.Count > 0)
                {
                    ListViewItem arq = lv_MultiDeck.SelectedItems[0];
                    if (arq != lv_MultiDeck.Items[0])
                    {
                        var indice = lv_MultiDeck.SelectedIndices[0];

                        lv_MultiDeck.Items.RemoveAt(indice);
                        lv_MultiDeck.Items.Insert(indice - 1, arq);

                    }
                }
            }
        }

        private void btn_Down_Click(object sender, EventArgs e)
        {
            if (lv_MultiDeck.Items.Count > 0)
            {
                if (lv_MultiDeck.SelectedItems.Count > 0)
                {
                    ListViewItem arq = lv_MultiDeck.SelectedItems[0];
                    int qtde = lv_MultiDeck.Items.Count - 1;
                    if (arq != lv_MultiDeck.Items[qtde])
                    {
                        var indice = lv_MultiDeck.SelectedIndices[0];

                        lv_MultiDeck.Items.RemoveAt(indice);
                        lv_MultiDeck.Items.Insert(indice + 1, arq);

                    }
                }
            }
        }

        private void textOrigem_DragEnter_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;

        }
        private void textOrigem_DragDrop_1(object sender, DragEventArgs e)
        {
            // textOrigem.Text = "";
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textOrigem.Text = files.First();

            }

        }

        private void textMeta_DragEnter_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;

        }
        private void textMeta_DragDrop_1(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textMeta.Text = files.First();

            }
        }

        private void textSaida_DragEnter_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;

        }

        private void textSaida_DragDrop_1(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textSaida.Text = files.First();

            }
        }

        private void btn_SearchResul_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = this.textResul.Text;
            fbd.Description = "SELECIONE O DIRETÓRIO";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textResul.Text = fbd.SelectedPath;
            }
        }

        private void textResul_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textResul.Text = files.First();

            }
        }

        private void textResul_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void btn_resultado_Click(object sender, EventArgs e)
        {
            if (textResul.Text == "" || !Directory.Exists(textResul.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textResul.Focus();
                return;
            }

            Thread nthread = new Thread(Program.DStools_resultado);
            nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            nthread.Start(textResul.Text);

        }

        private void btn_VerGraph_Click(object sender, EventArgs e)
        {
            if (textResul.Text == "" || !Directory.Exists(textResul.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textResul.Focus();
                return;
            }

            Thread nthread = new Thread(Program.DStools_ResGraph);
            nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            nthread.Start(textResul.Text);
        }

        private void btn_AllRes_Click(object sender, EventArgs e)
        {
            if (textResul.Text == "" || !Directory.Exists(textResul.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textResul.Focus();
                return;
            }

            Thread nthread = new Thread(Program.DSTool_AllResultados);
            nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            nthread.Start(textResul.Text);

        }

        private void btn_AllGraph_Click(object sender, EventArgs e)
        {
            if (textResul.Text == "" || !Directory.Exists(textResul.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                textResul.Focus();
                return;
            }

            Thread nthread = new Thread(Program.DSTool_AllGraphs);
            nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            nthread.Start(textResul.Text);
        }

        private void search_CompSem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = this.text_ComplDir.Text;
            fbd.Description = "SELECIONE O DIRETÓRIO";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.text_ComplDir.Text = fbd.SelectedPath;
            }
        }

        private void text_ComplDir_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                text_ComplDir.Text = files.First();

            }
        }

        private void text_ComplDir_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void Iniciar_CompSem_Click(object sender, EventArgs e)
        {

            if (text_ComplDir.Text == "" || !Directory.Exists(text_ComplDir.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                text_ComplDir.Focus();
                return;
            }
            string comando = text_ComplDir.Text;
            if (check_Expand.Checked)
            {
                comando = comando + "|true";
            }

            Thread nthread = new Thread(Program.DStools_complSem);
            nthread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            nthread.Start(comando);

        }

        private void dessemSabRVXbox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                dessemSabRVXbox.Text = files.First();
            }
        }

        private void dessemSabRVXbox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void dessemBaseRVXbox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void dessemBaseRVXbox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                dessemBaseRVXbox.Text = files.First();
            }
        }

        private void decompBaseRVXbox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                decompBaseRVXbox.Text = files.First();
            }
        }

        private void decompBaseRVXbox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private string SearchDirectoryBottom()
        {
            string directory = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.SelectedPath = this.text_ComplDir.Text;
            fbd.Description = "SELECIONE O DIRETÓRIO";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                directory = fbd.SelectedPath;
            }
            return directory;
        }

        private void saidaRVXBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void saidaRVXBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                saidaRVXBox.Text = files.First();
            }
        }

        private void btn_DsSabProc_Click(object sender, EventArgs e)
        {
            dessemSabRVXbox.Text = SearchDirectoryBottom();
        }

        private void btn_DsBaseProc_Click(object sender, EventArgs e)
        {
            dessemBaseRVXbox.Text = SearchDirectoryBottom();
        }

        private void btn_DcBaseProc_Click(object sender, EventArgs e)
        {
            decompBaseRVXbox.Text = SearchDirectoryBottom();
        }

        private void btn_SaidaRVXProc_Click(object sender, EventArgs e)
        {
            saidaRVXBox.Text = SearchDirectoryBottom();
        }
        
        public bool VerificaDIRSRVX()
        {
            if (decompBaseRVXbox.Text == "" || !Directory.Exists(decompBaseRVXbox.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                decompBaseRVXbox.Focus();
                return false;
            }
            if (dessemBaseRVXbox.Text == "" || !Directory.Exists(dessemBaseRVXbox.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                dessemBaseRVXbox.Focus();
                return false;
            }
            if (dessemSabRVXbox.Text == "" || !Directory.Exists(dessemSabRVXbox.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                dessemSabRVXbox.Focus();
                return false;
            }
            if (saidaRVXBox.Text == "" || !Directory.Exists(saidaRVXBox.Text))
            {
                string aviso = "Diretório inválido ou inexistente!";
                MessageBox.Show(aviso, "DESSEM-TOOLS");
                saidaRVXBox.Focus();
                return false;
            }
            return true;
        }
        private void btn_RVXStart_Click(object sender, EventArgs e)
        {
            try
            {
                bool prosseguir = VerificaDIRSRVX();
                if (prosseguir)
                {
                    var deckSab = DeckFactory.CreateDeck(dessemSabRVXbox.Text);
                    var deckDESSEMref = DeckFactory.CreateDeck(dessemBaseRVXbox.Text);
                    var deckDECOMPref = DeckFactory.CreateDeck(decompBaseRVXbox.Text);

                    DateTime dat = DateTime.Today;
                    DateTime sabFut = dat;
                    while (sabFut.DayOfWeek != DayOfWeek.Saturday) sabFut = sabFut.AddDays(1);

                    DateTime datVE = dat;
                    if (dat.DayOfWeek == DayOfWeek.Friday)
                    {
                        datVE = dat.AddDays(-1);
                    }
                    var rev = Tools.GetNextRev(datVE);


                    string dirSaida = Path.Combine(saidaRVXBox.Text, $"Dessem_RV{rev.rev}-" + dat.ToString("dd-MM-yyyy"));

                    if (deckSab is CommomLibrary.Dessem.Deck && deckDESSEMref is CommomLibrary.Dessem.Deck && deckDECOMPref is CommomLibrary.Decomp.Deck)
                    {
                        deckDESSEMref.CopyFilesToFolder(dirSaida);

                        bool copiarMapCorte = Services.DessemRVX.SalvarMapcutCortedeco(decompBaseRVXbox.Text, dirSaida);
                        if (copiarMapCorte == false)
                        {
                            return;
                        }

                        Services.DessemRVX.CriarDeflant(dirSaida, sabFut);
                        Services.DessemRVX.CriarCotasr11(dirSaida, sabFut);
                        Services.DessemRVX.CriarPtoper(dirSaida, sabFut);


                    }
                    int p = 0;
                    progressPercent.Text = p + "%";
                    //progressPercent.Update();
                    while (p < 100)
                    {
                        progressBarRVX.Value += 1;
                        p++;
                        progressPercent.Text = p + "%";

                    }
                    Program.testeProgress();

                }
            }
            catch (Exception ex)
            {

                if (ex.ToString().Contains("Arquivo Níveis de partida não encontrados para criação do Deflant.dat"))
                {
                    
                    string texto = ex.Message + ", processo interrompido.";
                    MessageBox.Show(texto, "Dessem Tools");
                    return;
                }
            }

        }

    }

}
