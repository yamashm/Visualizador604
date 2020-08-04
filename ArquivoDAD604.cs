using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Visualizador604
{
    public class ArquivoDAD604: Arquivo
    {
        public HeaderDAD604 HEADER { get; set; }
        public List<Cheque> CHEQUES { get; set; }
        public TrailerDAD604 TRAILER { get; set; }
    }

    public class HeaderDAD604 : DetalheArquivoCompe
    {
        public HeaderDAD604(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 0;
                NOME_REGISTRO = "Header DAD 604";
                CAMPOS = new List<CampoArquivoCompe>();
                
                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco remetente", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 8, 1, "Data do movimento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 13, 6, 0, "Nome do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 19, 180, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 199, 5, 1, "Versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 204, 10, 1, "Sequencial de arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 214, 27435, 2, "Filler"));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Header: " + erro.Message);
            }
        }
    }

    public class DetalheDAD604 : DetalheArquivoCompe
    {
        public DetalheDAD604(string linha, int numeroLinha, int quebra, int tamanhoTotal)
        {
            NUMERO_LINHA = numeroLinha;
            QUEBRA = quebra;
            TAMANHO_TOTAL_LINHA = tamanhoTotal;

            MontaCampos(linha);
        }

        public DetalheDAD604(string linha, int numeroLinha, SolidColorBrush cor)
        {
                NUMERO_LINHA = numeroLinha;
                Foreground = cor;

                MontaCampos(linha);
        }

        private void MontaCampos(string linha)
        {
            try
            {
                Text = linha;

                TIPO = 1;
                NOME_REGISTRO = "Detalhe DAD 604";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco remetente", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 29, 0, "Número do protocolo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 34, 3, 1, "Banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 37, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 38, 3, 1, "Local destino do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 41, 3, 1, "Banco do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 44, 4, 1, "Agência do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 48, 1, 1, "DV2", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 49, 12, 1, "Conta do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 61, 1, 1, "DV1", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 62, 6, 1, "Número do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 68, 1, 1, "DV3", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 69, 17, 1, "Valor do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(15, 86, 1, 1, "Tipificação do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 87, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 88, 3, 1, "Banco depositante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(18, 91, 4, 1, "Agência depositante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(19, 95, 4, 1, "Agência de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(20, 99, 12, 1, "Conta de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(21, 111, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(22, 112, 7, 1, "Número do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(23, 119, 3, 1, "Número sequencial do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(24, 122, 25, 1, "Número identificador", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(25, 147, 3, 1, "Tipo de documento original", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(26, 150, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(27, 151, 17, 1, "Valor correto do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(28, 168, 17, 1, "Valor do acerto", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(29, 185, 2, 1, "Motivo do acerto", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(30, 187, 9, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(31, 196, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(32, 199, 5, 1, "Número da versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(33, 204, 10, 1, "Sequencial de arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(34, 214, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(35, 215, 2, 1, "Total de registros da imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(36, 217, 2, 1, "Sequencial de imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(37, 219, 9, 1, "Tamanho da imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(38, 228, 9, 1, "Tamanho da assinatura", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(39, 237, 1, 0, "Indicador do tipo da imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(40, 238, 3, 2, "Filler", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Detalhe: " + erro.Message);
            }
        }
    }

    public class TrailerDAD604 : DetalheArquivoCompe
    {
        public long VALOR_TOTAL { get; set; }

        public TrailerDAD604(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 2;
                NOME_REGISTRO = "Trailer DAD 604";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco remetente", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 8, 1, "Data do movimento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 13, 6, 0, "Nome do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 19, 180, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 199, 5, 1, "Versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 204, 10, 1, "Sequencial de arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 214, 27435, 2, "Filler"));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Trailer: " + erro.Message);
            }
        }
    }
}
