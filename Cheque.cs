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

        public List<byte[]> GeraBinarios()
        {
            List<byte[]> binList = new List<byte[]>();

            int i = 0;

            long tamanhoTotal = IMAGEM_FRENTE.Length + ASSINATURA_FRENTE.Length;
            byte[] imagemAssinatura = new byte[tamanhoTotal];
            Array.Copy(IMAGEM_FRENTE, 0, imagemAssinatura, 0, IMAGEM_FRENTE.Length);
            Array.Copy(ASSINATURA_FRENTE, 0, imagemAssinatura, imagemAssinatura.Length - ASSINATURA_FRENTE.Length, ASSINATURA_FRENTE.Length);

            foreach (DetalheArquivoCompe d in DADOS_FRENTE)
            {
                int restoLinha = d.TAMANHO_TOTAL_LINHA - d.QUEBRA;

                byte[] binDetalhe = new byte[d.TAMANHO_TOTAL_LINHA];
                Array.Copy(d.GeraBinario(), binDetalhe, d.QUEBRA);

                tamanhoTotal = tamanhoTotal - restoLinha;

                if (i + 1 == TOTAL_REGISTROS_IMAGEM_FRENTE)
                    Array.Copy(imagemAssinatura, i * restoLinha, binDetalhe, d.QUEBRA, restoLinha + tamanhoTotal);
                else
                    Array.Copy(imagemAssinatura, i * restoLinha, binDetalhe, d.QUEBRA, restoLinha);

                i++;

                binList.Add(binDetalhe);

            }

            i = 0;

            tamanhoTotal = IMAGEM_VERSO.Length + ASSINATURA_VERSO.Length;

            imagemAssinatura = new byte[tamanhoTotal];
            Array.Copy(IMAGEM_VERSO, 0, imagemAssinatura, 0, IMAGEM_VERSO.Length);
            Array.Copy(ASSINATURA_VERSO, 0, imagemAssinatura, imagemAssinatura.Length - ASSINATURA_VERSO.Length, ASSINATURA_VERSO.Length);

            foreach (DetalheArquivoCompe d in DADOS_VERSO)
            {
                int restoLinha = d.TAMANHO_TOTAL_LINHA - d.QUEBRA;

                byte[] binDetalhe = new byte[d.TAMANHO_TOTAL_LINHA];
                Array.Copy(d.GeraBinario(), binDetalhe, d.QUEBRA);

                tamanhoTotal = tamanhoTotal - restoLinha;

                if (i + 1 == TOTAL_REGISTROS_IMAGEM_VERSO)
                    Array.Copy(imagemAssinatura, i * restoLinha, binDetalhe, d.QUEBRA, restoLinha + tamanhoTotal);
                else
                    Array.Copy(imagemAssinatura, i * restoLinha, binDetalhe, d.QUEBRA, restoLinha);

                i++;

                binList.Add(binDetalhe);

            }
            
            return binList;
        }
    }
}
