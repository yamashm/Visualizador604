using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Globalization;
using System.Threading;
using System.ComponentModel;

namespace Visualizador604
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> _BancosValores;
        List<string> _DadosLotes;
        List<string> _Duplicados;
        List<string> _dadosUnicos;

        string _caminhoarquivoaberto;
        //bool _arquivoEBCDIC;

        byte _tipoArquivoAberto;

        ArquivoCELNRA604 _registros604;
        ArquivoCELNRA605 _registros605;
        ArquivoDAD604 _registrosDAD604;
        ArquivoDAD606 _registrosDAD606;
        ArquivoCEL674 _registro674;
        ArquivoEBCDIC _registrosEBCDIC;

        //public delegate void CarregouBarra(double valor);
        //public event CarregouBarra OnCarregouBarra;

        //public delegate void Carregou(List<Dado604> dados, List<string> bancosValores, List<string> dadosLotes, string tipoarq, string versaoarquivo, string datamov, string qtdcheques, string qtdfechamentos, string valortotal);
        //public event Carregou OnCarregou;

        public MainWindow()
        {
            InitializeComponent();

            gpbCarregando.Visibility = System.Windows.Visibility.Hidden;
            gpbDadosArq.Visibility = System.Windows.Visibility.Hidden;
            MenuPesquisar.Visibility = System.Windows.Visibility.Hidden;
            MenuItemCopiar.IsEnabled = false;
            MenuItemCopiarSelecao.IsEnabled = false;
            //MenuItemDuplicarLinha.IsEnabled = false;
            MenuItemExtrair.IsEnabled = false;
            MenuItemRegerar.IsEnabled = false;
            MenuItemEditarArquivo.IsEnabled = false;

            gpbArqEBCDIC.Visibility = System.Windows.Visibility.Hidden;

            //_tipoArquivoAberto = 0;

            Autenticacao autenticacao = new Autenticacao();

            if (autenticacao.ShowDialog() == false)
            {
                this.Close();
            }
        }

        //void MainWindow_OnCarregou(List<Dado604> dados, List<string> bancosValores, List<string> dadosLotes, string tipoarq, string versaoarquivo, string datamov, string qtdcheques, string qtdfechamentos, string valortotal)
        //{
        //                this.Dispatcher.Invoke((Action)delegate
        //    {
        //    gpbDadosArq.Visibility = System.Windows.Visibility.Visible;
        //    MenuPesquisar.Visibility = System.Windows.Visibility.Visible;
        //    MenuItemCopiar.IsEnabled = true;
        //    MenuItemCopiarSelecao.IsEnabled = true;

        //    _dados = dados;
        //    _BancosValores = bancosValores;
        //    _DadosLotes = dadosLotes;

        //    PreencheLista(_dados);

        //    txbTipoArq.Text = tipoarq;
        //    txbVerArq.Text = versaoarquivo;
        //    txbDtAbertura.Text = new StringBuilder(datamov.Substring(0, 4)).Append("-").Append(datamov.Substring(4, 2)).Append("-").Append(datamov.Substring(6, 2)).ToString();

        //    txbQtdCheques.Text = qtdcheques.ToString();
        //    txbQtdLotes.Text = qtdfechamentos.ToString();
        //    txbValorTotal.Text = (Convert.ToDecimal(valortotal) / 100).ToString("N2");

        //    }, null);
        //}

        //void MainWindow_OnCarregouBarra(double valor)
        //{
        //    this.Dispatcher.Invoke((Action)delegate
        //    {
        //        pgbCarregando.Value = valor;

        //    }, null);
        //}

        //void carregando_OnCarregou(List<Dado604> dados, List<string> bancosValores, List<string> dadosLotes, string tipoarq, string versaoarquivo, string datamov, string qtdcheques, string qtdfechamentos, string valortotal)
        //{
        //    gpbDadosArq.Visibility = System.Windows.Visibility.Visible;
        //    MenuPesquisar.Visibility = System.Windows.Visibility.Visible;
        //    MenuItemCopiar.IsEnabled = true;
        //    MenuItemCopiarSelecao.IsEnabled = true;

        //    _dados = dados;
        //    _BancosValores = bancosValores;
        //    _DadosLotes = dadosLotes;

        //    PreencheLista(_dados);

        //    txbTipoArq.Text = tipoarq;
        //    txbVerArq.Text = versaoarquivo;
        //    txbDtAbertura.Text = new StringBuilder(datamov.Substring(0, 4)).Append("-").Append(datamov.Substring(4, 2)).Append("-").Append(datamov.Substring(6, 2)).ToString();

        //    txbQtdCheques.Text = qtdcheques.ToString();
        //    txbQtdLotes.Text = qtdfechamentos.ToString();
        //    txbValorTotal.Text = (Convert.ToDecimal(valortotal) / 100).ToString("N2");
        //}

        //private void AbreArquivoThread(string caminho)
        //{
        //    ThreadStart threadStart = new ThreadStart(delegate() { AbreArquivo(caminho); });
        //    Thread othread = new Thread(threadStart);
        //    othread.IsBackground = true;
        //    othread.Name = "AbreArquivo()";

        //    othread.Start();
        //}


        private void Destaca(Dado604 item)
        {
            Run run = new Run();

            run.Text = item.Text.Substring(147,3);

            run.Background = new SolidColorBrush(Colors.Red);
        }

        private void ltbSaida_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }

        private void MenuItemSair_CLick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (ltbSaida.Items.Count > 0)
                {
                    ltbSaida.SelectedItems.Clear();
                    ltbSaida.SelectAll();
                }
            }

            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                CopiaSelec();
            }

            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Pesquisar();
            }

            switch (e.Key)
            {
                case Key.Enter:
                    MostraDetalhesLinha();
                break;

                case Key.T:
                Destaca((Dado604)ltbSaida.SelectedItem);
                break;
            }

        }

        private void MostraDetalhesLinha()
        {
            if (ltbSaida.SelectedIndex != -1)
            {
                switch (_tipoArquivoAberto)
                {
                    case 1:
                        DetalheArquivoCompe detalhe604 = (DetalheArquivoCompe)ltbSaida.SelectedItem;
                        DetalhesRegistro detalhes604;

                        if (detalhe604.INDICE_CHEQUE == 0)
                        {
                            detalhes604 = new DetalhesRegistro(ltbSaida.SelectedItem, _tipoArquivoAberto);
                            detalhes604.ShowDialog();
                        }
                        else
                        {
                            LoteCEL604 lote = _registros604.LOTES.Find(x => x.INDICE == detalhe604.INDICE_LOTE);
                            Cheque cheque = lote.CHEQUES.Find(x => x.INDICE == detalhe604.INDICE_CHEQUE);

                            if (cheque != null)
                            {
                                detalhes604 = new DetalhesRegistro(ltbSaida.SelectedItem, cheque, _tipoArquivoAberto);
                                detalhes604.ShowDialog();
                            }
                        }
                        break;
                    case 2:
                        DetalheArquivoCompe detalhe605 = (DetalheArquivoCompe)ltbSaida.SelectedItem;
                        DetalhesRegistro detalhes605;

                        if (detalhe605.INDICE_CHEQUE == 0)
                        {
                            detalhes605 = new DetalhesRegistro(ltbSaida.SelectedItem, _tipoArquivoAberto);
                            detalhes605.ShowDialog();
                        }
                        else
                        {
                            LoteCEL605 lote = _registros605.LOTES.Find(x => x.INDICE == detalhe605.INDICE_LOTE);
                            Cheque cheque = lote.CHEQUES.Find(x => x.INDICE == detalhe605.INDICE_CHEQUE);

                            if (cheque != null)
                            {
                                detalhes605 = new DetalhesRegistro(ltbSaida.SelectedItem, cheque, _tipoArquivoAberto);
                                detalhes605.ShowDialog();
                            }
                        }
                        break;
                    case 3:
                        DetalheArquivoCompe detalheCEL674 = (DetalheArquivoCompe)ltbSaida.SelectedItem;
                        DetalhesRegistro detalhesCEL674;

                        detalhesCEL674 = new DetalhesRegistro(ltbSaida.SelectedItem, _tipoArquivoAberto);
                        detalhesCEL674.ShowDialog();

                        break;
                    case 4:
                        DetalheArquivoCompe detalheDAD604 = (DetalheArquivoCompe)ltbSaida.SelectedItem;
                        DetalhesRegistro detalhesDAD604;

                        if (detalheDAD604.INDICE_CHEQUE == 0)
                        {
                            detalhesDAD604 = new DetalhesRegistro(ltbSaida.SelectedItem, _tipoArquivoAberto);
                            detalhesDAD604.ShowDialog();
                        }
                        else
                        {
                            Cheque cheque = _registrosDAD604.CHEQUES.Find(x => x.INDICE == detalheDAD604.INDICE_CHEQUE);

                            if (cheque != null)
                            {
                                detalhesDAD604 = new DetalhesRegistro(ltbSaida.SelectedItem, cheque, _tipoArquivoAberto);
                                detalhesDAD604.ShowDialog();
                            }
                        }

                        break;
                    case 5:
                        DetalheArquivoCompe detalheDAD606 = (DetalheArquivoCompe)ltbSaida.SelectedItem;
                        DetalhesRegistro detalhesDAD606;

                        if (detalheDAD606.INDICE_CHEQUE == 0)
                        {
                            detalhesDAD606 = new DetalhesRegistro(ltbSaida.SelectedItem, _tipoArquivoAberto);
                            detalhesDAD606.ShowDialog();
                        }
                        else
                        {
                            Cheque cheque = _registrosDAD606.CHEQUES.Find(x => x.INDICE == detalheDAD606.INDICE_CHEQUE);

                            if (cheque != null)
                            {
                                detalhesDAD606 = new DetalhesRegistro(ltbSaida.SelectedItem, cheque, _tipoArquivoAberto);
                                detalhesDAD606.ShowDialog();
                            }
                        }
                        break;
                }
            }
        }

        private void btnValorLote_Click(object sender, RoutedEventArgs e)
        {
            if (_BancosValores != null)
            {
                ValoresLotes valorlotes = new ValoresLotes(_BancosValores);
                    valorlotes.ShowDialog();
            }
        }

        private void MenuPesquisar_Click(object sender, RoutedEventArgs e)
        {
            Pesquisar();
        }

        private void Pesquisar()
        {
            Pesquisa pesquisa = new Pesquisa(_registros604);

            //Pesquisa pesquisa = new Pesquisa(_dados);

            pesquisa.OnProximo += pesquisa_OnProximo;

            pesquisa.ShowDialog();

            pesquisa.OnProximo -= pesquisa_OnProximo;

            //if (ltbSaida.SelectedItem != null)
            //{
            //    Dado604 itemanterior = (Dado604)ltbSaida.SelectedItem;

            //    itemanterior.Background = new SolidColorBrush(Colors.Transparent);
            //}
        }

        void pesquisa_OnProximo(List<int> ItemsEncontrados)
        {
            //foreach (ListBoxItem item in ltbSaida.Items)
            //{
            //    DetalheArquivoCompe d = (DetalheArquivoCompe)item;
            //    d.Background = new SolidColorBrush(Colors.Transparent);
            //}

            foreach (int i in ItemsEncontrados)
            {
                DetalheArquivoCompe d = (DetalheArquivoCompe)ltbSaida.Items[i];
                d.Background = new SolidColorBrush(Colors.Green);
            }
            //if (ltbSaida.SelectedItem != null)
            //{
            //    Dado604 itemanterior = (Dado604)ltbSaida.SelectedItem;

            //    itemanterior.Background = new SolidColorBrush(Colors.Transparent);
            //}

            //ltbSaida.SelectedItem = ProximoItem;

            //Dado604 itemselecionado = (Dado604)ltbSaida.SelectedItem;

            //itemselecionado.Background = new SolidColorBrush(Colors.Red);

            //ltbSaida.ScrollIntoView(ProximoItem);
        }

        //void pesquisa_OnProximo(Dado604 ProximoItem)
        //{
        //    if (ltbSaida.SelectedItem != null)
        //    {
        //        Dado604 itemanterior = (Dado604)ltbSaida.SelectedItem;

        //        itemanterior.Background = new SolidColorBrush(Colors.Transparent);
        //    }

        //    ltbSaida.SelectedItem = ProximoItem;

        //    Dado604 itemselecionado = (Dado604)ltbSaida.SelectedItem;

        //    itemselecionado.Background = new SolidColorBrush(Colors.Red);
         
        //    ltbSaida.ScrollIntoView(ProximoItem);
        //}

        private void MenuItemCopiar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DetalheArquivoCompe dado in ltbSaida.Items)
            {
                sb.Append(dado.Text).Append("\r\n");
            }

            Clipboard.SetText(sb.ToString());
        }

        private void btnDadosLotes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_DadosLotes != null)
            {
                DadosLotes dadoslotes = new DadosLotes(_DadosLotes);

                dadoslotes.ShowDialog();
            }
        }

        private void MenuItemCopiarSelec_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CopiaSelec();
        }

        private void CopiaSelec()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DetalheArquivoCompe dado in ltbSaida.SelectedItems)
            {
                sb.Append(dado.Text).Append("\r\n");
            }

            Clipboard.SetText(sb.ToString());
        }

        private void MenuVerificaDuplicados_Click(object sender, RoutedEventArgs e)
        {
            _Duplicados = new List<string>();
            _dadosUnicos.GroupBy(txt => txt).Where(grouping => grouping.Count() > 1).ToList().ForEach(groupItem => _Duplicados.Add(groupItem.Key.ToString()));

            if (_Duplicados.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("CMC7 dos cheques repetidos: ").Append("\n\r");

                foreach (string s in _Duplicados)
                {
                    sb.Append(s.Substring(8,30)).Append("\n\r");
                }

                MessageBox.Show(sb.ToString());
            }
            else
            {
                MessageBox.Show("Não há cheques duplicados.");
            }

        }

        private void MenuItemDuplicar_Click(object sender, RoutedEventArgs e)
        {
            //Dado604 dadoselec = (Dado604)ltbSaida.SelectedItem;

            //Dado604 novo = new Dado604();
            //novo.CMC7 = dadoselec.CMC7;
            //novo.Linha = dadoselec.Linha + 1;
            //novo.binImagemAss = new byte[27408];
            //novo.binDados = new byte[27648];

            //Array.Copy(dadoselec.binImagemAss, novo.binImagemAss, 27408);
            //Array.Copy(dadoselec.binDados, novo.binDados, 27648);
       
            //novo.CRC = dadoselec.CRC;
            //novo.Text = dadoselec.Text;
            //novo.Tag = dadoselec.Tag;

            //novo.FontFamily = new System.Windows.Media.FontFamily("Courier New");
            //novo.FontSize = 14;
            //novo.Height = GetScreenSize("0", new FontFamily("Courier New"), 14, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal).Height;

            //switch ((int)novo.Tag)
            //{
            //    case 0:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkBlue);
            //        break;
            //    case 2:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkMagenta);
            //        break;
            //    case 3:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkRed);
            //        break;
            //    case 12:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkCyan);
            //        break;
            //    case 20:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkBlue);
            //        break;
            //    case 22:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkCyan);
            //        break;
            //    case 29:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkRed);
            //        break;
            //    default:
            //        novo.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
            //        break;
            //}

            //ltbSaida.Items.Insert(novo.Linha, novo);
            //_dados.Insert(novo.Linha, novo);
        }

        private void MenuItemSalvar_Click(object sender, RoutedEventArgs e)
        {
            //string caminho = @"C:\Projetos15\Teste.rec";

            //BinaryWriter bw = new BinaryWriter(File.Open(caminho, FileMode.OpenOrCreate));

            //foreach (Dado604 dado in _dados)
            //{
            //    bw.Write(dado.binDados);
            //}

            //bw.Flush();
            //bw.Close();

        }

        private void MenuItemExtrairImagens_Click(object sender, RoutedEventArgs e)
        {
            //Extraindo ext = new Extraindo(_registros604);
            //    ext.ShowDialog();

            Extraindo ext;

            switch (_tipoArquivoAberto)
            {
                case 1:
                    ext = new Extraindo(_registros604);
                    ext.ShowDialog();
                    break;
                case 4:
                    ext = new Extraindo(_registrosDAD604);
                    ext.ShowDialog();
                    break;
            }
        }

        private void MenuItemAbrir604_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                ZeraRegistros();

                if (AbreArquivoCEL604(ofd.FileName))
                {
                    ConfiguraTela(true, ofd.FileName);
                    PreencheListaSaidaArquivoCEL604();
                }
                else
                {
                    ConfiguraTela(false, ofd.FileName);
                }
            }
        }

        private void MenuItemAbrir605_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                ZeraRegistros();

                if (AbreArquivoCEL605(ofd.FileName))
                {
                    ConfiguraTela(true, ofd.FileName);
                    PreencheListaSaidaArquivoCEL605();
                }
                else
                {
                    ConfiguraTela(false, ofd.FileName);
                }
            }
        }

        private void MenuItemAbrirDAD606_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                ZeraRegistros();
                if (AbreArquivoDAD606(ofd.FileName))
                {
                    ConfiguraTela(true, ofd.FileName);
                    PreencheListaSaidaArquivoDAD606();
                }
                else
                {
                    ConfiguraTela(false, ofd.FileName);
                }
            }
        }

        private void MenuItemAbrirDAD604_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                ZeraRegistros();
                if (AbreArquivoDAD604(ofd.FileName))
                {
                    ConfiguraTela(true, ofd.FileName);
                    PreencheListaSaidaArquivoDAD604();
                }
                else
                {
                    ConfiguraTela(false, ofd.FileName);
                }
            }
        }

        private void MenuItemAbrirDAD674_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                ZeraRegistros();
                if (AbreArquivoCEL674(ofd.FileName))
                {
                    ConfiguraTela(true, ofd.FileName);
                }
                else
                {
                    ConfiguraTela(false, ofd.FileName);
                }
            }
        }

        private bool AbreArquivoCEL604(string caminho)
        {
            bool retorno = true;

            _tipoArquivoAberto = 1;
            //_arquivoEBCDIC = true;

            byte[] binImagemAss = null;
            LoteCEL604 lote = null;
            Cheque cheque = null;
            int indiceCheque = 0;
            int indiceLote = 0;

            _registros604 = new ArquivoCELNRA604();
            _registros604.LOTES = new List<LoteCEL604>();
            _registros604.LINHASERRO = new List<LinhaErro>();

            _registros604.TIPO_ARQUIVO = 1;

            int linha = 0;
            int quebra = _registros604.QUEBRA;
            int comprimentoTotalLinha = _registros604.COMPRIMENTO_LINHA;
            int restoLinha = comprimentoTotalLinha - quebra;

            List<byte[]> linhas = Util.MontaListaBinariosArquivo(new FileInfo(caminho), comprimentoTotalLinha);

            string linhaASCII = string.Empty;

            foreach (byte[] b in linhas)
            {
                try
                {
                    linha++;
                    byte[] binDados = new byte[quebra];
                    Array.Copy(b, 0, binDados, 0, quebra);

                    linhaASCII = Encoding.UTF8.GetString(Tabela.ConverteEBCDICParaASCII(binDados));

                    if (linhaASCII.Substring(0, 47) == new string('0',47))
                    {
                        _registros604.HEADER = new HeaderCEL604(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkBlue));
                        _registros604.HEADER.INDICE_CHEQUE = 0;
                        _registros604.HEADER.INDICE_LOTE = 0;

                        _registros604.NOME_ARQUIVO = _registros604.HEADER.CAMPOS.Find(x => x.NUMERO == 2).CONTEUDO;
                        _registros604.VERSAO_ARQUIVO = _registros604.HEADER.CAMPOS.Find(x => x.NUMERO == 3).CONTEUDO;
                        _registros604.INDICADOR_REMESSA = _registros604.HEADER.CAMPOS.Find(x => x.NUMERO == 6).CONTEUDO;
                        _registros604.DATA_MOVIMENTO = _registros604.HEADER.CAMPOS.Find(x => x.NUMERO == 7).CONTEUDO;
                    }

                    else if (linhaASCII.Substring(0, 47) == new string('9', 47))
                    {
                        _registros604.TRAILER = new TrailerCEL604(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkRed));
                        _registros604.TRAILER.INDICE_CHEQUE = 0;
                        _registros604.TRAILER.INDICE_LOTE = 0;
                    }

                    else if (linhaASCII.Substring(6, 27) == new string('9', 27))
                    {
                        lote.FECHAMENTO = new FechamentoCEL604(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkMagenta));
                        _registros604.LOTES.Add(lote);
                        lote.FECHAMENTO.INDICE_CHEQUE = 0;
                        lote.FECHAMENTO.INDICE_LOTE = indiceLote;
                        lote.INDICE = indiceLote;

                        lote = null;
                    }
                    else 
                    {
                        if (lote == null)
                        {
                            indiceLote++;
                            lote = new LoteCEL604();
                            lote.CHEQUES = new List<Cheque>();
                        }

                        DetalheCEL604 detalhe = new DetalheCEL604(linhaASCII, linha, quebra, comprimentoTotalLinha);

                        if (cheque == null)
                        {
                            indiceCheque++;

                            cheque = new Cheque();

                            cheque.CMC7 = Util.MontaCMC7(detalhe.CAMPOS[0].CONTEUDO,
                                    detalhe.CAMPOS[1].CONTEUDO,
                                    detalhe.CAMPOS[2].CONTEUDO,
                                    detalhe.CAMPOS[3].CONTEUDO,
                                    detalhe.CAMPOS[4].CONTEUDO,
                                    detalhe.CAMPOS[5].CONTEUDO,
                                    detalhe.CAMPOS[6].CONTEUDO,
                                    detalhe.CAMPOS[7].CONTEUDO,
                                    detalhe.CAMPOS[10].CONTEUDO);

                            cheque.VALOR = Convert.ToInt64(detalhe.CAMPOS[9].CONTEUDO);

                            cheque.INDICE = indiceCheque;
                            cheque.INDICE_LOTE = indiceLote;

                            cheque.DADOS_VERSO = new List<DetalheArquivoCompe>();
                            cheque.DADOS_FRENTE = new List<DetalheArquivoCompe>();

                            cheque.NUMERO = Convert.ToInt32(detalhe.CAMPOS[6].CONTEUDO);

                            lote.NUMERO = Convert.ToInt32(detalhe.CAMPOS[19].CONTEUDO);
                        }

                        if (detalhe.CAMPOS[32].CONTEUDO == "V")
                        {
                            detalhe.Foreground = new SolidColorBrush(Colors.DarkCyan);
                            cheque.DADOS_VERSO.Add(detalhe);
                            detalhe.VERSO = true;
                        }
                        else
                        {
                            detalhe.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
                            cheque.DADOS_FRENTE.Add(detalhe);
                            detalhe.VERSO = false;
                        }

                        byte totalRegistrosImagem = Convert.ToByte(detalhe.CAMPOS[28].CONTEUDO);

                        if (totalRegistrosImagem > 0)
                        {
                            byte sequencialImagem = Convert.ToByte(detalhe.CAMPOS[29].CONTEUDO);
                            long tamanhoImagem = Convert.ToInt64(detalhe.CAMPOS[30].CONTEUDO);
                            long tamanhoAssinatura = Convert.ToInt64(detalhe.CAMPOS[31].CONTEUDO);
                            long tamanhoTotal = tamanhoImagem + tamanhoAssinatura;
                            long tamanhoDetalheImAss = 0;

                            if (sequencialImagem == 1)
                            {
                                binImagemAss = new byte[tamanhoTotal];
                            }

                            if (sequencialImagem == totalRegistrosImagem)
                            {
                                tamanhoDetalheImAss = tamanhoTotal;

                                while (tamanhoDetalheImAss > 0)
                                {
                                    tamanhoDetalheImAss = tamanhoDetalheImAss - restoLinha;
                                }

                                Array.Copy(b, quebra, binImagemAss, (sequencialImagem - 1) * restoLinha, restoLinha + tamanhoDetalheImAss);

                                if (cheque.IMAGEM_FRENTE == null)
                                {
                                    cheque.IMAGEM_FRENTE = new byte[tamanhoImagem];
                                    cheque.ASSINATURA_FRENTE = new byte[tamanhoAssinatura];
                                    cheque.TOTAL_REGISTROS_IMAGEM_FRENTE = totalRegistrosImagem;
                                    Array.Copy(binImagemAss, 0, cheque.IMAGEM_FRENTE, 0, tamanhoImagem);
                                    Array.Copy(binImagemAss, tamanhoImagem, cheque.ASSINATURA_FRENTE, 0, tamanhoAssinatura);
                                }
                                else
                                {
                                    cheque.IMAGEM_VERSO = new byte[tamanhoImagem];
                                    cheque.ASSINATURA_VERSO = new byte[tamanhoAssinatura];
                                    cheque.TOTAL_REGISTROS_IMAGEM_VERSO = totalRegistrosImagem;
                                    Array.Copy(binImagemAss, 0, cheque.IMAGEM_VERSO, 0, tamanhoImagem);
                                    Array.Copy(binImagemAss, tamanhoImagem, cheque.ASSINATURA_VERSO, 0, tamanhoAssinatura);

                                    lote.CHEQUES.Add(cheque);
                                    cheque = null;
                                }
                            }
                            else
                                Array.Copy(b, quebra, binImagemAss, (sequencialImagem - 1) * restoLinha, restoLinha);
                        }
                        else
                        {
                            lote.CHEQUES.Add(cheque);

                            cheque = null;
                        }

                        detalhe.INDICE_LOTE = indiceLote;
                        detalhe.INDICE_CHEQUE = indiceCheque;
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message + " Linha: " + linha.ToString(), "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    _registros604.LINHASERRO.Add(new LinhaErro(linhaASCII, linha, new SolidColorBrush(Colors.Red)));
                    
                    continue;
                }
            }

            return retorno;
        }

        private void PreencheListaSaidaArquivoCEL604()
        {
            ltbSaida.Items.Clear();

            int quantidadeCheques = 0;

            ltbSaida.Items.Add(_registros604.HEADER);

            foreach (LoteCEL604 lote in _registros604.LOTES)
            {
                foreach (Cheque cheque in lote.CHEQUES)
                {
                    foreach (DetalheArquivoCompe d in cheque.DADOS_FRENTE)
                    {
                        ltbSaida.Items.Add(d);
                    }

                    if (cheque.DADOS_VERSO != null)
                    {
                        foreach (DetalheArquivoCompe d in cheque.DADOS_VERSO)
                        {
                            ltbSaida.Items.Add(d);
                        }
                    }

                    quantidadeCheques++;
                }
                ltbSaida.Items.Add(lote.FECHAMENTO);
            }

            ltbSaida.Items.Add(_registros604.TRAILER);

            foreach (DetalheArquivoCompe d in _registros604.LINHASERRO)
            {
                ltbSaida.Items.Add(d);
            }

            ltbSaida.Items.SortDescriptions.Add(new SortDescription("NUMERO_LINHA", ListSortDirection.Ascending));

            if (_registros604.HEADER != null)
            {
                txbTipoArq.Text = _registros604.HEADER.CAMPOS.Find(x => x.NUMERO == 2).CONTEUDO;
                txbVerArq.Text = _registros604.HEADER.CAMPOS.Find(x => x.NUMERO == 3).CONTEUDO;
                string datamov = _registros604.HEADER.CAMPOS.Find(x => x.NUMERO == 7).CONTEUDO;
                txbDtAbertura.Text = new StringBuilder(datamov.Substring(0, 4)).Append("-").Append(datamov.Substring(4, 2)).Append("-").Append(datamov.Substring(6, 2)).ToString();
            }

            txbQtdCheques.Text = quantidadeCheques.ToString();
            txbQtdLotes.Text = _registros604.LOTES.Count().ToString();

            if (_registros604.TRAILER != null)
            {
                string strValorTotal = _registros604.TRAILER.CAMPOS.Find(x => x.NUMERO == 8).CONTEUDO;
                decimal valorTotal = 0;
                if(decimal.TryParse(strValorTotal, out valorTotal))
                    txbValorTotal.Text = (valorTotal / 100).ToString("N2");
                else
                    txbValorTotal.Text = "0,00";
            }
        }

        private bool AbreArquivoCEL605(string caminho)
        {
            bool retorno = true;

            _tipoArquivoAberto = 2;
            //_arquivoEBCDIC = false;

            LoteCEL605 lote = null;
            Cheque cheque = null;
            int indiceCheque = 0;
            int indiceLote = 0;

            _registros605 = new ArquivoCELNRA605();
            _registros605.LOTES = new List<LoteCEL605>();

            _registros605.TIPO_ARQUIVO = 2;

            int linha = 0;

            List<string> linhas = Util.MontaListaLinhasArquivoASCII(new FileInfo(caminho));

            foreach (string linhaASCII in linhas)
            {
                try
                {
                    linha++;

                    if (linhaASCII.Substring(0, 47) == new string('0', 47))
                    {
                        _registros605.HEADER = new HeaderCEL605(linhaASCII, linha, new SolidColorBrush(Colors.DarkBlue));
                        _registros605.HEADER.INDICE_CHEQUE = 0;
                        _registros605.HEADER.INDICE_LOTE = 0;

                        _registros605.NOME_ARQUIVO = _registros605.HEADER.CAMPOS.Find(x => x.NUMERO == 2).CONTEUDO;
                        _registros605.VERSAO_ARQUIVO = _registros605.HEADER.CAMPOS.Find(x => x.NUMERO == 3).CONTEUDO;
                        _registros605.INDICADOR_REMESSA = _registros605.HEADER.CAMPOS.Find(x => x.NUMERO == 6).CONTEUDO;
                        _registros605.DATA_MOVIMENTO = _registros605.HEADER.CAMPOS.Find(x => x.NUMERO == 7).CONTEUDO;
                    }

                     else if (linhaASCII.Substring(0, 47) == new string('9', 47))
                    {
                        _registros605.TRAILER = new TrailerCEL605(linhaASCII, linha, new SolidColorBrush(Colors.DarkRed));
                        _registros605.TRAILER.INDICE_CHEQUE = 0;
                        _registros605.TRAILER.INDICE_LOTE = 0;
                    }

                    else if (linhaASCII.Substring(6, 27) == new string('9', 27))
                    {
                        lote.FECHAMENTO = new FechamentoCEL605(linhaASCII, linha, new SolidColorBrush(Colors.DarkMagenta));
                        _registros605.LOTES.Add(lote);
                        lote.FECHAMENTO.INDICE_CHEQUE = 0;
                        lote.FECHAMENTO.INDICE_LOTE = indiceLote;
                        lote.INDICE = indiceLote;

                        lote = null;
                    }
                    else
                    {
                        if (lote == null)
                        {
                            indiceLote++;
                            lote = new LoteCEL605();
                            lote.CHEQUES = new List<Cheque>();
                        }

                        DetalheCEL605 detalhe = new DetalheCEL605(linhaASCII, linha);

                        if (cheque == null)
                        {
                            indiceCheque++;

                            cheque = new Cheque();

                            cheque.CMC7 = Util.MontaCMC7(detalhe.CAMPOS[0].CONTEUDO,
                                    detalhe.CAMPOS[1].CONTEUDO,
                                    detalhe.CAMPOS[2].CONTEUDO,
                                    detalhe.CAMPOS[3].CONTEUDO,
                                    detalhe.CAMPOS[4].CONTEUDO,
                                    detalhe.CAMPOS[5].CONTEUDO,
                                    detalhe.CAMPOS[6].CONTEUDO,
                                    detalhe.CAMPOS[7].CONTEUDO,
                                    detalhe.CAMPOS[10].CONTEUDO);

                            cheque.VALOR = Convert.ToInt64(detalhe.CAMPOS[9].CONTEUDO);

                            cheque.INDICE = indiceCheque;
                            cheque.INDICE_LOTE = indiceLote;

                            cheque.DADOS_VERSO = new List<DetalheArquivoCompe>();
                            cheque.DADOS_FRENTE = new List<DetalheArquivoCompe>();
                        }

                        detalhe.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
                        cheque.DADOS_FRENTE.Add(detalhe);

                        lote.CHEQUES.Add(cheque);
                        cheque = null;

                        detalhe.VERSO = false;

                        detalhe.INDICE_LOTE = indiceLote;
                        detalhe.INDICE_CHEQUE = indiceCheque;
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message, "Erro Linha: " + linha.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }
            }

            return retorno;
        }

        private void PreencheListaSaidaArquivoCEL605()
        {
            ltbSaida.Items.Clear();

            int quantidadeCheques = 0;

            ltbSaida.Items.Add(_registros605.HEADER);

            foreach (LoteCEL605 lote in _registros605.LOTES)
            {
                foreach (Cheque cheque in lote.CHEQUES)
                {
                    foreach (DetalheArquivoCompe d in cheque.DADOS_FRENTE)
                    {
                        ltbSaida.Items.Add(d);
                    }

                    if (cheque.DADOS_VERSO != null)
                    {
                        foreach (DetalheArquivoCompe d in cheque.DADOS_VERSO)
                        {
                            ltbSaida.Items.Add(d);
                        }
                    }

                    quantidadeCheques++;
                }
                ltbSaida.Items.Add(lote.FECHAMENTO);
            }

            ltbSaida.Items.Add(_registros605.TRAILER);

            if (_registros605.HEADER != null)
            {
                txbTipoArq.Text = _registros605.HEADER.CAMPOS.Find(x => x.NUMERO == 2).CONTEUDO;
                txbVerArq.Text = _registros605.HEADER.CAMPOS.Find(x => x.NUMERO == 3).CONTEUDO;
                string datamov = _registros605.HEADER.CAMPOS.Find(x => x.NUMERO == 7).CONTEUDO;
                txbDtAbertura.Text = new StringBuilder(datamov.Substring(0, 4)).Append("-").Append(datamov.Substring(4, 2)).Append("-").Append(datamov.Substring(6, 2)).ToString();
            }

            txbQtdCheques.Text = quantidadeCheques.ToString();
            txbQtdLotes.Text = _registros605.LOTES.Count().ToString();

            if (_registros605.TRAILER != null)
            {
                string valorTotal = _registros605.TRAILER.CAMPOS.Find(x => x.NUMERO == 8).CONTEUDO;
                txbValorTotal.Text = (Convert.ToDecimal(valorTotal) / 100).ToString("N2");
            }
        }

        private bool AbreArquivoCEL674(string caminho)
        {
            bool retorno = true;

            ltbSaida.Items.Clear();

            _tipoArquivoAberto = 3;
            //_arquivoEBCDIC = false;

            ArquivoCEL674 registros674 = new ArquivoCEL674();
            registros674.LOTES_REMETIDOS = new List<LoteRemetido>();
            registros674.LOTES_RECEBIDOS = new List<LoteRecebido>();
            registros674.LOTES_REMETIDOS_COM_OCORRECIA = new List<LoteRemetidoComOcorrencia>();
            registros674.DETALHES_REMETIDOS_RECUSADOS = new List<DetalheRemetidoRecusado>();
            registros674.LOTES_RECEBIDOS_COM_OCORRENCIA = new List<LoteRecebidoComOcorrencia>();
            registros674.DETALHES_A_RECEBER_RECUSADOS = new List<DetalheAReceberRecusado>();
            registros674.OCORRECIAS_HEADER_TRAILER = new List<OcorrenciasHeadereTrailer>();
            registros674.VERSOES_PROCESSADAS = new List<VersoesProcessadas>();

            registros674.TIPO_ARQUIVO = 3;

            Ocorrencias.InicializaOcorrencias();

            int linha = 0;
            int quebra = 140;
            int comprimentoTotalLinha = 140;

            List<string> linhas = Util.MontaListaLinhasArquivoASCII(new FileInfo(caminho));

            foreach (string s in linhas)
            {
                try
                {
                    linha++;

                    if (s == "")
                        continue;

                    switch (s.Substring(0, 1))
                    {
                        case "0":
                            registros674.HEADER = new HeaderCEL674(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.Black));
                            ltbSaida.Items.Add(registros674.HEADER);
                            break;
                        case "1":
                            LoteRemetido loteRemetido = new LoteRemetido(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkBlue));
                            registros674.LOTES_REMETIDOS.Add(loteRemetido);
                            ltbSaida.Items.Add(loteRemetido);
                            break;
                        case "2":
                            LoteRecebido loteRecebido = new LoteRecebido(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.BlueViolet));
                            registros674.LOTES_RECEBIDOS.Add(loteRecebido);
                            ltbSaida.Items.Add(loteRecebido);
                            break;
                        case "3":
                            if (s.Substring(12, 2) == "44")
                            {
                                DetalheRemetidoRecusado detalheRemetidoRecusado = new DetalheRemetidoRecusado(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkRed));
                                registros674.DETALHES_REMETIDOS_RECUSADOS.Add(detalheRemetidoRecusado);
                                ltbSaida.Items.Add(detalheRemetidoRecusado);
                            }
                            else
                            {
                                LoteRemetidoComOcorrencia loteRemetidoComOcorrencia = new LoteRemetidoComOcorrencia(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkSalmon));
                                registros674.LOTES_REMETIDOS_COM_OCORRECIA.Add(loteRemetidoComOcorrencia);
                                ltbSaida.Items.Add(loteRemetidoComOcorrencia);
                            }
                            break;
                        case "4":
                            if (s.Substring(12, 2) == "44")
                            {
                                DetalheAReceberRecusado detalheAReceberRecusado = new DetalheAReceberRecusado(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkSeaGreen));
                                registros674.DETALHES_A_RECEBER_RECUSADOS.Add(detalheAReceberRecusado);
                                ltbSaida.Items.Add(detalheAReceberRecusado);
                            }
                            else
                            {
                                LoteRecebidoComOcorrencia loteRecebidoComOcorrencia = new LoteRecebidoComOcorrencia(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkTurquoise));
                                registros674.LOTES_RECEBIDOS_COM_OCORRENCIA.Add(loteRecebidoComOcorrencia);
                                ltbSaida.Items.Add(loteRecebidoComOcorrencia);
                            }
                            break;
                        case "5":
                            OcorrenciasHeadereTrailer ocorrenciasHeadereTrailer = new OcorrenciasHeadereTrailer(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkOrange));
                            registros674.OCORRECIAS_HEADER_TRAILER.Add(ocorrenciasHeadereTrailer);
                            ltbSaida.Items.Add(ocorrenciasHeadereTrailer);
                            break;
                        case "6":
                            VersoesProcessadas versoesProcessadas = new VersoesProcessadas(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkOrchid));
                            registros674.VERSOES_PROCESSADAS.Add(versoesProcessadas);
                            ltbSaida.Items.Add(versoesProcessadas);
                            break;
                        case "9":
                            registros674.TRAILER = new TrailerCEL674(s, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkGoldenrod));
                            ltbSaida.Items.Add(registros674.TRAILER);
                            break;
                    }

                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }
            }

            return retorno;
        }

        private bool AbreArquivoDAD604(string caminho)
        {
            bool retorno = true;

            ltbSaida.Items.Clear();

            _tipoArquivoAberto = 4;
            //_arquivoEBCDIC = true;

            byte[] binImagemAss = null;
            Cheque cheque = null;
            int indiceCheque = 0;

            _registrosDAD604 = new ArquivoDAD604();
            _registrosDAD604.CHEQUES = new List<Cheque>();

            _registrosDAD604.TIPO_ARQUIVO = 4;

            int linha = 0;
            int quebra = _registrosDAD604.QUEBRA;
            int comprimentoTotalLinha = _registrosDAD604.COMPRIMENTO_LINHA;
            int restoLinha = comprimentoTotalLinha - quebra;

            long valorTotalArquivo = 0;

            List<byte[]> linhas = Util.MontaListaBinariosArquivo(new FileInfo(caminho), comprimentoTotalLinha);

            foreach (byte[] b in linhas)
            {
                try
                {
                    linha++;
                    byte[] binDados = new byte[quebra];
                    Array.Copy(b, 0, binDados, 0, quebra);

                    string linhaASCII = Encoding.UTF8.GetString(Tabela.ConverteEBCDICParaASCII(binDados));

                    switch (linhaASCII.Substring(0, 1))
                    {
                        case "0":
                            _registrosDAD604.HEADER = new HeaderDAD604(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkBlue));
                            _registrosDAD604.HEADER.INDICE_CHEQUE = 0;
                            break;
                        case "9":
                            _registrosDAD604.TRAILER = new TrailerDAD604(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkRed));
                            _registrosDAD604.TRAILER.INDICE_CHEQUE = 0;
                            _registrosDAD604.TRAILER.VALOR_TOTAL = valorTotalArquivo;
                            break;
                        case "1":
                            DetalheDAD604 detalhe = new DetalheDAD604(linhaASCII, linha, quebra, comprimentoTotalLinha);

                            if (cheque == null)
                            {
                                indiceCheque++;

                                cheque = new Cheque();

                                cheque.CMC7 = Util.MontaCMC7(detalhe.CAMPOS[5].CONTEUDO,
                                        detalhe.CAMPOS[6].CONTEUDO,
                                        detalhe.CAMPOS[7].CONTEUDO,
                                        detalhe.CAMPOS[8].CONTEUDO,
                                        detalhe.CAMPOS[9].CONTEUDO,
                                        detalhe.CAMPOS[10].CONTEUDO,
                                        detalhe.CAMPOS[11].CONTEUDO,
                                        detalhe.CAMPOS[12].CONTEUDO,
                                        detalhe.CAMPOS[14].CONTEUDO);

                                cheque.VALOR = Convert.ToInt64(detalhe.CAMPOS[13].CONTEUDO);

                                cheque.INDICE = indiceCheque;

                                cheque.DADOS_VERSO = new List<DetalheArquivoCompe>();
                                cheque.DADOS_FRENTE = new List<DetalheArquivoCompe>();

                                valorTotalArquivo = valorTotalArquivo + cheque.VALOR;
                            }

                            if (detalhe.CAMPOS[38].CONTEUDO == "V")
                            {
                                detalhe.Foreground = new SolidColorBrush(Colors.DarkCyan);
                                cheque.DADOS_VERSO.Add(detalhe);
                                detalhe.VERSO = true;
                            }
                            else
                            {
                                detalhe.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
                                cheque.DADOS_FRENTE.Add(detalhe);
                                detalhe.VERSO = false;
                            }

                            byte totalRegistrosImagem = Convert.ToByte(detalhe.CAMPOS[34].CONTEUDO);

                            if (totalRegistrosImagem > 0)
                            {
                                byte sequencialImagem = Convert.ToByte(detalhe.CAMPOS[35].CONTEUDO);
                                long tamanhoImagem = Convert.ToInt64(detalhe.CAMPOS[36].CONTEUDO);
                                long tamanhoAssinatura = Convert.ToInt64(detalhe.CAMPOS[37].CONTEUDO);
                                long tamanhoTotal = tamanhoImagem + tamanhoAssinatura;
                                long tamanhoDetalheImAss = 0;

                                if (sequencialImagem == 1)
                                {
                                    binImagemAss = new byte[tamanhoTotal];
                                }

                                if (sequencialImagem == totalRegistrosImagem)
                                {
                                    tamanhoDetalheImAss = tamanhoTotal;

                                    while (tamanhoDetalheImAss > 0)
                                    {
                                        tamanhoDetalheImAss = tamanhoDetalheImAss - restoLinha;
                                    }

                                    Array.Copy(b, quebra, binImagemAss, (sequencialImagem - 1) * restoLinha, restoLinha + tamanhoDetalheImAss);

                                    if (cheque.IMAGEM_FRENTE == null)
                                    {
                                        cheque.IMAGEM_FRENTE = new byte[tamanhoImagem];
                                        cheque.ASSINATURA_FRENTE = new byte[tamanhoAssinatura];
                                        cheque.TOTAL_REGISTROS_IMAGEM_FRENTE = totalRegistrosImagem;
                                        Array.Copy(binImagemAss, 0, cheque.IMAGEM_FRENTE, 0, tamanhoImagem);
                                        Array.Copy(binImagemAss, tamanhoImagem, cheque.ASSINATURA_FRENTE, 0, tamanhoAssinatura);
                                    }
                                    else
                                    {
                                        cheque.IMAGEM_VERSO = new byte[tamanhoImagem];
                                        cheque.ASSINATURA_VERSO = new byte[tamanhoAssinatura];
                                        cheque.TOTAL_REGISTROS_IMAGEM_VERSO = totalRegistrosImagem;
                                        Array.Copy(binImagemAss, 0, cheque.IMAGEM_VERSO, 0, tamanhoImagem);
                                        Array.Copy(binImagemAss, tamanhoImagem, cheque.ASSINATURA_VERSO, 0, tamanhoAssinatura);

                                        _registrosDAD604.CHEQUES.Add(cheque);
                                        cheque = null;
                                    }
                                }
                                else
                                    Array.Copy(b, quebra, binImagemAss, (sequencialImagem - 1) * restoLinha, restoLinha);
                            }
                            else
                            {
                                _registrosDAD604.CHEQUES.Add(cheque);
                                cheque = null;
                            }

                        detalhe.INDICE_CHEQUE = indiceCheque;
                            break;
                    }
                }
                catch(Exception erro)
                {
                    MessageBox.Show(erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }
            }

            return retorno;
        }

        private void PreencheListaSaidaArquivoDAD604()
        {
            ltbSaida.Items.Clear();

            int quantidadeCheques = 0;

            ltbSaida.Items.Add(_registrosDAD604.HEADER);

            foreach (Cheque cheque in _registrosDAD604.CHEQUES)
            {
                foreach (DetalheArquivoCompe d in cheque.DADOS_FRENTE)
                {
                    ltbSaida.Items.Add(d);
                }

                if (cheque.DADOS_VERSO != null)
                {
                    foreach (DetalheArquivoCompe d in cheque.DADOS_VERSO)
                    {
                        ltbSaida.Items.Add(d);
                    }
                }

                quantidadeCheques++;
            }

            ltbSaida.Items.Add(_registrosDAD604.TRAILER);

            if (_registrosDAD604.HEADER != null)
            {
                txbTipoArq.Text = _registrosDAD604.HEADER.CAMPOS.Find(x => x.NUMERO == 4).CONTEUDO;
                txbVerArq.Text = _registrosDAD604.HEADER.CAMPOS.Find(x => x.NUMERO == 6).CONTEUDO;
                string datamov = _registrosDAD604.HEADER.CAMPOS.Find(x => x.NUMERO == 3).CONTEUDO;
                txbDtAbertura.Text = new StringBuilder(datamov.Substring(0, 4)).Append("-").Append(datamov.Substring(4, 2)).Append("-").Append(datamov.Substring(6, 2)).ToString();
            }

            txbQtdCheques.Text = quantidadeCheques.ToString();
            txbQtdLotes.Text = "0";

            if (_registrosDAD604.TRAILER != null)
            {
                string valorTotal = _registrosDAD604.TRAILER.VALOR_TOTAL.ToString();
                txbValorTotal.Text = (Convert.ToDecimal(valorTotal) / 100).ToString("N2");
            }
        }

        private bool AbreArquivoDAD606(string caminho)
        {
            bool retorno = true;

            ltbSaida.Items.Clear();

            _tipoArquivoAberto = 5;
            //_arquivoEBCDIC = true;

             byte[] binImagemAss = null;
            Cheque cheque = null;
            int indiceCheque = 0;

            _registrosDAD606 = new ArquivoDAD606();
            _registrosDAD606.CHEQUES = new List<Cheque>();

            _registrosDAD606.TIPO_ARQUIVO = 5;

            int linha = 0;
            int quebra = _registrosDAD606.QUEBRA;
            int comprimentoTotalLinha = _registrosDAD606.COMPRIMENTO_LINHA;
            int restoLinha = comprimentoTotalLinha - quebra;

            long valorTotalArquivo = 0;

            byte[] arquivo = Util.LeBinario(caminho);
            byte[] header = new byte[213];
            byte[] detalhes = new byte[arquivo.Length - comprimentoTotalLinha - comprimentoTotalLinha];
            byte[] trailer = new byte[213];

            Array.Copy(arquivo, 0, header, 0, 213);
            Array.Copy(arquivo, comprimentoTotalLinha + 39, detalhes, 0, arquivo.Length - comprimentoTotalLinha - comprimentoTotalLinha);
            Array.Copy(arquivo, arquivo.Length - comprimentoTotalLinha - 39, trailer, 0, 213);

            List<byte[]> linhas = new List<byte[]>();

            linhas.Add(header);

            int totalDetalhes = detalhes.Length / comprimentoTotalLinha;

            for (int i = 0; i < totalDetalhes; i++)
            {
                byte[] binDados = new byte[quebra];
                Array.Copy(detalhes, (i * (comprimentoTotalLinha + 39)) , binDados, 0, quebra);
                linhas.Add(binDados);
            }

            linhas.Add(trailer);

            foreach (byte[] b in linhas)
            {
                try
                {
                    linha++;

                    string linhaASCII = Encoding.UTF8.GetString(Tabela.ConverteEBCDICParaASCII(b));

                    switch (linhaASCII.Substring(0, 1))
                    {
                        case "0":
                            _registrosDAD606.HEADER = new HeaderDAD606(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkBlue));
                            _registrosDAD606.HEADER.INDICE_CHEQUE = 0;
                            break;
                        case "9":
                            _registrosDAD606.TRAILER = new TrailerDAD606(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkRed));
                            _registrosDAD606.TRAILER.INDICE_CHEQUE = 0;
                            _registrosDAD606.TRAILER.VALOR_TOTAL = valorTotalArquivo;
                            break;
                        case "1":
                            DetalheDAD606 detalhe = new DetalheDAD606(linhaASCII, linha, quebra, comprimentoTotalLinha);

                            if (cheque == null)
                            {
                                indiceCheque++;

                                cheque = new Cheque();

                                cheque.VALOR = Convert.ToInt64(detalhe.CAMPOS[11].CONTEUDO);

                                cheque.INDICE = indiceCheque;

                                cheque.DADOS_VERSO = new List<DetalheArquivoCompe>();
                                cheque.DADOS_FRENTE = new List<DetalheArquivoCompe>();

                                valorTotalArquivo = valorTotalArquivo + cheque.VALOR;
                            }

                            if (detalhe.CAMPOS[22].CONTEUDO == "V")
                            {
                                detalhe.Foreground = new SolidColorBrush(Colors.DarkCyan);
                                cheque.DADOS_VERSO.Add(detalhe);
                                detalhe.VERSO = true;
                            }
                            else
                            {
                                detalhe.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
                                cheque.DADOS_FRENTE.Add(detalhe);
                                detalhe.VERSO = false;
                            }

                            byte totalRegistrosImagem = Convert.ToByte(detalhe.CAMPOS[18].CONTEUDO);

                            if (totalRegistrosImagem > 0)
                            {
                                byte sequencialImagem = Convert.ToByte(detalhe.CAMPOS[19].CONTEUDO);
                                long tamanhoImagem = Convert.ToInt64(detalhe.CAMPOS[20].CONTEUDO);
                                long tamanhoAssinatura = Convert.ToInt64(detalhe.CAMPOS[21].CONTEUDO);
                                long tamanhoTotal = tamanhoImagem + tamanhoAssinatura;
                                long tamanhoDetalheImAss = 0;

                                if (sequencialImagem == 1)
                                {
                                    binImagemAss = new byte[tamanhoTotal];
                                }

                                if (sequencialImagem == totalRegistrosImagem)
                                {
                                    tamanhoDetalheImAss = tamanhoTotal;

                                    while (tamanhoDetalheImAss > 0)
                                    {
                                        tamanhoDetalheImAss = tamanhoDetalheImAss - restoLinha;
                                    }

                                    Array.Copy(b, quebra, binImagemAss, (sequencialImagem - 1) * restoLinha, restoLinha + tamanhoDetalheImAss);

                                    if (cheque.IMAGEM_FRENTE == null)
                                    {
                                        cheque.IMAGEM_FRENTE = new byte[tamanhoImagem];
                                        cheque.ASSINATURA_FRENTE = new byte[tamanhoAssinatura];
                                        cheque.TOTAL_REGISTROS_IMAGEM_FRENTE = totalRegistrosImagem;
                                        Array.Copy(binImagemAss, 0, cheque.IMAGEM_FRENTE, 0, tamanhoImagem);
                                        Array.Copy(binImagemAss, tamanhoImagem, cheque.ASSINATURA_FRENTE, 0, tamanhoAssinatura);
                                    }
                                    else
                                    {
                                        cheque.IMAGEM_VERSO = new byte[tamanhoImagem];
                                        cheque.ASSINATURA_VERSO = new byte[tamanhoAssinatura];
                                        cheque.TOTAL_REGISTROS_IMAGEM_VERSO = totalRegistrosImagem;
                                        Array.Copy(binImagemAss, 0, cheque.IMAGEM_VERSO, 0, tamanhoImagem);
                                        Array.Copy(binImagemAss, tamanhoImagem, cheque.ASSINATURA_VERSO, 0, tamanhoAssinatura);

                                        _registrosDAD604.CHEQUES.Add(cheque);
                                        cheque = null;
                                    }
                                }
                                else
                                    Array.Copy(b, quebra, binImagemAss, (sequencialImagem - 1) * restoLinha, restoLinha);
                            }
                            else
                            {
                                _registrosDAD606.CHEQUES.Add(cheque);
                                cheque = null;
                            }

                        detalhe.INDICE_CHEQUE = indiceCheque;

                            break;
                    }
                }
                catch(Exception erro)
                {
                    MessageBox.Show(erro.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }
            }

            return retorno;
        }

        private void PreencheListaSaidaArquivoDAD606()
        {
            ltbSaida.Items.Clear();

            int quantidadeCheques = 0;

            ltbSaida.Items.Add(_registrosDAD606.HEADER);

            foreach (Cheque cheque in _registrosDAD606.CHEQUES)
            {
                foreach (DetalheArquivoCompe d in cheque.DADOS_FRENTE)
                {
                    ltbSaida.Items.Add(d);
                }

                if (cheque.DADOS_VERSO != null)
                {
                    foreach (DetalheArquivoCompe d in cheque.DADOS_VERSO)
                    {
                        ltbSaida.Items.Add(d);
                    }
                }

                quantidadeCheques++;
            }

            ltbSaida.Items.Add(_registrosDAD606.TRAILER);

            if (_registrosDAD606.HEADER != null)
            {
                txbTipoArq.Text = _registrosDAD606.HEADER.CAMPOS.Find(x => x.NUMERO == 4).CONTEUDO;
                txbVerArq.Text = _registrosDAD606.HEADER.CAMPOS.Find(x => x.NUMERO == 6).CONTEUDO;
                string datamov = _registrosDAD606.HEADER.CAMPOS.Find(x => x.NUMERO == 3).CONTEUDO;
                txbDtAbertura.Text = new StringBuilder(datamov.Substring(0, 4)).Append("-").Append(datamov.Substring(4, 2)).Append("-").Append(datamov.Substring(6, 2)).ToString();
            }

            txbQtdCheques.Text = quantidadeCheques.ToString();
            txbQtdLotes.Text = "0";

            if (_registrosDAD606.TRAILER != null)
            {
                string valorTotal = _registrosDAD606.TRAILER.VALOR_TOTAL.ToString();
                txbValorTotal.Text = (Convert.ToDecimal(valorTotal) / 100).ToString("N2");
            }
        }

        private void ConfiguraTela(bool sucesso, string nomeArquivo)
        {
            if (sucesso)
            {
                this.Title = (nomeArquivo);
                _caminhoarquivoaberto = nomeArquivo;

                switch (_tipoArquivoAberto)
                {
                    case 6:
                        gpbCarregando.Visibility = System.Windows.Visibility.Hidden;
                        gpbDadosArq.Visibility = System.Windows.Visibility.Hidden;
                        gpbArqEBCDIC.Visibility = System.Windows.Visibility.Visible;
                        MenuPesquisar.Visibility = System.Windows.Visibility.Visible;
                        MenuItemCopiar.IsEnabled = true;
                        MenuItemCopiarSelecao.IsEnabled = true;
                        //MenuItemDuplicarLinha.IsEnabled = true;
                        MenuItemExtrair.IsEnabled = true;
                        MenuItemRegerar.IsEnabled = true;
                        MenuItemEditarArquivo.IsEnabled = true;

                        btnDadosLotes.Visibility = System.Windows.Visibility.Collapsed;
                        btnValorLote.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    default:
                        gpbCarregando.Visibility = System.Windows.Visibility.Hidden;
                        gpbDadosArq.Visibility = System.Windows.Visibility.Visible;
                        gpbArqEBCDIC.Visibility = System.Windows.Visibility.Hidden;
                        MenuPesquisar.Visibility = System.Windows.Visibility.Visible;
                        MenuItemCopiar.IsEnabled = true;
                        MenuItemCopiarSelecao.IsEnabled = true;
                        //MenuItemDuplicarLinha.IsEnabled = true;
                        MenuItemExtrair.IsEnabled = true;
                        MenuItemRegerar.IsEnabled = true;
                        MenuItemEditarArquivo.IsEnabled = true;

                        btnDadosLotes.Visibility = System.Windows.Visibility.Collapsed;
                        btnValorLote.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                }
            }
            else
            {
                MessageBox.Show("Erro ao Abrir Arquivo - " + System.IO.Path.GetFileName(nomeArquivo), "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Title = System.IO.Path.GetFileName(nomeArquivo);
                gpbCarregando.Visibility = System.Windows.Visibility.Hidden;
                gpbDadosArq.Visibility = System.Windows.Visibility.Hidden;
                MenuPesquisar.Visibility = System.Windows.Visibility.Hidden;
                MenuItemCopiar.IsEnabled = false;
                MenuItemCopiarSelecao.IsEnabled = false;
                //MenuItemDuplicarLinha.IsEnabled = false;
                MenuItemExtrair.IsEnabled = false;
                MenuItemRegerar.IsEnabled = false;
            }
        }

        private void ZeraRegistros()
        {
            if (_registros604 != null)
            {
                _registros604.HEADER = null;
                _registros604.LOTES = null;
                _registros604.TRAILER = null;
                _registros604 = null;
            }

            if (_registros605 != null)
            {
                _registros605.HEADER = null;
                _registros605.LOTES = null;
                _registros605.TRAILER = null;
                _registros605 = null;
            }

            if (_registrosDAD604 != null)
            {
                _registrosDAD604.HEADER = null;
                _registrosDAD604.CHEQUES = null;
                _registrosDAD604.TRAILER = null;
                _registrosDAD604 = null;
            }

            if (_registrosDAD606 != null)
            {
                _registrosDAD606.HEADER = null;
                _registrosDAD606.CHEQUES = null;
                _registrosDAD606.TRAILER = null;
                _registrosDAD606 = null;
            }

            if (_registro674 != null)
            {
                _registro674.DETALHES_A_RECEBER_RECUSADOS = null;
                _registro674.DETALHES_REMETIDOS_RECUSADOS = null;
                _registro674.HEADER = null;
                _registro674.LOTES_RECEBIDOS = null;
                _registro674.LOTES_RECEBIDOS_COM_OCORRENCIA = null;
                _registro674.LOTES_REMETIDOS = null;
                _registro674.LOTES_REMETIDOS_COM_OCORRECIA = null;
                _registro674.OCORRECIAS_HEADER_TRAILER = null;
                _registro674.VERSOES_PROCESSADAS = null;
                _registro674.TRAILER = null;
                _registro674 = null;
            }

            if (_registrosEBCDIC != null)
            {
                _registrosEBCDIC.LINHAS = null;
                _registrosEBCDIC = null;
            }
        }

        private void RegerarArquivoCEL604()
        {
            List<byte[]> binArquivoSaida = new List<byte[]>();
            byte[] binImagemAssinatura = null;
            int restoLinha = 0;

            binArquivoSaida.Add(Util.GeraBinarioLinhaEBCDIC(_registros604.HEADER));

            foreach (LoteCEL604 lote in _registros604.LOTES)
            {
                foreach (Cheque cheque in lote.CHEQUES)
                {
                    byte[] binDetalhe = null; 
                    int j = 0;
                    int tamanhoTotal = 0;

                    if (cheque.IMAGEM_FRENTE != null)
                    {
                        tamanhoTotal = cheque.IMAGEM_FRENTE.Length + cheque.ASSINATURA_FRENTE.Length;
                        binImagemAssinatura = new byte[tamanhoTotal];

                        Array.Copy(cheque.IMAGEM_FRENTE, 0, binImagemAssinatura, 0, cheque.IMAGEM_FRENTE.Length);
                        Array.Copy(cheque.ASSINATURA_FRENTE, 0, binImagemAssinatura, cheque.IMAGEM_FRENTE.Length, cheque.ASSINATURA_FRENTE.Length);

                        restoLinha = cheque.DADOS_FRENTE[0].TAMANHO_TOTAL_LINHA - cheque.DADOS_FRENTE[0].QUEBRA;
                    }

                    foreach(DetalheArquivoCompe d in cheque.DADOS_FRENTE)
                    {
                        binDetalhe = Util.GeraBinarioLinhaEBCDIC(d);

                        tamanhoTotal = tamanhoTotal - restoLinha;

                        if (binImagemAssinatura != null)
                        {
                            if(j+1 == cheque.TOTAL_REGISTROS_IMAGEM_FRENTE)
                                Array.Copy(binImagemAssinatura, j * restoLinha, binDetalhe, d.QUEBRA, restoLinha + tamanhoTotal);   
                            else
                                Array.Copy(binImagemAssinatura, j * restoLinha, binDetalhe, d.QUEBRA, restoLinha);

                            j++;
                        }
                        binArquivoSaida.Add(binDetalhe);
                    }

                    if (cheque.IMAGEM_VERSO != null)
                    {
                        binDetalhe = null;
                        j = 0;
                        tamanhoTotal = cheque.IMAGEM_VERSO.Length + cheque.ASSINATURA_VERSO.Length;
                        binImagemAssinatura = new byte[tamanhoTotal];

                        Array.Copy(cheque.IMAGEM_VERSO, 0, binImagemAssinatura, 0, cheque.IMAGEM_VERSO.Length);
                        Array.Copy(cheque.ASSINATURA_VERSO, 0, binImagemAssinatura, cheque.IMAGEM_VERSO.Length, cheque.ASSINATURA_VERSO.Length);

                        restoLinha = cheque.DADOS_VERSO[0].TAMANHO_TOTAL_LINHA - cheque.DADOS_VERSO[0].QUEBRA;

                        foreach (DetalheArquivoCompe d in cheque.DADOS_VERSO)
                        {
                            binDetalhe = Util.GeraBinarioLinhaEBCDIC(d);

                            tamanhoTotal = tamanhoTotal - restoLinha;

                            if (j + 1 == cheque.TOTAL_REGISTROS_IMAGEM_VERSO)
                                Array.Copy(binImagemAssinatura, j * restoLinha, binDetalhe, d.QUEBRA, restoLinha + tamanhoTotal);
                            else
                                Array.Copy(binImagemAssinatura, j * restoLinha, binDetalhe, d.QUEBRA, restoLinha);

                            j++;
                            binArquivoSaida.Add(binDetalhe);
                        }
                    }
                }

                binArquivoSaida.Add(Util.GeraBinarioLinhaEBCDIC(lote.FECHAMENTO));
            }

            binArquivoSaida.Add(Util.GeraBinarioLinhaEBCDIC(_registros604.TRAILER));

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = System.IO.Path.GetDirectoryName(_caminhoarquivoaberto);
            sfd.FileName = System.IO.Path.GetFileName(_caminhoarquivoaberto);

            if (sfd.ShowDialog() == true)
            {
                FileStream objFileStream = null;

                objFileStream = new FileStream(sfd.FileName, FileMode.Create);

                foreach (byte[] b in binArquivoSaida)
                {
                    objFileStream.Write(b, 0, b.Length);
                }

                objFileStream.Close();
                objFileStream.Dispose();
            }
        }

        private void MenuItemRegerar_Click(object sender, RoutedEventArgs e)
        {
            RegerarArquivoCEL604();
        }

        private void MenuItemEditarArquivo_Click(object sender, RoutedEventArgs e)
        {
            EditarArquivoCELNRA604 editar604 = new EditarArquivoCELNRA604(_registros604);

            editar604.ShowDialog();

            PreencheListaSaidaArquivoCEL604();
            
        }

        private void MenuItemNovo_Click(object sender, RoutedEventArgs e)
        {
            NovoArquivoCEL604();
        }

        private void NovoArquivoCEL604()
        {
            _registros604 = new ArquivoCELNRA604();

            _registros604.LOTES = new List<LoteCEL604>();

            int linha = 0;
            int quebra = _registros604.QUEBRA;
            int comprimentoTotalLinha = _registros604.COMPRIMENTO_LINHA;

            _registros604.HEADER = new HeaderCEL604(new string(' ', quebra), linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkBlue));
            _registros604.HEADER.INDICE_CHEQUE = 0;
            _registros604.HEADER.INDICE_LOTE = 0;

            _registros604.NOME_ARQUIVO = "CEL604";
            _registros604.VERSAO_ARQUIVO = "1";
            _registros604.INDICADOR_REMESSA = "1";
            _registros604.DATA_MOVIMENTO = DateTime.Now.ToString("yyyyMMdd");

            _registros604.HEADER.CAMPOS[0].CONTEUDO = new string('0',47);
            _registros604.HEADER.CAMPOS[1].CONTEUDO = _registros604.NOME_ARQUIVO;
            _registros604.HEADER.CAMPOS[2].CONTEUDO = _registros604.VERSAO_ARQUIVO;
            _registros604.HEADER.CAMPOS[5].CONTEUDO = _registros604.INDICADOR_REMESSA;
            _registros604.HEADER.CAMPOS[6].CONTEUDO = _registros604.DATA_MOVIMENTO;
            _registros604.HEADER.CAMPOS[8].CONTEUDO = "1";

            _registros604.TRAILER = new TrailerCEL604(new string(' ', quebra), linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.DarkRed));
            _registros604.TRAILER.INDICE_CHEQUE = 0;
            _registros604.TRAILER.INDICE_LOTE = 0;

            _registros604.TRAILER.CAMPOS[0].CONTEUDO = new string('9',47);
            _registros604.TRAILER.CAMPOS[1].CONTEUDO = _registros604.NOME_ARQUIVO;
            _registros604.TRAILER.CAMPOS[2].CONTEUDO = _registros604.VERSAO_ARQUIVO;
            _registros604.TRAILER.CAMPOS[5].CONTEUDO = _registros604.INDICADOR_REMESSA;
            _registros604.TRAILER.CAMPOS[6].CONTEUDO = _registros604.DATA_MOVIMENTO;
            _registros604.TRAILER.CAMPOS[9].CONTEUDO = "2";

            _registros604.HEADER.RegeraLinha();
            _registros604.TRAILER.RegeraLinha();

            ConfiguraTela(true, _registros604.NOME_ARQUIVO);
            PreencheListaSaidaArquivoCEL604();
        }

        private void ltbSaida_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MostraDetalhesLinha();
        }

        private void MenuItemAbrirEBCDIC_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                ZeraRegistros();
                if (AbreArquivoEBCDIC(ofd.FileName))
                {
                    ConfiguraTela(true, ofd.FileName);
                    PreencheListaSaidaArquivoEBCDIC();
                }
                else
                {
                    ConfiguraTela(false, ofd.FileName);
                }
            }
        }

        private bool AbreArquivoEBCDIC(string caminho)
        {
            bool retorno = true;

            _tipoArquivoAberto = 6;
            //_arquivoEBCDIC = true;

            _registrosEBCDIC = new ArquivoEBCDIC();
            _registrosEBCDIC.LINHAS = new List<LinhaEBCDIC>();

            _registrosEBCDIC.TIPO_ARQUIVO = 6;

            int linha = 0;

            int quebra = Convert.ToInt32(txbQbr.Text);
            int ini = Convert.ToInt32(txbIni.Text);
            int fim = Convert.ToInt32(txbFim.Text);
            int comprimentoTotalLinha = fim - ini;

            List<byte[]> linhas = Util.MontaListaBinariosArquivo(new FileInfo(caminho), ini, fim);

            string linhaASCII = string.Empty;

            foreach (byte[] b in linhas)
            {
                try
                {
                    linha++;
                    byte[] binDados = new byte[quebra];
                    Array.Copy(b, 0, binDados, 0, quebra);

                    linhaASCII = Encoding.UTF8.GetString(Tabela.ConverteEBCDICParaASCII(binDados));

                    LinhaEBCDIC linhaEBCDIC = new LinhaEBCDIC(linhaASCII, linha, quebra, comprimentoTotalLinha, new SolidColorBrush(Colors.Black));

                    _registrosEBCDIC.LINHAS.Add(linhaEBCDIC);
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message + " Linha: " + linha.ToString(), "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    continue;
                }
            }

            return retorno;
        }

        private void PreencheListaSaidaArquivoEBCDIC()
        {
            ltbSaida.Items.Clear();

            foreach (LinhaEBCDIC linha in _registrosEBCDIC.LINHAS)
            {
                ltbSaida.Items.Add(linha);
            }

            ltbSaida.Items.SortDescriptions.Add(new SortDescription("NUMERO_LINHA", ListSortDirection.Ascending));
        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            AbreArquivoEBCDIC(_caminhoarquivoaberto);
            ConfiguraTela(true, _caminhoarquivoaberto);
            PreencheListaSaidaArquivoEBCDIC();
        }
    }
}
