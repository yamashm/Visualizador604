using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Visualizador604
{
    public class ArquivoEBCDIC: Arquivo
    {
        public List<LinhaEBCDIC> LINHAS { get; set; }
    }

    public class LinhaEBCDIC : DetalheArquivoCompe
    {
        public LinhaEBCDIC(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 1;
                NOME_REGISTRO = "Linha do Arquivo EBCDIC";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, linha.Length, 0, "Linha arquivo EBCDIC", ref linha));

            }
            catch (Exception erro)
            {
                throw new ApplicationException("Lotes Remetidos: " + erro.Message);
            }
        }
    }
}
