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
    /// Interaction logic for EditarCheque.xaml
    /// </summary>
    public partial class EditarCheque : Window
    {
        DetalheArquivoCompe _detalheF;
        DetalheArquivoCompe _detalheV;

        Cheque _cheque;
        DataTable _dt;

        byte[] _imagemFrente;
        byte[] _assinaturaFrente;
        byte[] _imagemVerso;
        byte[] _assinaturaVerso;

        public EditarCheque(Cheque cheque)
        {
            InitializeComponent();

            _dt = new DataTable();

            _dt.Columns.Add("Campo", typeof(int));
            _dt.Columns.Add("Descrição", typeof(string));
            _dt.Columns.Add("Posição", typeof(string));
            _dt.Columns.Add("Valor", typeof(string));

            _cheque = cheque;
            _detalheF = _cheque.DADOS_FRENTE[0];

            this.Title = "Cheque " + cheque.INDICE.ToString();

            foreach (CampoArquivoCompe c in _detalheF.CAMPOS)
            {
                if (c.PERMITE_EDICAO_INDIVIDUAL)
                {
                    _dt.Rows.Add(c.NUMERO, c.DESCRICAO, c.POSICAO, c.CONTEUDO);
                }
            }

            if (_cheque.IMAGEM_FRENTE != null)
            {
                imgChequeF.Source = Imaging.CreateBitmapSourceFromHBitmap(
                     BitmapFromByteArray(_cheque.IMAGEM_FRENTE).GetHbitmap(),
                     IntPtr.Zero,
                     Int32Rect.Empty,
                     BitmapSizeOptions.FromEmptyOptions());

                if (_cheque.ASSINATURA_FRENTE != null)
                {
                    txb_NomeAssinaturaFrente.Text = "Assinatura do arquivo";
                    _assinaturaFrente = _cheque.ASSINATURA_FRENTE;
                }
                else
                {
                    txb_NomeAssinaturaFrente.Text = "Assinatura não carregada";
                }

                if (_cheque.IMAGEM_VERSO != null)
                {
                    _detalheV = _cheque.DADOS_VERSO[0];

                    imgChequeV.Source = Imaging.CreateBitmapSourceFromHBitmap(
                         BitmapFromByteArray(_cheque.IMAGEM_VERSO).GetHbitmap(),
                         IntPtr.Zero,
                         Int32Rect.Empty,
                         BitmapSizeOptions.FromEmptyOptions());
                }

                if (_cheque.ASSINATURA_VERSO != null)
                {
                    txb_NomeAssinaturaVerso.Text = "Assinatura do arquivo";
                    _assinaturaVerso = _cheque.ASSINATURA_VERSO;
                }
                else
                {
                    txb_NomeAssinaturaVerso.Text = "Assinatura não carregada";
                }
            }
            else
            {
                //btnAlteraImagemFrente.Visibility = System.Windows.Visibility.Collapsed;
                //btnAlteraImagemVerso.Visibility = System.Windows.Visibility.Collapsed;
                //imgChequeF.Visibility = System.Windows.Visibility.Collapsed;
                //imgChequeV.Visibility = System.Windows.Visibility.Collapsed;
            }

            dtgDetalhes.ItemsSource = _dt.DefaultView;

            dtgDetalhes.Focus(); 
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

        private void btnRegistraAlteracao_Click(object sender, RoutedEventArgs e)
        {
            if (_imagemFrente != null)
            {
                _cheque.IMAGEM_FRENTE = new byte[_imagemFrente.Length];
                Array.Copy(_imagemFrente, _cheque.IMAGEM_FRENTE, _imagemFrente.Length);
            }

            if (_assinaturaFrente != null)
            {
                _cheque.ASSINATURA_FRENTE = new byte[_assinaturaFrente.Length];
                Array.Copy(_assinaturaFrente, _cheque.ASSINATURA_FRENTE, _assinaturaFrente.Length);
            }

            if (_imagemVerso != null)
            {
                _cheque.IMAGEM_VERSO = new byte[_imagemVerso.Length];
                Array.Copy(_imagemVerso, _cheque.IMAGEM_VERSO, _imagemVerso.Length);

                if (_cheque.DADOS_VERSO == null)
                {
                    _cheque.DADOS_VERSO = new List<DetalheArquivoCompe>();
                    _detalheV = _detalheF;
                    _cheque.DADOS_VERSO.Add(_detalheV);
                }
            }

            if (_assinaturaVerso != null)
            {
                _cheque.ASSINATURA_VERSO = new byte[_assinaturaVerso.Length];
                Array.Copy(_assinaturaVerso, _cheque.ASSINATURA_VERSO, _assinaturaVerso.Length);
            }

            if (_cheque.IMAGEM_FRENTE == null)
            {
                _cheque.ASSINATURA_FRENTE = null;
                _cheque.IMAGEM_VERSO = null;
                _cheque.ASSINATURA_VERSO = null;
            }
            else
            {
                if (_cheque.ASSINATURA_FRENTE == null)
                {
                    MessageBox.Show("Não tem assinatura da frente");
                    return;
                }
                if (_cheque.IMAGEM_VERSO == null)
                {
                    MessageBox.Show("Não tem imagem do verso");
                    return;
                }
                if (_cheque.ASSINATURA_VERSO == null)
                {
                    MessageBox.Show("Não tem assinatura do verso");
                    return;
                }
            }

            Dictionary<int, string> listaIndicesValores = new Dictionary<int, string>();

            foreach (DataRowView dv in dtgDetalhes.Items.SourceCollection)
            {
                listaIndicesValores.Add(Convert.ToInt32(dv.Row[0]), dv.Row[3].ToString());
            }

            foreach (int i in listaIndicesValores.Keys)
            {
                CampoArquivoCompe campo = _detalheF.CAMPOS.Find(x => x.NUMERO == i);

                if (listaIndicesValores[i].Length > campo.TAMANHO)
                    campo.CONTEUDO = listaIndicesValores[i].Substring(0, campo.TAMANHO);
                else
                    campo.CONTEUDO = listaIndicesValores[i];

                if (campo.CONTEUDO != _detalheF.CAMPOS.Find(x => x.NUMERO == i).CONTEUDO)
                {
                    _detalheF.CAMPOS.Remove(campo);
                    _detalheF.CAMPOS.Insert(i - 1, campo);
                }
            }

            _detalheF.RegeraLinha();

            if (_cheque.DADOS_VERSO != null)
            {
                foreach (int i in listaIndicesValores.Keys)
                {
                    CampoArquivoCompe campo = _detalheV.CAMPOS.Find(x => x.NUMERO == i);

                    if (listaIndicesValores[i].Length > campo.TAMANHO)
                        campo.CONTEUDO = listaIndicesValores[i].Substring(0, campo.TAMANHO);
                    else
                        campo.CONTEUDO = listaIndicesValores[i];

                    if (campo.CONTEUDO != _detalheV.CAMPOS.Find(x => x.NUMERO == i).CONTEUDO)
                    {
                        _detalheV.CAMPOS.Remove(campo);
                        _detalheV.CAMPOS.Insert(i - 1, campo);
                    }
                }

                _detalheV.RegeraLinha();
            }

            _cheque.DADOS_FRENTE.Clear();

            byte totalRegistroImagem = 0;
            long tamanhoTotal = 0;
            int restoLinha = 0;

            if (_cheque.IMAGEM_FRENTE != null)
            {
                tamanhoTotal = _cheque.IMAGEM_FRENTE.Length + _cheque.ASSINATURA_FRENTE.Length;
                restoLinha = _detalheF.TAMANHO_TOTAL_LINHA - _detalheF.QUEBRA;

                while (tamanhoTotal > 0)
                {
                    totalRegistroImagem++;
                    tamanhoTotal = tamanhoTotal - restoLinha;
                }

                _cheque.TOTAL_REGISTROS_IMAGEM_FRENTE = totalRegistroImagem;

                DetalheArquivoCompe detalhe;

                for (int i = 0; i < totalRegistroImagem; i++)
                {
                    detalhe = new DetalheArquivoCompe();
                    detalhe.CAMPOS = new List<CampoArquivoCompe>(_detalheF.CAMPOS.Count);

                    detalhe.VERSO = false;
                    detalhe.INDICE_LOTE = _detalheF.INDICE_LOTE;
                    detalhe.INDICE_CHEQUE = _detalheF.INDICE_CHEQUE;
                    detalhe.NOME_REGISTRO = _detalheF.NOME_REGISTRO;
                    detalhe.Text = _detalheF.Text;
                    detalhe.Foreground = _detalheF.Foreground;

                    _detalheF.CAMPOS.ForEach((item) =>
                    {
                        detalhe.CAMPOS.Add(new CampoArquivoCompe(item));
                    });

                    _cheque.DADOS_FRENTE.Add(detalhe);
                }

                if (_cheque.DADOS_VERSO != null)
                {
                    _cheque.DADOS_VERSO.Clear();

                    totalRegistroImagem = 0;
                    tamanhoTotal = _cheque.IMAGEM_VERSO.Length + _cheque.ASSINATURA_VERSO.Length;

                    while (tamanhoTotal > 0)
                    {
                        totalRegistroImagem++;
                        tamanhoTotal = tamanhoTotal - restoLinha;
                    }

                    _cheque.TOTAL_REGISTROS_IMAGEM_VERSO = totalRegistroImagem;

                    for (int i = 0; i < totalRegistroImagem; i++)
                    {
                        detalhe = new DetalheArquivoCompe();
                        detalhe.CAMPOS = new List<CampoArquivoCompe>(_detalheV.CAMPOS.Count);

                        detalhe.VERSO = true;
                        detalhe.INDICE_LOTE = _detalheV.INDICE_LOTE;
                        detalhe.INDICE_CHEQUE = _detalheV.INDICE_CHEQUE;
                        detalhe.NOME_REGISTRO = _detalheV.NOME_REGISTRO;
                        detalhe.Text = _detalheV.Text;
                        detalhe.Foreground = _detalheV.Foreground;

                        _detalheV.CAMPOS.ForEach((item) =>
                        {
                            detalhe.CAMPOS.Add(new CampoArquivoCompe(item));
                        });

                        _cheque.DADOS_VERSO.Add(detalhe);
                    }
                }
            }
            else
            {
                _cheque.DADOS_FRENTE.Add(_detalheF);
                _cheque.DADOS_VERSO = null;
            }

            this.DialogResult = true;
        }

        private void btnAlteraImagemFrente_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                _imagemFrente = Util.LeBinario(ofd.FileName);

                mostraImagem(imgChequeF, _imagemFrente);
            }
        }

        private void btnAlteraImagemVerso_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                _imagemVerso = Util.LeBinario(ofd.FileName);

                mostraImagem(imgChequeV, _imagemVerso);
            }
        }

        private void mostraImagem(System.Windows.Controls.Image imageControl, byte[] imagem)
        {
            try
            {
                imageControl.Source = Imaging.CreateBitmapSourceFromHBitmap(
                     BitmapFromByteArray(imagem).GetHbitmap(),
                     IntPtr.Zero,
                     Int32Rect.Empty,
                     BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btnAlteraAssinaturaFrente_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                _assinaturaFrente = Util.LeBinario(ofd.FileName);
                txb_NomeAssinaturaFrente.Text = ofd.FileName;
            }
        }

        private void btnAlteraAssinaturaVersoe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                _assinaturaVerso = Util.LeBinario(ofd.FileName);
                txb_NomeAssinaturaVerso.Text = ofd.FileName;
            }
        }

        private void btnRemoveImagensAssinaturas_Click(object sender, RoutedEventArgs e)
        {
            _imagemFrente = null;
            _assinaturaFrente = null;
            _imagemVerso = null;
            _assinaturaVerso = null;
            _cheque.IMAGEM_FRENTE = null;
            _cheque.ASSINATURA_FRENTE = null;
            _cheque.IMAGEM_VERSO = null;
            _cheque.ASSINATURA_VERSO = null;

            imgChequeF.Source = null;
            imgChequeV.Source = null;

            txb_NomeAssinaturaFrente.Text = string.Empty;
            txb_NomeAssinaturaVerso.Text = string.Empty;
        }

        private void btnSalvarImagemFrente_Click(object sender, RoutedEventArgs e)
        {
            SalvaArquivo(_cheque.IMAGEM_FRENTE, _cheque.CMC7 + "_F.tif");
        }

        private void btnSalvarImagemVerso_Click(object sender, RoutedEventArgs e)
        {
            SalvaArquivo(_cheque.IMAGEM_VERSO, _cheque.CMC7 + "_V.tif");
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

        private void btnLerXML_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                string cmc7;

                if (XMLCheque.Le(ofd.FileName, out cmc7, ref _imagemFrente, ref _imagemVerso))
                {
                    MemoryStream ms;

                    if (_imagemFrente.Length > 0)
                    {
                        mostraImagem(imgChequeF, _imagemFrente);
                    }

                    if (_imagemVerso.Length > 0)
                    {
                        mostraImagem(imgChequeV, _imagemVerso);
                    }

                }
                else
                {
                    MessageBox.Show("Não foi possível ler o arquivo", "Erro");
                }
            }
        }

        private void btnSalvaAssinaturaFrente_Click(object sender, RoutedEventArgs e)
        {
            SalvaArquivo(_cheque.ASSINATURA_FRENTE, _cheque.CMC7 + "_F.P7S");
        }

        private void btnSalvaAssinaturaVerso_Click(object sender, RoutedEventArgs e)
        {
            SalvaArquivo(_cheque.ASSINATURA_VERSO, _cheque.CMC7 + "_V.P7S");
        }
    }
}
