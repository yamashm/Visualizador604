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
using Microsoft.Win32;

namespace Visualizador604
{
    /// <summary>
    /// Interaction logic for DetalhesRegistroOcorrencia.xaml
    /// </summary>
    public partial class DetalhesRegistro : Window
    {
        DetalheArquivoCompe _detalhe;
        DataTable _dt;

        byte[] _imagem;
        byte[] _assinatura;
        string _nomeImagem;
        string _nomeAssinatura;

        public DetalhesRegistro(Object registro, byte tipoArquivo)
        {
            InitializeComponent();

            _dt = new DataTable();

            _dt.Columns.Add("Campo", typeof(int));
            _dt.Columns.Add("Descrição", typeof(string));
            _dt.Columns.Add("Posição", typeof(string));
            _dt.Columns.Add("Valor", typeof(string));

            _detalhe = (DetalheArquivoCompe)registro;

            this.Title = _detalhe.NOME_REGISTRO;

            foreach (CampoArquivoCompe c in _detalhe.CAMPOS)
            {
                if (c.TIPO != 2) //Se não for Filler
                {
                    _dt.Rows.Add(c.NUMERO, c.DESCRICAO, c.POSICAO, c.CONTEUDO);
                }
            }

            this.Height = 350;

            btnSalvaImagem.Visibility = System.Windows.Visibility.Hidden;
            btnSalvaAssinatura.Visibility = System.Windows.Visibility.Hidden;

            dtgDetalhes.ItemsSource = _dt.DefaultView;

            dtgDetalhes.Focus(); 
        }

        public DetalhesRegistro(Object registro, Cheque cheque, byte tipoArquivo)
        {
            InitializeComponent();

            _dt = new DataTable();

            _dt.Columns.Add("Campo", typeof(int));
            _dt.Columns.Add("Descrição", typeof(string));
            _dt.Columns.Add("Posição", typeof(string));
            _dt.Columns.Add("Valor", typeof(string));

            _detalhe = (DetalheArquivoCompe)registro;

            this.Title = _detalhe.NOME_REGISTRO;

            foreach (CampoArquivoCompe c in _detalhe.CAMPOS)
            {
                if (c.TIPO != 2) //Se não for Filler
                {
                    _dt.Rows.Add(c.NUMERO, c.DESCRICAO, c.POSICAO, c.CONTEUDO);
                }
            }

            if (cheque.IMAGEM_FRENTE != null)
            {
                if (!_detalhe.VERSO)
                {
                    imgCheque.Source = Imaging.CreateBitmapSourceFromHBitmap(
                         BitmapFromByteArray(cheque.IMAGEM_FRENTE).GetHbitmap(),
                         IntPtr.Zero,
                         Int32Rect.Empty,
                         BitmapSizeOptions.FromEmptyOptions());

                    _imagem = cheque.IMAGEM_FRENTE;
                    _nomeImagem = cheque.CMC7 + "_F.tif";

                    if (cheque.ASSINATURA_FRENTE != null)
                    {
                        _assinatura = cheque.ASSINATURA_FRENTE;
                        _nomeAssinatura = cheque.CMC7 + "_F.P7S";
                    }
                }
                else
                {
                    imgCheque.Source = Imaging.CreateBitmapSourceFromHBitmap(
                         BitmapFromByteArray(cheque.IMAGEM_VERSO).GetHbitmap(),
                         IntPtr.Zero,
                         Int32Rect.Empty,
                         BitmapSizeOptions.FromEmptyOptions());

                    _imagem = cheque.IMAGEM_VERSO;
                    _nomeImagem = cheque.CMC7 + "_V.tif";

                    if (cheque.ASSINATURA_VERSO != null)
                    {
                        _assinatura = cheque.ASSINATURA_VERSO;
                        _nomeAssinatura = cheque.CMC7 + "_V.P7S";
                    }
                }
            }
            else
            {
                this.Height = 350;
                btnSalvaImagem.Visibility = System.Windows.Visibility.Hidden;
            }

            dtgDetalhes.ItemsSource = _dt.DefaultView;

            dtgDetalhes.Focus(); 
        }

        private void FormataTela(byte tipoArquivo)
        {
            switch (tipoArquivo)
            {
                case 1:
                    imgCheque.IsEnabled = true;
                    btnSalvaImagem.Visibility = System.Windows.Visibility.Visible;
                    btnSalvaAssinatura.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    imgCheque.IsEnabled = false;
                    btnSalvaImagem.Visibility = System.Windows.Visibility.Hidden;
                    btnSalvaAssinatura.Visibility = System.Windows.Visibility.Hidden;
                    break;
            }
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

        private void SalvaArquivo(byte[] arquivo, string nome)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = nome;

            if (sfd.ShowDialog() == true)
            {
                BinaryWriter bw = new BinaryWriter(File.Open(sfd.FileName, FileMode.OpenOrCreate));
                bw.Write(arquivo);

                bw.Flush();
                bw.Close();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void btnRegistraAlteracao_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, string> listaIndicesValores = new Dictionary<int, string>();

            foreach (DataRowView dv in dtgDetalhes.Items.SourceCollection)
            {
                listaIndicesValores.Add(Convert.ToInt32(dv.Row[0]), dv.Row[3].ToString());
            }

            StringBuilder sb = new StringBuilder();

            foreach (int i in listaIndicesValores.Keys)
            {
                CampoArquivoCompe campo = _detalhe.CAMPOS.Find(x => x.NUMERO == i);

                if (listaIndicesValores[i].Length > campo.TAMANHO)
                    listaIndicesValores[i].Substring(0, campo.TAMANHO);

                if (campo.TIPO == 1)
                {
                    campo.CONTEUDO = listaIndicesValores[i].PadLeft(campo.TAMANHO, '0');
                }
                else if (campo.TIPO == 0)
                {
                    campo.CONTEUDO = listaIndicesValores[i].PadRight(campo.TAMANHO, ' ');
                }

                if (campo.CONTEUDO != _detalhe.CAMPOS.Find(x => x.NUMERO == i).CONTEUDO)
                {
                    _detalhe.CAMPOS.Remove(campo);
                    _detalhe.CAMPOS.Insert(i - 1, campo);
                }
            }

            foreach (CampoArquivoCompe c in _detalhe.CAMPOS)
            {
                sb.Append(c.CONTEUDO);
            }

            _detalhe.Text = sb.ToString().Substring(0, _detalhe.QUEBRA);
        }

        private void btnSalvarImagem_Click(object sender, RoutedEventArgs e)
        {
            SalvaArquivo(_imagem, _nomeImagem);
        }

        private void btnSalvarAssinatura_Click(object sender, RoutedEventArgs e)
        {
            SalvaArquivo(_assinatura, _nomeAssinatura);
        }
    }
}
