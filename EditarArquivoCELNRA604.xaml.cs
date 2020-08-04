using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Interop;

namespace Visualizador604
{
    /// <summary>
    /// Interaction logic for EditarArquivoCELNRA604.xaml
    /// </summary>
    public partial class EditarArquivoCELNRA604 : Window
    {
        ArquivoCELNRA604 _arquivo;

        public EditarArquivoCELNRA604(ArquivoCELNRA604 arquivo)
        {
            InitializeComponent();

            _arquivo = arquivo;

            txb_DataMovimento.Text = _arquivo.DATA_MOVIMENTO;
            txb_IndicadorRemessa.Text = _arquivo.INDICADOR_REMESSA;
            txb_NomeArquivo.Text = _arquivo.NOME_ARQUIVO;
            txb_VersaoArquivo.Text = _arquivo.VERSAO_ARQUIVO;

            btn_RemoverCheque.IsEnabled = false;
            btn_RemoverLote.IsEnabled = false;
            btn_EditarDadosLote.IsEnabled = false;
            btn_EditarCheque.IsEnabled = false;

            PreencheListaLotes();
        }

        private Bitmap BitmapFromByteArray(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            Bitmap image = new Bitmap(stream);
            stream.Close();
            stream.Dispose();
            return image;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void ltb_Lotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ltb_Lotes.SelectedIndex != -1)
            {
                PreencheListaCheques();
                btn_RemoverLote.IsEnabled = true;
                btn_EditarDadosLote.IsEnabled = true;
            }
            else
            {
                btn_RemoverLote.IsEnabled = false;
                btn_EditarDadosLote.IsEnabled = false;
            }
        }

        private void PreencheListaLotes()
        {
            ltb_Lotes.Items.Clear();

            foreach (LoteCEL604 lote in _arquivo.LOTES)
            {
                if (lote.INDICE > 0)
                {
                    ltb_Lotes.Items.Add("Lote " + lote.NUMERO.ToString());
                }
            }

            if(ltb_Lotes.Items.Count > 0)
                ltb_Lotes.SelectedIndex = 0;
        }

        private void PreencheListaCheques()
        {
            ltb_Cheques.Items.Clear();

            if (ltb_Lotes.SelectedIndex != -1)
            {
                LoteCEL604 lote = _arquivo.LOTES.Find(x => x.INDICE == ltb_Lotes.SelectedIndex + 1);

                foreach (Cheque cheque in lote.CHEQUES)
                {
                    if (cheque.INDICE > 0)
                    {
                        TextBlock item = new TextBlock();
                        item.Text = "Cheque " + cheque.NUMERO.ToString();
                        item.Tag = cheque.INDICE;
                        ltb_Cheques.Items.Add(item);
                    }
                }
            }

            if (ltb_Cheques.Items.Count > 0)
                ltb_Cheques.SelectedIndex = 0;
        }

        private void btn_Aplicar_Click(object sender, RoutedEventArgs e)
        {
            _arquivo.DATA_MOVIMENTO = txb_DataMovimento.Text.PadLeft(8, '0');
            _arquivo.NOME_ARQUIVO = txb_NomeArquivo.Text.PadRight(6, ' ');
            _arquivo.INDICADOR_REMESSA = txb_IndicadorRemessa.Text;
            _arquivo.VERSAO_ARQUIVO = txb_VersaoArquivo.Text.PadLeft(7, '0');

            _arquivo.HEADER.CAMPOS[1].CONTEUDO = _arquivo.NOME_ARQUIVO;
            _arquivo.HEADER.CAMPOS[2].CONTEUDO = _arquivo.VERSAO_ARQUIVO;
            _arquivo.HEADER.CAMPOS[5].CONTEUDO = _arquivo.INDICADOR_REMESSA;
            _arquivo.HEADER.CAMPOS[6].CONTEUDO = _arquivo.DATA_MOVIMENTO;
            _arquivo.HEADER.NUMERO_LINHA = 1;
            _arquivo.HEADER.RegeraLinha();

            int sequencialArquivo = 2;

            long valorLote = 0;
            long valorArquivo = 0;
            byte sequencialImagem = 0;

            List<Cheque> chequesRemover = new List<Cheque>();
            List<LoteCEL604> lotesRemover = new List<LoteCEL604>();

            foreach (LoteCEL604 lote in _arquivo.LOTES)
            {
                if (lote.INDICE > 0)
                {
                    foreach (Cheque cheque in lote.CHEQUES)
                    {
                        if (cheque.INDICE > 0)
                        {
                            foreach (DetalheArquivoCompe d in cheque.DADOS_FRENTE)
                            {
                                cheque.VALOR = Convert.ToInt64(d.CAMPOS[9].CONTEUDO);

                                d.CAMPOS[18].CONTEUDO = _arquivo.DATA_MOVIMENTO;
                                d.CAMPOS[24].CONTEUDO = _arquivo.VERSAO_ARQUIVO;
                                d.CAMPOS[26].CONTEUDO = sequencialArquivo.ToString().PadLeft(10, '0');
                                d.NUMERO_LINHA = sequencialArquivo;
                                sequencialArquivo++;

                                if (cheque.IMAGEM_FRENTE != null)
                                {
                                    sequencialImagem++;
                                    d.CAMPOS[28].CONTEUDO = cheque.TOTAL_REGISTROS_IMAGEM_FRENTE.ToString().PadLeft(2, '0');
                                    d.CAMPOS[30].CONTEUDO = cheque.IMAGEM_FRENTE.Length.ToString().PadLeft(9, '0');
                                    d.CAMPOS[31].CONTEUDO = cheque.ASSINATURA_FRENTE.Length.ToString().PadLeft(9, '0');
                                    d.CAMPOS[32].CONTEUDO = "F";
                                }
                                else
                                {
                                    d.CAMPOS[28].CONTEUDO = "00";
                                    d.CAMPOS[30].CONTEUDO = "000000000";
                                    d.CAMPOS[31].CONTEUDO = "000000000";
                                    d.CAMPOS[32].CONTEUDO = " ";
                                }

                                d.CAMPOS[29].CONTEUDO = sequencialImagem.ToString().PadLeft(2, '0');

                                if (cheque.TOTAL_REGISTROS_IMAGEM_FRENTE == sequencialImagem)
                                {
                                    valorLote = valorLote + cheque.VALOR;
                                }

                                d.Foreground = new SolidColorBrush(Colors.DarkOliveGreen);
                                d.TAMANHO_TOTAL_LINHA = _arquivo.COMPRIMENTO_LINHA;
                                d.QUEBRA = _arquivo.QUEBRA;
                                d.INDICE_CHEQUE = cheque.INDICE;
                                d.INDICE_LOTE = lote.INDICE;
                            }

                            sequencialImagem = 0;

                            if (cheque.DADOS_VERSO != null)
                            {
                                foreach (DetalheArquivoCompe d in cheque.DADOS_VERSO)
                                {
                                    d.CAMPOS[18].CONTEUDO = _arquivo.DATA_MOVIMENTO;
                                    d.CAMPOS[24].CONTEUDO = _arquivo.VERSAO_ARQUIVO;
                                    d.CAMPOS[26].CONTEUDO = sequencialArquivo.ToString().PadLeft(10, '0');
                                    d.NUMERO_LINHA = sequencialArquivo;

                                    sequencialArquivo++;

                                    if (cheque.IMAGEM_VERSO != null)
                                    {
                                        sequencialImagem++;
                                        d.CAMPOS[28].CONTEUDO = cheque.TOTAL_REGISTROS_IMAGEM_VERSO.ToString().PadLeft(2, '0');
                                        d.CAMPOS[30].CONTEUDO = cheque.IMAGEM_VERSO.Length.ToString().PadLeft(9, '0');
                                        d.CAMPOS[31].CONTEUDO = cheque.ASSINATURA_VERSO.Length.ToString().PadLeft(9, '0');
                                        d.CAMPOS[32].CONTEUDO = "V";
                                    }
                                    else
                                    {
                                        d.CAMPOS[28].CONTEUDO = "00";
                                        d.CAMPOS[30].CONTEUDO = "000000000";
                                        d.CAMPOS[31].CONTEUDO = "000000000";
                                        d.CAMPOS[32].CONTEUDO = " ";
                                    }

                                    d.CAMPOS[29].CONTEUDO = sequencialImagem.ToString().PadLeft(2, '0');

                                    d.Foreground = new SolidColorBrush(Colors.DarkCyan);
                                    d.TAMANHO_TOTAL_LINHA = _arquivo.COMPRIMENTO_LINHA;
                                    d.QUEBRA = _arquivo.QUEBRA;
                                    d.INDICE_CHEQUE = cheque.INDICE;
                                    d.INDICE_LOTE = lote.INDICE;
                                }

                                sequencialImagem = 0;
                            }

                            cheque.RegeraLinhasFrenteVerso();

                        }
                        else
                        {
                            chequesRemover.Add(cheque);
                        }
                    }

                    foreach (Cheque cheque in chequesRemover)
                    {
                        lote.CHEQUES.Remove(cheque);
                    }

                    lote.FECHAMENTO.CAMPOS[2].CONTEUDO = new string('9', 27);
                    lote.FECHAMENTO.CAMPOS[7].CONTEUDO = _arquivo.DATA_MOVIMENTO;
                    lote.FECHAMENTO.CAMPOS[13].CONTEUDO = _arquivo.VERSAO_ARQUIVO;
                    lote.FECHAMENTO.CAMPOS[15].CONTEUDO = sequencialArquivo.ToString().PadLeft(10, '0');
                    lote.FECHAMENTO.CAMPOS[12].CONTEUDO = lote.FECHAMENTO.CAMPOS[15].CONTEUDO;
                    lote.FECHAMENTO.CAMPOS[3].CONTEUDO = valorLote.ToString().PadLeft(17,'0');
                    lote.FECHAMENTO.NUMERO_LINHA = sequencialArquivo;

                    foreach (Cheque cheque in lote.CHEQUES)
                    {
                        foreach (DetalheArquivoCompe d in cheque.DADOS_FRENTE)
                        {
                            d.CAMPOS[0].CONTEUDO = lote.FECHAMENTO.CAMPOS[0].CONTEUDO;
                            d.CAMPOS[1].CONTEUDO = lote.FECHAMENTO.CAMPOS[1].CONTEUDO;
                            d.CAMPOS[19].CONTEUDO = lote.FECHAMENTO.CAMPOS[8].CONTEUDO;
                            d.CAMPOS[25].CONTEUDO = lote.FECHAMENTO.CAMPOS[14].CONTEUDO;
                            d.CAMPOS[20].CONTEUDO = lote.FECHAMENTO.CAMPOS[9].CONTEUDO;
                        }

                        if (cheque.DADOS_VERSO != null)
                        {
                            foreach (DetalheArquivoCompe d in cheque.DADOS_VERSO)
                            {
                                d.CAMPOS[0].CONTEUDO = lote.FECHAMENTO.CAMPOS[0].CONTEUDO;
                                d.CAMPOS[1].CONTEUDO = lote.FECHAMENTO.CAMPOS[1].CONTEUDO;
                                d.CAMPOS[19].CONTEUDO = lote.FECHAMENTO.CAMPOS[8].CONTEUDO;
                                d.CAMPOS[25].CONTEUDO = lote.FECHAMENTO.CAMPOS[14].CONTEUDO;
                                d.CAMPOS[20].CONTEUDO = lote.FECHAMENTO.CAMPOS[9].CONTEUDO;
                            }
                        }

                        cheque.RegeraLinhasFrenteVerso();
                    }

                    valorArquivo = valorArquivo + valorLote;
                    valorLote = 0;

                    sequencialArquivo++;
                    lote.FECHAMENTO.RegeraLinha();
                }
                else
                {
                    lotesRemover.Add(lote);
                }
            }

            foreach (LoteCEL604 lote in lotesRemover)
            {
                _arquivo.LOTES.Remove(lote);
            }

            _arquivo.TRAILER.CAMPOS[1].CONTEUDO = _arquivo.NOME_ARQUIVO;
            _arquivo.TRAILER.CAMPOS[2].CONTEUDO = _arquivo.VERSAO_ARQUIVO;
            _arquivo.TRAILER.CAMPOS[5].CONTEUDO = _arquivo.INDICADOR_REMESSA;
            _arquivo.TRAILER.CAMPOS[6].CONTEUDO = _arquivo.DATA_MOVIMENTO;
            _arquivo.TRAILER.CAMPOS[9].CONTEUDO = sequencialArquivo.ToString().PadLeft(10, '0');
            _arquivo.TRAILER.CAMPOS[7].CONTEUDO = valorArquivo.ToString().PadLeft(17, '0');
            _arquivo.TRAILER.NUMERO_LINHA = sequencialArquivo;
            sequencialArquivo++;

            _arquivo.TRAILER.RegeraLinha();

            this.Close();
        }

        private void btn_RemoverCheque_Click(object sender, RoutedEventArgs e)
        {
            TextBlock itemSelecionado = (TextBlock)ltb_Cheques.SelectedItem;

            LoteCEL604 lote = _arquivo.LOTES.Find(x => x.INDICE == ltb_Lotes.SelectedIndex + 1);
            Cheque cheque = lote.CHEQUES.Find(x => x.INDICE == (int)itemSelecionado.Tag);
            cheque.INDICE = cheque.INDICE * -1;

            PreencheListaCheques();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (LoteCEL604 lote in _arquivo.LOTES)
            {
                foreach (Cheque cheque in lote.CHEQUES)
                {
                    if (cheque.INDICE < 0)
                        cheque.INDICE = cheque.INDICE * -1;
                }
            }
        }

        private void ltb_Cheques_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ltb_Cheques.SelectedIndex != -1)
            {
                btn_RemoverCheque.IsEnabled = true;
                btn_EditarCheque.IsEnabled = true;
            }
            else
            {
                btn_RemoverCheque.IsEnabled = false;
                btn_EditarCheque.IsEnabled = false;
            }
        }

        private void btn_RemoverLote_Click(object sender, RoutedEventArgs e)
        {
            LoteCEL604 lote = _arquivo.LOTES.Find(x => x.INDICE == ltb_Lotes.SelectedIndex + 1);
            lote.INDICE = lote.INDICE * -1;

            PreencheListaLotes();
        }

        private void btn_AdicionarLote_Click(object sender, RoutedEventArgs e)
        {
            LoteCEL604 lote = new LoteCEL604();
            lote.FECHAMENTO = new FechamentoCEL604(new string(' ', _arquivo.QUEBRA), 0, _arquivo.QUEBRA, _arquivo.COMPRIMENTO_LINHA, new SolidColorBrush(Colors.DarkMagenta));
            lote.INDICE = ltb_Lotes.Items.Count + 1;

            EditaDetalhe edita = new EditaDetalhe(lote.FECHAMENTO);

            if (edita.ShowDialog() == true)
            {
                lote.CHEQUES = new List<Cheque>();
                _arquivo.LOTES.Add(lote);
                PreencheListaLotes();
            }
            else
                lote = null;
        }

        private void btn_EditarDadosLote_Click(object sender, RoutedEventArgs e)
        {
            EditarLote();
        }

        private void EditarLote()
        {
            LoteCEL604 lote = _arquivo.LOTES.Find(x => x.INDICE == ltb_Lotes.SelectedIndex + 1);
            EditaDetalhe edita = new EditaDetalhe(lote.FECHAMENTO);
            edita.ShowDialog();
        }

        private void ltb_Lotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditarLote();
        }

        private void btn_AdicionarCheque_Click(object sender, RoutedEventArgs e)
        {
            DetalheCEL604 detalhe = new DetalheCEL604(new string(' ', _arquivo.QUEBRA), 0, _arquivo.QUEBRA, _arquivo.COMPRIMENTO_LINHA);

           Cheque cheque = new Cheque();
            cheque.DADOS_FRENTE = new List<DetalheArquivoCompe>();
            cheque.DADOS_FRENTE.Add(detalhe);
            cheque.INDICE = ltb_Cheques.Items.Count + 1;

            EditarCheque edita = new EditarCheque(cheque);

            if (edita.ShowDialog() == true)
            {
                LoteCEL604 lote = _arquivo.LOTES.Find(x => x.INDICE == ltb_Lotes.SelectedIndex + 1);
                lote.CHEQUES.Add(cheque);

                PreencheListaCheques();
            }
            else
                cheque = null;
        }

        private void btn_EditarCheque_Click(object sender, RoutedEventArgs e)
        {
            EditarCheque();
        }

        private void EditarCheque()
        {
            TextBlock itemSelecionado = (TextBlock)ltb_Cheques.SelectedItem;

            LoteCEL604 lote = _arquivo.LOTES.Find(x => x.INDICE == ltb_Lotes.SelectedIndex + 1);
            Cheque cheque = lote.CHEQUES.Find(x => x.INDICE == (int)itemSelecionado.Tag);

            EditarCheque edita = new EditarCheque(cheque);

            edita.ShowDialog();
        }

        private void ltb_Cheques_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditarCheque();
        }


    }
}
