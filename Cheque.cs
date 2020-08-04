using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visualizador604
{
    public class Cheque
    {
        public int INDICE { get; set; }
        public int INDICE_LOTE { get; set; }
        public string CMC7 { get; set; }
        public long VALOR { get; set; }
        public byte TOTAL_REGISTROS_IMAGEM_FRENTE { get; set; }
        public byte TOTAL_REGISTROS_IMAGEM_VERSO { get; set; }
        public byte[] IMAGEM_FRENTE { get; set; }
        public byte[] ASSINATURA_FRENTE { get; set; }
        public byte[] IMAGEM_VERSO { get; set; }
        public byte[] ASSINATURA_VERSO { get; set; }
        public List<DetalheArquivoCompe> DADOS_FRENTE { get; set; }
        public List<DetalheArquivoCompe> DADOS_VERSO { get; set; }
        public int NUMERO { get; set; }
        public int NUMERO_LOTE { get; set; }

        public Cheque()
        {
        }

        public void RegeraLinhasFrenteVerso()
        {
            foreach (DetalheArquivoCompe d in DADOS_FRENTE)
            {
                d.RegeraLinha();
            }

            if (DADOS_VERSO != null)
            {
                foreach (DetalheArquivoCompe d in DADOS_VERSO)
                {
                    d.RegeraLinha();
                }
            }
        }
    }
}
