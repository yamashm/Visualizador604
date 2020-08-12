using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Visualizador604
{
    public class CampoArquivoCompe
    {
        public int NUMERO { get; set; }
        public string POSICAO { get; set; }
        public byte TIPO { get; set; } // 0: alfa numerico; 1: numerico; 2: filler
        public int TAMANHO { get; set; }
        public string DESCRICAO { get; set; }
        public string CONTEUDO { get; set; }
        public bool PERMITE_EDICAO_INDIVIDUAL { get; set; }

        public CampoArquivoCompe(CampoArquivoCompe campo)
        {
            NUMERO = campo.NUMERO;
            POSICAO = campo.POSICAO;
            TIPO = campo.TIPO;
            TAMANHO = campo.TAMANHO;
            DESCRICAO = campo.DESCRICAO;
            CONTEUDO = campo.CONTEUDO;
            PERMITE_EDICAO_INDIVIDUAL = campo.PERMITE_EDICAO_INDIVIDUAL;
        }

        public CampoArquivoCompe(int numero, int posicaoInicio, int tamanho, byte tipo, string descricao, ref string conteudo)
        {
            NUMERO = numero;
            POSICAO = new StringBuilder(posicaoInicio.ToString().PadLeft(3, '0')).Append("-").Append((posicaoInicio + tamanho - 1).ToString().PadLeft(3,'0')).ToString();
            TIPO = tipo;
            TAMANHO = tamanho;
            DESCRICAO = descricao;
            CONTEUDO = conteudo.Substring(posicaoInicio - 1, tamanho);
            PERMITE_EDICAO_INDIVIDUAL = true;
        }

        public CampoArquivoCompe(int numero, int posicaoInicio, int tamanho, byte tipo, string descricao, ref string conteudo, bool permiteEdicao)
        {
            NUMERO = numero;
            POSICAO = new StringBuilder(posicaoInicio.ToString().PadLeft(3, '0')).Append("-").Append((posicaoInicio + tamanho - 1).ToString().PadLeft(3, '0')).ToString();
            TIPO = tipo;
            TAMANHO = tamanho;
            DESCRICAO = descricao;
            CONTEUDO = conteudo.Substring(posicaoInicio - 1, tamanho);
            PERMITE_EDICAO_INDIVIDUAL = permiteEdicao;
        }

        public CampoArquivoCompe(int numero, int posicaoInicio, int tamanho, byte tipo, string descricao, string conteudo)
        {
            NUMERO = numero;
            POSICAO = new StringBuilder(posicaoInicio.ToString().PadLeft(3, '0')).Append("-").Append((posicaoInicio + tamanho - 1).ToString().PadLeft(3, '0')).ToString();
            TIPO = tipo;
            TAMANHO = tamanho;
            DESCRICAO = descricao;
            CONTEUDO = conteudo;
            PERMITE_EDICAO_INDIVIDUAL = true;
        }

        public CampoArquivoCompe(int numero, int posicaoInicio, int tamanho, byte tipo, string descricao)
        {
            NUMERO = numero;
            POSICAO = new StringBuilder(posicaoInicio.ToString().PadLeft(3, '0')).Append("-").Append((posicaoInicio + tamanho - 1).ToString().PadLeft(3, '0')).ToString();
            TIPO = tipo;
            TAMANHO = tamanho;
            DESCRICAO = descricao;
            CONTEUDO = new string(' ', tamanho);
            PERMITE_EDICAO_INDIVIDUAL = false;
        }
    }

    public class DetalheArquivoCompe: TextBlock
    {
        public Int32 NUMERO_LINHA { get; set; }
        public Int32 TIPO { get; set; }
        public String NOME_REGISTRO { get; set; }
        public Int32 QUEBRA { get; set; }
        public Int32 TAMANHO_TOTAL_LINHA { get; set; }
        public List<CampoArquivoCompe> CAMPOS { get; set; }
        public int INDICE_LOTE { get; set; }
        public int INDICE_CHEQUE { get; set; }
        public bool VERSO { get; set; }

        public DetalheArquivoCompe()
        {
            FontFamily = new System.Windows.Media.FontFamily("Courier New");
            FontSize = 14; 
            Height = GetScreenSize("0", new FontFamily("Courier New"), 14, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal).Height;
        }

        private Size GetScreenSize(string text, FontFamily fontFamily, double fontSize, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch)
        {
            fontFamily = fontFamily ?? new TextBlock().FontFamily;
            fontSize = fontSize > 0 ? fontSize : new TextBlock().FontSize;
            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            var ft = new FormattedText(text ?? string.Empty, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }

        public void RegeraLinha()
        {
            if (CAMPOS != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CampoArquivoCompe c in CAMPOS)
                {
                    if (c.TIPO == 1)
                    {
                        if (c.CONTEUDO == new string(' ', c.TAMANHO))
                            c.CONTEUDO = c.CONTEUDO.Replace(' ', '0');

                        c.CONTEUDO = c.CONTEUDO.PadLeft(c.TAMANHO, '0');
                    }
                    else if (c.TIPO == 0)
                    {
                        c.CONTEUDO = c.CONTEUDO.PadRight(c.TAMANHO, ' ');
                    }

                    sb.Append(c.CONTEUDO);
                }

                Text = sb.ToString();
            }
        }

        public byte[] GeraBinario()
        {
            byte[] bin = new byte[TAMANHO_TOTAL_LINHA];

            Array.Copy(Util.ConverteASCIIParaEBCDIC(System.Text.ASCIIEncoding.ASCII.GetBytes(Text)), bin, QUEBRA);

            return bin;
        }
    }

    public class LinhaErro : DetalheArquivoCompe
    {
        public LinhaErro(string linha, int numeroLinha, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;

                Foreground = cor;

                TIPO = 99;
                NOME_REGISTRO = "Linha Erro";
                CAMPOS = new List<CampoArquivoCompe>();
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Linha Erro: " + erro.Message);
            }
        }
    }
}
