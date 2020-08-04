using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Visualizador604
{
    /// <summary>
    /// Interaction logic for Carregando.xaml
    /// </summary>
    public partial class Carregando : Window
    {
        public delegate void Carregou(List<Dado604> dados, List<string> bancosValores, List<string> dadosLotes, string tipoarq, string versaoarquivo, string datamov, string qtdcheques, string qtdfechamentos, string valortotal);
        public event Carregou OnCarregou;

        public Carregando()
        {
            InitializeComponent();

            //AbreArquivo(caminho);
        }

        private void AbreArquivo(string caminho)
        {
            bool retorno = true;

            byte[] buffer = null;
            byte[] bufferSaida;
            int fim = 27648;
            int quebra = 240;

            bool contoucheque = false;

            long qtdcheques = 0;
            long qtdfechamentos = 0;
            string valortotal = string.Empty;
            string tipoarq = string.Empty;
            string versaoarquivo = string.Empty;
            string datamov = string.Empty;

            int qtdchequeslote = 0;

            List<Dado604> dados = new List<Dado604>();

            List<string> bancosValores = new List<string>();
            List<string> dadosLotes = new List<string>();

            try
            {
                buffer = LeBinario(caminho);

                long linhas = (buffer.Length) / fim;

                //pgbCarregando.Minimum = 0;
                //pgbCarregando.Maximum = linhas;
                //pgbCarregando.Value = 0;

                for (int i = 0; i < linhas; i++)
                {
                    bufferSaida = new byte[fim];

                    Array.Copy(buffer, fim * i, bufferSaida, 0, fim);

                    //_dadoscompletos.Add(bufferSaida);
                    bufferSaida = Tabela.ConverteEBCDICParaASCII(bufferSaida);

                    //TextBlock item = new TextBlock();
                    Dado604 item = new Dado604();
                    item.FontFamily = new System.Windows.Media.FontFamily("Courier New");
                    item.FontSize = 14;
                    item.Height = GetScreenSize("0", new FontFamily("Courier New"), 14, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal).Height;

                    item.Text = Encoding.UTF8.GetString(bufferSaida).Substring(0, quebra) + "\n";

                    if (item.Text.Substring(0, 47) == "00000000000000000000000000000000000000000000000")
                    {
                        tipoarq = item.Text.Substring(47, 6);
                        versaoarquivo = item.Text.Substring(56, 4);
                        datamov = item.Text.Substring(65, 8);
                        item.Tag = 0;
                    }

                    else if (item.Text.Substring(0, 47) == "99999999999999999999999999999999999999999999999")
                    {
                        valortotal = item.Text.Substring(73, 17);
                        item.Tag = 3;
                    }

                    else if (item.Text.Substring(6, 27) == "999999999999999999999999999")
                    {
                        qtdfechamentos++;
                        item.Tag = 2;

                        bancosValores.Add(item.Text.Substring(3, 3) + item.Text.Substring(33, 17));
                        dadosLotes.Add(new StringBuilder(qtdfechamentos.ToString()).Append("&").Append(item.Text.Substring(89, 8)).Append("&").Append(item.Text.Substring(33, 17)).Append("&").Append(qtdchequeslote.ToString()).ToString());

                        qtdchequeslote = 0;

                    }
                    else if ((item.Text.Substring(222, 1) == "F"))
                    {
                        if (!contoucheque)
                        {
                            qtdcheques++;
                            qtdchequeslote++;
                        }

                        string codbanco = item.Text.Substring(3, 3);
                        string codagencia = item.Text.Substring(6, 4);
                        string codcompe = item.Text.Substring(0, 3);
                        string numcheque = item.Text.Substring(24, 6);
                        string tipificacao = item.Text.Substring(50, 1);
                        string numconta = item.Text.Substring(11, 12);

                        item.CMC7 = Util.MontaCMC7(codbanco, codagencia, codcompe, numcheque, tipificacao, numconta);

                        item.binImagemAss = new byte[27408];
                        Array.Copy(Tabela.ConverteASCIIParaEBCDIC(bufferSaida), 240, item.binImagemAss, 0, 27408);

                        item.Tag = 1;
                        contoucheque = true;
                    }
                    else if ((item.Text.Substring(222, 1) == "V"))
                    {
                        string codbanco = item.Text.Substring(3, 3);
                        string codagencia = item.Text.Substring(6, 4);
                        string codcompe = item.Text.Substring(0, 3);
                        string numcheque = item.Text.Substring(24, 6);
                        string tipificacao = item.Text.Substring(50, 1);
                        string numconta = item.Text.Substring(11, 12);

                        item.CMC7 = Util.MontaCMC7(codbanco, codagencia, codcompe, numcheque, tipificacao, numconta);

                        item.binImagemAss = new byte[27408];
                        Array.Copy(Tabela.ConverteASCIIParaEBCDIC(bufferSaida), 240, item.binImagemAss, 0, 27408);

                        item.Tag = 12;
                        contoucheque = false;
                    }
                    else if (item.Text.Substring(222, 1) == " ")
                    {
                        qtdcheques++;
                        item.Tag = 13;
                    }

                    dados.Add(item);
                    //pgbCarregando.Value = i + 1;
                }
            }
            catch
            {
                dados = null;
                bancosValores = null;
                dadosLotes = null;
            }

            if (OnCarregou != null)
                OnCarregou(dados, bancosValores, dadosLotes, tipoarq, versaoarquivo, datamov, qtdcheques.ToString(), qtdfechamentos.ToString(), valortotal);

            //this.Close();
        }

        private byte[] LeBinario(string caminho)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(caminho, FileMode.Open, FileAccess.Read);
            try
            {
                int tamanho = (int)fileStream.Length;
                buffer = new byte[tamanho];
                int contador;
                int soma = 0;

                while ((contador = fileStream.Read(buffer, soma, tamanho - soma)) > 0)
                    soma += contador;
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        private Size GetScreenSize(string text, FontFamily fontFamily, double fontSize, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch)
        {
            fontFamily = fontFamily ?? new TextBlock().FontFamily;
            fontSize = fontSize > 0 ? fontSize : new TextBlock().FontSize;
            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            var ft = new FormattedText(text ?? string.Empty, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
    }
}
