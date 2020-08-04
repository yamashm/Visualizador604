using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Visualizador604
{
    /// <summary>
    /// Interaction logic for Extraindo.xaml
    /// </summary>
    public partial class Extraindo : Window
    {
        //List<Dado604> _Lista;
        //List<DetalheArquivoCompe> _Lista;
        ArquivoCELNRA604 _arquivoCELNRA604;
        ArquivoDAD604 _arquivoDAD604;

        byte _tipoArquivo = 0;

        public delegate void Terminou();
        public event Terminou OnTerminou;

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public Extraindo(Arquivo dados)
        {
            InitializeComponent();

            _tipoArquivo = dados.TIPO_ARQUIVO;

            switch (_tipoArquivo)
            {
                case 1:
                    _arquivoCELNRA604 = (ArquivoCELNRA604)dados;
                    break;
                case 4:
                    _arquivoDAD604 = (ArquivoDAD604)dados;
                    break;
            }
            

            ckbImagens.IsChecked = true;

            OnTerminou += Extraindo_OnTerminou;
        }

        void Extraindo_OnTerminou()
        {
             this.Dispatcher.Invoke((Action)delegate {
                 this.Close();
             }, null);
        }

        private void ExtraiImagens(string Caminho, List<Dado604> Dados)
        {
            //ThreadStart threadStart = new ThreadStart(delegate() { ExtraiImagensThread(Caminho, Dados); });
            //Thread othread = new Thread(threadStart);
            //othread.IsBackground = true;
            //othread.Name = "ExtraiImagens()";

            //othread.Start();
        }

        private void ExtraiImagens()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb_Extraindo.Value = e.ProgressPercentage;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (sender as BackgroundWorker);
        }

        private void ExtraiImagensThread(string caminho)
        {
            if (txbCaminho.Text != string.Empty)
            {
                string caminhoFrente;
                string caminhoVerso;

                foreach (LoteCEL604 lote in _arquivoCELNRA604.LOTES)
                {
                    foreach (Cheque cheque in lote.CHEQUES)
                    {
                        caminhoFrente = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_F.tif").ToString());
                        caminhoVerso = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_V.tif").ToString());
                        File.WriteAllBytes(caminhoFrente, cheque.IMAGEM_FRENTE);
                        File.WriteAllBytes(caminhoVerso, cheque.IMAGEM_VERSO);
                    }
                }
            }
        }

        private void GravaImagensAssinatura(Cheque cheque, string caminho, bool imagens, bool assinaturas)
        {
            string caminhoFrente;
            string caminhoVerso;

            if (imagens)
            {
                if (cheque.IMAGEM_FRENTE != null || cheque.IMAGEM_FRENTE != null)
                {
                    caminhoFrente = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_F.tif").ToString());
                    caminhoVerso = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_V.tif").ToString());
                    File.WriteAllBytes(caminhoFrente, cheque.IMAGEM_FRENTE);
                    File.WriteAllBytes(caminhoVerso, cheque.IMAGEM_VERSO);
                }
            }

            if (assinaturas)
            {
                if (cheque.ASSINATURA_FRENTE != null || cheque.ASSINATURA_VERSO != null)
                {
                    caminhoFrente = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_F.ass").ToString());
                    caminhoVerso = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_V.ass").ToString());
                    File.WriteAllBytes(caminhoFrente, cheque.ASSINATURA_FRENTE);
                    File.WriteAllBytes(caminhoVerso, cheque.ASSINATURA_VERSO);
                }
            }
        }

        private void ExtraiImagensAssinaturasThread(byte tipoArquivo, string caminho, bool imagens, bool assinaturas)
        {
            if (txbCaminho.Text != string.Empty)
            {
                switch (_tipoArquivo)
                {
                    case 1:
                        if (_arquivoCELNRA604.LOTES.Count > 0)
                        {
                            pgb_Extraindo.Maximum = _arquivoCELNRA604.LOTES.Count;

                            foreach (LoteCEL604 lote in _arquivoCELNRA604.LOTES)
                            {
                                foreach (Cheque cheque in lote.CHEQUES)
                                {
                                    GravaImagensAssinatura(cheque, caminho, imagens, assinaturas);
                                }
                            }
                        }
                        break;
                    case 4:
                        if (_arquivoDAD604.CHEQUES.Count > 0)
                        {
                            pgb_Extraindo.Maximum = _arquivoDAD604.CHEQUES.Count;

                            foreach (Cheque cheque in _arquivoDAD604.CHEQUES)
                            {
                                GravaImagensAssinatura(cheque, caminho, imagens, assinaturas);
                            }
                        }
                        break;
                }
            }
        }


        private void ExtraiAssinaturasThread(string caminho)
        {
            if (txbCaminho.Text != string.Empty)
            {
                string caminhoFrente;
                string caminhoVerso;

                foreach (LoteCEL604 lote in _arquivoCELNRA604.LOTES)
                {
                    foreach (Cheque cheque in lote.CHEQUES)
                    {
                        caminhoFrente = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_F.ass").ToString());
                        caminhoVerso = System.IO.Path.Combine(caminho, new StringBuilder(cheque.CMC7).Append("_V.ass").ToString());
                        File.WriteAllBytes(caminhoFrente, cheque.ASSINATURA_FRENTE);
                        File.WriteAllBytes(caminhoVerso, cheque.ASSINATURA_VERSO);
                    }
                }
            }
        }

        private void btnExtrair_Click(object sender, RoutedEventArgs e)
        {
            //if (_arquivoCELNRA604.LOTES.Count > 0)
            //{
                txbCaminho.IsEnabled = false;
                btnExtrair.IsEnabled = false;

                pgb_Extraindo.Minimum = 0;

                ExtraiImagensAssinaturasThread(_tipoArquivo, @txbCaminho.Text, (bool)ckbImagens.IsChecked, (bool)ckbAssinaturas.IsChecked);

                //if (ckbImagens.IsChecked == true && ckbAssinaturas.IsChecked == true)
                //{
                //    ExtraiImagensThread(@txbCaminho.Text);
                //    ExtraiAssinaturasThread(@txbCaminho.Text);
                //}
                //else if (ckbImagens.IsChecked == true)
                //    ExtraiImagensThread(@txbCaminho.Text);
                //else if (ckbAssinaturas.IsChecked == true) 
                //    ExtraiAssinaturasThread(@txbCaminho.Text);

                txbCaminho.IsEnabled = true;
                btnExtrair.IsEnabled = true;
                    
           // }
        }
    }
}
