using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Visualizador604
{
    public static class Util
    {
        public static List<string> MontaListaLinhasArquivoASCII(FileInfo fnfo)
        {
            List<string> retorno = new List<string>();

            StreamReader arquivo = new StreamReader(fnfo.FullName);

            string linha = string.Empty;

            while ((linha = arquivo.ReadLine()) != null)
            {
                retorno.Add(linha);
            }

            arquivo.Close();

            return retorno;
        }

        public static List<string> MontaListaLinhasArquivoEBCDIC(FileInfo fnfo, int quebra, int fim)
        {
            List<string> retorno = new List<string>();

            byte[] buffer = null;
            byte[] bufferSaida = null;

            buffer = LeBinario(fnfo.FullName);

            int linhas = (buffer.Length) / fim;

            for (int i = 0; i < linhas; i++)
            {
                bufferSaida = new byte[fim];

                Array.Copy(buffer, fim * i, bufferSaida, 0, fim);

                retorno.Add(Encoding.UTF8.GetString(bufferSaida));
            }

            return retorno;
        }

        public static List<byte[]> MontaListaBinariosArquivo(FileInfo fnfo, int fim)
        {
            List<byte[]> retorno = new List<byte[]>();

            byte[] buffer = null;
            byte[] bufferSaida = null;

            buffer = LeBinario(fnfo.FullName);

            int linhas = (buffer.Length) / fim;

            for (int i = 0; i < linhas; i++)
            {
                bufferSaida = new byte[fim];

                Array.Copy(buffer, fim * i, bufferSaida, 0, fim);

                retorno.Add(bufferSaida);
            }

            return retorno;
        }

        public static List<byte[]> MontaListaBinariosArquivo(FileInfo fnfo, int ini, int fim)
        {
            List<byte[]> retorno = new List<byte[]>();

            byte[] buffer = null;
            byte[] bufferSaida = null;

            buffer = LeBinario(fnfo.FullName);

            int linhas = (buffer.Length) / fim;

            for (int i = 0; i < linhas; i++)
            {
                bufferSaida = new byte[fim];

                Array.Copy(buffer, (fim * i) + ini, bufferSaida, 0, fim);

                retorno.Add(bufferSaida);
            }

            return retorno;
        }

        public static byte[] GeraBinarioLinhaEBCDIC(DetalheArquivoCompe detalhe)
        {
            StringBuilder sb = new StringBuilder();
            byte[] binDetalhe = new byte[detalhe.TAMANHO_TOTAL_LINHA];

            foreach (CampoArquivoCompe c in detalhe.CAMPOS)
            {
                sb.Append(c.CONTEUDO);
            }

            Array.Copy(Util.ConverteASCIIParaEBCDIC(System.Text.ASCIIEncoding.ASCII.GetBytes(sb.ToString())), binDetalhe, detalhe.TAMANHO_TOTAL_LINHA);

            return binDetalhe;
        }

        public static byte[] LeBinario(string caminho)
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

        private static byte[] TabelaEBCDIC_ASCII = 
		{

		/* 00 */     0X00,0x01,0x02,0x03,0xB0,0x09,0xB1,0x7F,
		/* 08 */     0xB2,0xB3,0xB4,0x0B,0x0C,0x0D,0x0E,0x0F,
		/* 10 */     0x10,0x11,0x12,0x13,0xB5,0xB6,0x08,0xB7,
		/* 18 */     0x18,0x19,0xB8,0xB9,0x1C,0x1d,0x1E,0x1F,
		/* 20 */     0xBA,0xBB,0xBC,0xBD,0xBE,0x0A,0x17,0x1B,
		/* 28 */     0xBF,0xC0,0xC1,0xC2,0xC3,0x05,0x06,0x07,
		/* 30 */     0xC4,0xC5,0x16,0xC6,0xC7,0xC8,0xC9,0x04,
		/* 38 */     0xCA,0xCB,0xCC,0xCD,0x14,0x15,0xCE,0x1A,
		/* 40 */     0x20,0x5B,0x5D,0x9C,0x9D,0x9E,0xE0,0xE1,
		/* 48 */     0xCF,0xD0,0x9B,0x2E,0x3C,0x28,0x2B,0xD1,
		/* 50 */     0x26,0xA7,0xD2,0xD3,0xD4,0xD5,0xD6,0x85,
		/* 58 */     0x8A,0xD7,0x21,0x24,0x2A,0x29,0x3B,0x5E,
		/* 60 */     0x2D,0x2F,0xD8,0xD9,0x86,0x98,0xDA,0xDB,
		/* 68 */     0x88,0xDC,0x7C,0x2C,0x25,0x5F,0x3E,0x3F,
		/* 70 */     0x8D,0x95,0x97,0x81,0x87,0x84,0x89,0x8B,
		/* 78 */     0x94,0x60,0x3A,0x23,0x40,0x27,0x3D,0x22,
		/* 80 */     0xDD,0x61,0x62,0x63,0x64,0x65,0x66,0x67,
		/* 88 */     0x68,0x69,0x83,0xDE,0x8C,0x93,0x96,0xA0,
		/* 90 */     0x82,0x6A,0x6B,0x6C,0x6D,0x6E,0x6F,0x70,
		/* 98 */     0x71,0x72,0xa1,0xA2,0xDF,0xA4,0x9F,0xE2,
		/* A0 */     0x92,0x7E,0x73,0x74,0x75,0x76,0x77,0x78,
		/* A8 */     0x79,0x7A,0xAB,0xAC,0x8F,0xA8,0xE3,0xE4,
		/* B0 */     0xE5,0xE6,0xAA,0xE7,0xE8,0xE9,0xEA,0x8E,
		/* B8 */     0xEB,0xEC,0x99,0x9A,0xAD,0xAE,0x91,0xAF,
		/* C0 */     0x7B,0x41,0x42,0x43,0x44,0x45,0x46,0x47,
		/* C8 */     0x48,0x49,0xEE,0xA9,0x90,0xEF,0xF0,0xF1,
		/* D0 */     0x7D,0x4A,0x4B,0x4C,0x4D,0x4E,0x4F,0x50,
		/* D8 */     0x51,0x52,0xF2,0xA3,0xA5,0xF3,0xF4,0xF5,
		/* E0 */     0x5C,0xF6,0x53,0x54,0x55,0x56,0x57,0x58,
		/* E8 */     0x59,0x5A,0xF7,0xA6,0x80,0xF8,0xF9,0xFA,
		/* F0 */     0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,
		/* F8 */     0x38,0x39,0xFB,0xED,0xFC,0xFD,0xFE,0xFF  
		};


        private static byte[] TabelaASCII_EBCDIC = 
		{

		/* 00 */     0x00,0x01,0x02,0x03,0x37,0x2D,0x2E,0x2F,
		/* 08 */     0x16,0x05,0x25,0x0B,0x0C,0x0D,0x0E,0x0F,
		/* 10 */     0x10,0x11,0x12,0x13,0x3C,0x3D,0x32,0x26,
		/* 18 */     0x18,0x19,0x3F,0x27,0x1C,0x1D,0x1E,0x1F,
		/* 20 */     0x40,0x5A,0x7F,0x7B,0x5B,0x6C,0x50,0x7D,
		/* 28 */     0x4D,0x5D,0x5C,0x4E,0x6B,0x60,0x4B,0x61,
		/* 30 */     0xF0,0xF1,0xF2,0xF3,0xF4,0xF5,0xF6,0xF7,
		/* 38 */     0xF8,0xF9,0x7A,0x5E,0x4C,0x7E,0x6E,0x6F,
		/* 40 */     0x7C,0xC1,0xC2,0xC3,0xC4,0xC5,0xC6,0xC7,
		/* 48 */     0xC8,0xC9,0xD1,0xD2,0xD3,0xD4,0xD5,0xD6,
		/* 50 */     0xD7,0xD8,0xD9,0xE2,0xE3,0xE4,0xE5,0xE6,
		/* 58 */     0xE7,0xE8,0xE9,0x41,0xE0,0x42,0x5F,0x6D,
		/* 60 */     0x79,0x81,0x82,0x83,0x84,0x85,0x86,0x87,
		/* 68 */     0x88,0x89,0x91,0x92,0x93,0x94,0x95,0x96,
		/* 70 */     0x97,0x98,0x99,0xA2,0xA3,0xA4,0xA5,0xA6,
		/* 78 */     0xA7,0xA8,0xA9,0xC0,0x6A,0xD0,0xA1,0x07,
		/* 80 */     0xEC,0x73,0x90,0x8A,0x75,0x57,0x64,0x74,
		/* 88 */     0x68,0x76,0x58,0x77,0x8C,0x70,0xB7,0xAC,
		/* 90 */     0xCC,0xBE,0xA0,0x8D,0x78,0x71,0x8E,0x72,
		/* 98 */     0x65,0xBA,0xBB,0x4A,0x43,0x44,0x45,0x9E,
		/* A0 */     0x8F,0x9A,0x9B,0xDB,0x9D,0xDC,0xEB,0x51,
		/* A8 */     0xAD,0xCB,0xB2,0xAA,0xAB,0xBC,0xBD,0xBF,
		/* B0 */     0x04,0x06,0x08,0x09,0x0A,0x14,0x15,0x17,
		/* B8 */     0x1A,0x1B,0x20,0x21,0x22,0x23,0x24,0x28,
		/* C0 */     0x29,0x2A,0x2B,0x2C,0x30,0x31,0x33,0x34,
		/* C8 */     0x35,0x36,0x38,0x39,0x3A,0x3B,0x3E,0x48,
		/* D0 */     0x49,0x4F,0x52,0x53,0x54,0x55,0x56,0x59,
		/* D8 */     0x62,0x63,0x66,0x67,0x69,0x80,0x8B,0x9C,
		/* E0 */     0x46,0x47,0x9F,0xAE,0xAF,0xB0,0xB1,0xB3,
		/* E8 */     0xB4,0xB5,0xB6,0xB8,0xB9,0xFB,0xCA,0xCD,
		/* F0 */     0xCE,0xCF,0xDA,0xDD,0xDE,0xDF,0xE1,0xEA,
		/* F8 */     0xED,0xEE,0xEF,0xFA,0xFC,0xFD,0xFE,0xFF  
		};

        public static byte[] ConverteASCIIParaEBCDIC(byte[] asciiBytes)
        {
            if (asciiBytes == null) return null;

            byte[] ebcdicBytes = new byte[asciiBytes.Length];

            for (int i = 0; i < asciiBytes.Length; i++)
            {
                ebcdicBytes[i] = TabelaASCII_EBCDIC[asciiBytes[i]];
            }

            return ebcdicBytes;
        }

        public static byte[] ConverteEBCDICParaASCII(byte[] ebcdicBytes)
        {
            if (ebcdicBytes == null) return null;

            byte[] asciiBytes = new byte[ebcdicBytes.Length];

            for (int i = 0; i < ebcdicBytes.Length; i++)
            {
                asciiBytes[i] = TabelaEBCDIC_ASCII[ebcdicBytes[i]];
            }

            return asciiBytes;
        }

        public static string MontaCMC7(string codCompe, string codBanco, string codAgencia,
            string DV2, string numConta, string DV1, 
            string numCheque, string DV3, string tipificacao)
        {
            StringBuilder sbPrimeiraBanda = new StringBuilder();
            StringBuilder sbSegundaBanda = new StringBuilder();
            StringBuilder sbTerceiraBanda = new StringBuilder();
            StringBuilder sbCMC7 = new StringBuilder();

            sbPrimeiraBanda.Append(codBanco.PadLeft(3, '0')).Append(codAgencia.PadLeft(4, '0'));

            sbSegundaBanda.Append(codCompe.PadLeft(3, '0')).Append(numCheque.PadLeft(6, '0')).Append(tipificacao);

            if (numConta.Length > 10)
                numConta = numConta.Remove(0, numConta.Length - 10);

            sbTerceiraBanda.Append(numConta.PadLeft(10, '0'));

            sbCMC7.Append(sbPrimeiraBanda).Append(DV2).Append(sbSegundaBanda).Append(DV1).Append(sbTerceiraBanda).Append(DV3);

            return sbCMC7.ToString();
        }

        public static string MontaCMC7(string codbanco, string codagencia, string codcompe, string numcheque, string tipificacao, string numconta)
        {
            StringBuilder sbPrimeiraBanda = new StringBuilder();
            StringBuilder sbSegundaBanda = new StringBuilder();
            StringBuilder sbTerceiraBanda = new StringBuilder();
            StringBuilder sbCMC7 = new StringBuilder();
            int dv1;
            int dv2;
            int dv3;

            sbPrimeiraBanda.Append(codbanco.PadLeft(3, '0')).Append(codagencia.PadLeft(4, '0'));

            dv1 = DBase10(sbPrimeiraBanda.ToString());

            sbSegundaBanda.Append(codcompe.PadLeft(3, '0')).Append(numcheque.PadLeft(6, '0')).Append(tipificacao);

            dv2 = DBase10(sbSegundaBanda.ToString());

            if (numconta.Length > 10)
                numconta = numconta.Remove(0, numconta.Length - 10);

            sbTerceiraBanda.Append(numconta.PadLeft(10, '0'));

            dv3 = DBase10(sbTerceiraBanda.ToString());

            sbCMC7.Append(sbPrimeiraBanda).Append(dv2.ToString()).Append(sbSegundaBanda).Append(dv1.ToString()).Append(sbTerceiraBanda).Append(dv3.ToString());

            return sbCMC7.ToString();
        }

        public static int DBase10(string d)
        {
            try
            {
                int lSize = 0, iDig = 0;
                decimal dValor = 0;
                bool bFlag = true;
                lSize = (int)d.Length - 1;

                for (int x = lSize; x >= 0; x--)
                {
                    if (bFlag)
                    {
                        iDig = Convert.ToInt32(d.Substring(x, 1)) * 2;
                        bFlag = false;
                    }
                    else
                    {
                        iDig = Convert.ToInt32(d.Substring(x, 1));
                        bFlag = true;
                    }

                    if (iDig > 9)
                    {
                        iDig = 1 + (iDig - 10);
                        dValor += iDig;
                    }
                    else
                    {
                        dValor += iDig;
                    }
                }

                int iValor = Convert.ToInt32(dValor);
                iDig = Convert.ToInt32(10 * ((dValor / 10) - (iValor / 10)));
                if (iDig > 0)
                    iDig = 10 - iDig;
                return iDig;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu o seguinte erro:" + ex.Message);
            }
        }
    }
}
