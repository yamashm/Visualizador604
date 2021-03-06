﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Visualizador604
{
    public class ArquivoCELNRA604: Arquivo
    {
        public HeaderCEL604 HEADER { get; set; }
        public List<LoteCEL604> LOTES { get; set; }
        public TrailerCEL604 TRAILER { get; set; }
        public List<LinhaErro> LINHASERRO { get; set; }

        public string NOME_ARQUIVO { get; set; }
        public string VERSAO_ARQUIVO { get; set; }
        public string INDICADOR_REMESSA { get; set; }
        public string DATA_MOVIMENTO { get; set; }

        public List<byte[]> binLinhas()
        {
            List<byte[]> binArquivoSaida = new List<byte[]>();

            binArquivoSaida.Add(HEADER.GeraBinario());

            foreach (LoteCEL604 lote in LOTES)
            {
                foreach (Cheque c in lote.CHEQUES)
                {
                    binArquivoSaida.AddRange(c.GeraBinarios());
                }
                binArquivoSaida.Add(lote.FECHAMENTO.GeraBinario());
            }

            binArquivoSaida.Add(TRAILER.GeraBinario());

            return binArquivoSaida;
        }
    }

    public class HeaderCEL604 : DetalheArquivoCompe
    {
        public HeaderCEL604(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 0;
                NOME_REGISTRO = "Header CEL NRA 604";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 47, 1, "Controle do header", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 48, 6, 0, "Nome do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 54, 7, 1, "Número da versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 61, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 64, 1, 0, "Dígito verificador do banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 65, 1, 1, "Indicador de remessa", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 66, 8, 1, "Data do movimento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 74, 77, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 151, 10, 1, "Sequencial de arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 161, 27488, 2, "Filler"));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Header: " + erro.Message);
            }
        }
    }

    public class LoteCEL604
    {
        public int INDICE { get; set; }
        public int NUMERO { get; set; }
        public List<Cheque> CHEQUES { get; set; }
        public FechamentoCEL604 FECHAMENTO { get; set; }
    }

    public class DetalheCEL604 : DetalheArquivoCompe
    {
        public DetalheCEL604(string linha, int numeroLinha, int quebra, int tamanhoTotal)
        {
            NUMERO_LINHA = numeroLinha;
            QUEBRA = quebra;
            TAMANHO_TOTAL_LINHA = tamanhoTotal;

            MontaCampos(linha);
        }

        public DetalheCEL604(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            NUMERO_LINHA = numeroLinha;
            QUEBRA = quebra;
            TAMANHO_TOTAL_LINHA = tamanhoTotal;

            Foreground = cor;
            MontaCampos(linha);
        }

        private void MontaCampos(string linha)
        {
            try
            {
                Text = linha;

                TIPO = 1;
                NOME_REGISTRO = "Detalhe  CEL NRA 604";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 3, 1, "Local destino do cheque", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(2, 4, 3, 1, "Banco do cheque", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(3, 7, 4, 1, "Agência do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 11, 1, 1, "DV2", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 12, 12, 1, "Conta do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 24, 1, 1, "DV1", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 25, 6, 1, "Número do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 31, 1, 1, "DV3", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 32, 2, 0, "UF", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 34, 17, 1, "Valor do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 51, 1, 1, "Tipificação do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 52, 1, 1, "Tipo de conta depósto", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 53, 1, 1, "Canal de atendimento utilizado", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 54, 2, 2, "Filler", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(15, 56, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 59, 4, 1, "Agência apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 63, 4, 1, "Agência de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(18, 67, 12, 1, "Conta de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(19, 79, 3, 0, "Filler preenchimento livre", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(20, 82, 8, 1, "Data da apresentação", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(21, 90, 7, 1, "Número do lote", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(22, 97, 3, 1, "Número sequencial do lote", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(23, 100, 6, 0, "Centro processador do lote", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(24, 106, 25, 1, "Número identificador", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(25, 131, 10, 1, "Sequencial original de referência", ref linha,false));
                CAMPOS.Add(new CampoArquivoCompe(26, 141, 7, 1, "Número da versão do arquivo", ref linha,false));
                CAMPOS.Add(new CampoArquivoCompe(27, 148, 3, 1, "Tipo de documento", ref linha,false));
                CAMPOS.Add(new CampoArquivoCompe(28, 151, 10, 1, "Sequencial de arquivo", ref linha,false));
                CAMPOS.Add(new CampoArquivoCompe(29, 161, 40, 2, "Filler", ref linha,false));
                CAMPOS.Add(new CampoArquivoCompe(30, 201, 2, 1, "Total de registros da imagem", ref linha,false));
                CAMPOS.Add(new CampoArquivoCompe(31, 203, 2, 1, "Sequencial de imagem", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(32, 205, 9, 1, "Tamanho da imagem", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(33, 214, 9, 1, "Tamanho da assinatura", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(34, 223, 1, 0, "Indicador do tipo da imagem", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(35, 224, 17, 2, "Filler", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(36, 241, 27408, 2, "Filler"));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Detalhe: " + erro.Message);
            }
        }
    }

    public class FechamentoCEL604 : DetalheArquivoCompe
    {
        public FechamentoCEL604(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 2;
                NOME_REGISTRO = "Fechamento  CEL NRA 604"; 
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 3, 1, "Local destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 4, 3, 1, "Banco destinatário", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 7, 27, 1, "Controle do fechamento do lote", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(4, 34, 17, 1, "Valor do lote", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(5, 51, 5, 2, "Filler", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(6, 56, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 59, 23, 2, "Filler", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(8, 82, 8, 1, "Data do movimento", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(9, 90, 7, 1, "Número do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 97, 3, 1, "Número sequencial do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 100, 6, 1, "Centro processador", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 106, 25, 2, "Filler", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(13, 131, 10, 1, "Sequencial original de referência", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(14, 141, 7, 1, "Número da versão do arquivo", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(15, 148, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 151, 10, 1, "Sequencial de arquivo", ref linha, false));
                CAMPOS.Add(new CampoArquivoCompe(17, 161, 27488, 2, "Filler"));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Fechamento: " + erro.Message);
            }
        }
    }

    public class TrailerCEL604 : DetalheArquivoCompe
    {
        public TrailerCEL604(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 3;
                NOME_REGISTRO = "Trailer  CEL NRA 604";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 47, 1, "Controle do trailer", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 48, 6, 0, "Nome do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 54, 7, 1, "Número da versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 61, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 64, 1, 0, "Dígito verificador do banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 65, 1, 1, "Indicador de remessa", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 66, 8, 1, "Data do movimento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 74, 17, 1, "Valor do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 91, 60, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 151, 10, 1, "Sequencial de arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 161, 27488, 2, "Filler"));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Trailer: " + erro.Message);
            }
        }
    }
}
