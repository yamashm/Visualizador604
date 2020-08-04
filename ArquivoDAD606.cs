using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Visualizador604
{
    public class ArquivoDAD606: Arquivo
    {
        public HeaderDAD606 HEADER { get; set; }
        public List<Cheque> CHEQUES { get; set; }
        public TrailerDAD606 TRAILER { get; set; }
    }

    public class HeaderDAD606 : DetalheArquivoCompe
    {
        public HeaderDAD606(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 0;
                NOME_REGISTRO = "Header DAD 606";
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

    public class DetalheDAD606 : DetalheArquivoCompe
    {
        public DetalheDAD606(string linha, int numeroLinha, int quebra, int tamanhoTotal)
        {
            NUMERO_LINHA = numeroLinha;
            QUEBRA = quebra;
            TAMANHO_TOTAL_LINHA = tamanhoTotal;

            MontaCampos(linha);
        }

        public DetalheDAD606(string linha, int numeroLinha, SolidColorBrush cor)
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
                NOME_REGISTRO = "Detalhe DAD 606";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco remetente", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 29, 0, "Número do protocolo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 34, 3, 1, "Banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 37, 4, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 41, 3, 1, "Banco do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 44, 103, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 147, 3, 1, "Tipo de documento original", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 150, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 152, 8, 1, "Data da solicitação do acerto", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 160, 8, 1, "Data da resposta", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 168, 17, 1, "Valor do acerto", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 185, 2, 1, "Motivo do acerto", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 187, 9, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(15, 196, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 199, 5, 1, "Número da versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 204, 10, 1, "Sequencial de arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(18, 214, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(19, 215, 2, 1, "Total de registros da imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(20, 217, 2, 1, "Sequencial de imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(21, 219, 9, 1, "Tamanho da imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(22, 228, 9, 1, "Tamanho da assinatura", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(23, 237, 1, 0, "Indicador do tipo da imagem", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(24, 238, 3, 2, "Filler", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Detalhe: " + erro.Message);
            }
        }
    }

    public class TrailerDAD606 : DetalheArquivoCompe
    {
        public long VALOR_TOTAL { get; set; }

        public TrailerDAD606(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 2;
                NOME_REGISTRO = "Trailer DAD 606";
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
