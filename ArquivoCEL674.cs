using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;

namespace Visualizador604
{
    public class ArquivoCEL674: Arquivo
    {
        public HeaderCEL674 HEADER { get; set; }
        public List<LoteRemetido> LOTES_REMETIDOS { get; set; }
        public List<LoteRecebido> LOTES_RECEBIDOS { get; set; }
        public List<LoteRemetidoComOcorrencia> LOTES_REMETIDOS_COM_OCORRECIA { get; set; }
        public List<DetalheRemetidoRecusado> DETALHES_REMETIDOS_RECUSADOS { get; set; }
        public List<LoteRecebidoComOcorrencia> LOTES_RECEBIDOS_COM_OCORRENCIA { get; set; }
        public List<DetalheAReceberRecusado> DETALHES_A_RECEBER_RECUSADOS { get; set; }
        public List<OcorrenciasHeadereTrailer> OCORRECIAS_HEADER_TRAILER { get; set; }
        public List<VersoesProcessadas> VERSOES_PROCESSADAS { get; set; }
        public TrailerCEL674 TRAILER { get; set; }
    }

    public class HeaderCEL674 : DetalheArquivoCompe
    {
        public HeaderCEL674(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 0;
                NOME_REGISTRO = "Header CEL NRA 674"; 
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 9, 0, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 10, 6, 0, "Nome do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 16, 3, 1, "Banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 19, 1, 0, "Dígito verificador do código do banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 20, 1, 1, "Indicador de remessa", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 21, 4, 1, "Versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 25, 3, 1, "Número da prévia de processamento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 28, 8, 1, "Data do movimento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 36, 8, 1, "Data da criação do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 44, 4, 1, "Hora da criação do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 48, 5, 0, "Origem do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 53, 3, 0, "Flag fim", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 56, 79, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Header: " + erro.Message);
            }
        }
    }

    public class LoteRemetido : DetalheArquivoCompe
    {
        public LoteRemetido(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 1;
                NOME_REGISTRO = "Detalhe Tipo 1 - Lote Remetido";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 7, 3, 1, "Banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 10, 3, 1, "Local destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 13, 6, 1, "Quantidade de lotes remetidos", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 19, 17, 1, "Valor", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 36, 6, 1, "Quantidade de documentos", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 42, 85, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 127, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 130, 5, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Lotes Remetidos: " + erro.Message);
            }
        }
    }

    public class LoteRecebido : DetalheArquivoCompe
    {
        public LoteRecebido(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 2;
                NOME_REGISTRO = "Detalhe Tipo 2 - Lote Recebido";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 7, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 10, 3, 1, "Local destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 13, 6, 1, "Quantidade de lotes recebidos", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 19, 17, 1, "Valor", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 36, 6, 1, "Quantidade de documentos", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 42, 85, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 127, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 130, 5, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Lotes Recebidos: " + erro.Message);
            }
        }
    }

    public class LoteRemetidoComOcorrencia : DetalheArquivoCompe
    {
        public LoteRemetidoComOcorrencia(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 3;
                NOME_REGISTRO = "Detalhe Tipo 3 - Lote Remetido com Ocorrência";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 7, 3, 1, "Banco destinatário", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 10, 3, 1, "Local destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 13, 2, 1, "Tipo de ocorrência", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 15, 6, 1, "Quantidade apurada", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 21, 3, 1, "Quantidade informada", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 24, 17, 1, "Valor apurado", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 41, 17, 1, "Valor informado", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 58, 24, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 82, 7, 1, "Número do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 89, 26, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 115, 3, 1, "Ocorrência de erro", Ocorrencias.Descricao(Convert.ToInt16(linha.Substring(114, 3)))));
                CAMPOS.Add(new CampoArquivoCompe(15, 118, 10, 1, "Sequencial do erro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 128, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 131, 3, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(18, 134, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(19, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Lotes Remetidos com Ocorrência: " + erro.Message);
            }
        }
    }

    public class DetalheRemetidoRecusado : DetalheArquivoCompe
    {
        public DetalheRemetidoRecusado(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 4;
                NOME_REGISTRO = "Detalhe Tipo 3 - Detalhe Remetido Recusado";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 7, 3, 1, "Banco destinatário", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 10, 3, 1, "Local destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 13, 2, 1, "Tipo de ocorrência", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 15, 3, 1, "Local destino do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 18, 3, 1, "Banco do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 21, 4, 1, "Agência do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 25, 1, 1, "DV1", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 26, 12, 1, "Conta do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 38, 1, 1, "DV2", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 39, 6, 1, "Número do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 45, 1, 1, "DV3", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(15, 46, 2, 0, "UF", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 48, 17, 1, "Valor do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 65, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(18, 67, 1, 0, "Tipificação do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(19, 68, 3, 1, "Banco apresentante da troca", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(20, 71, 4, 1, "Agência apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(21, 75, 7, 1, "Número do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(22, 82, 3, 1, "Número sequencial do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(23, 85, 8, 1, "Data da apresentação", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(24, 93, 6, 0, "Centro processador do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(25, 99, 12, 1, "Conta de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(26, 111, 4, 1, "Agência de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(27, 115, 3, 1, "Ocorrência de erro", Ocorrencias.Descricao(Convert.ToInt16(linha.Substring(114, 3)))));
                CAMPOS.Add(new CampoArquivoCompe(28, 118, 10, 1, "Sequencial do erro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(29, 128, 3, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(30, 131, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(31, 134, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(32, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Detalhes Remetidos Recusados: " + erro.Message);
            }
        }
    }

    public class LoteRecebidoComOcorrencia : DetalheArquivoCompe
    {
        public LoteRecebidoComOcorrencia(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 5;
                NOME_REGISTRO = "Detalhe Tipo 4 - Lote Recebido com Ocorrência";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco destinatário", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 7, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 10, 3, 1, "Local destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 13, 2, 1, "Tipo de ocorrência", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 15, 6, 1, "Quantidade apurada", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 21, 3, 1, "Quantidade informada", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 24, 17, 1, "Valor apurado", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 41, 17, 1, "Valor informado", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 58, 24, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 82, 7, 1, "Número do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 89, 26, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 115, 3, 1, "Ocorrência de erro", Ocorrencias.Descricao(Convert.ToInt16(linha.Substring(114, 3)))));
                CAMPOS.Add(new CampoArquivoCompe(15, 118, 10, 1, "Sequencial do erro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 128, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 131, 3, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(18, 134, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(19, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Lotes Recebidos com Ocorrência: " + erro.Message);
            }
        }
    }

    public class DetalheAReceberRecusado : DetalheArquivoCompe
    {
        public DetalheAReceberRecusado(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 6;
                NOME_REGISTRO = "Detalhe Tipo 4 - Detalhe a Receber Recusado";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco destinatário", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 7, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 10, 3, 1, "Local destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 13, 2, 1, "Tipo de ocorrência", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 15, 3, 1, "Local destino do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 18, 3, 1, "Banco do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 21, 4, 1, "Agência do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 25, 1, 1, "DV1", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 26, 12, 1, "Conta do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 38, 1, 1, "DV2", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 39, 6, 1, "Número do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 45, 1, 1, "DV3", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(15, 46, 2, 0, "UF", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(16, 48, 17, 1, "Valor do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 65, 2, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(18, 67, 1, 0, "Tipificação do cheque", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(19, 68, 3, 1, "Banco apresentante da troca", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(20, 71, 4, 1, "Agência apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(21, 75, 7, 1, "Número do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(22, 82, 3, 1, "Número sequencial do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(23, 85, 8, 1, "Data da apresentação", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(24, 93, 6, 0, "Centro processador do lote", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(25, 99, 12, 1, "Conta de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(26, 111, 4, 1, "Agência de depósito", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(27, 115, 3, 1, "Ocorrência de erro", Ocorrencias.Descricao(Convert.ToInt16(linha.Substring(114, 3)))));
                CAMPOS.Add(new CampoArquivoCompe(28, 118, 10, 1, "Sequencial do erro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(29, 128, 3, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(30, 131, 3, 1, "Tipo de documento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(31, 134, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(32, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Detalhes a Receber Recusados: " + erro.Message);
            }
        }
    }

    public class OcorrenciasHeadereTrailer : DetalheArquivoCompe
    {
        public OcorrenciasHeadereTrailer(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 7;
                NOME_REGISTRO = "Detalhe Tipo 5 - Ocorrências de Header e Trailer";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 2, 3, 1, "Banco apresentante", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 5, 1, 1, "Indicador de remessa", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 6, 7, 1, "Número da versão", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 13, 2, 1, "Número da prévia de processamento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 15, 8, 1, "Data do movimento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 23, 10, 1, "Quantidade de registros", ref linha));

                for (int i = 1; i <= 20; i++)
                {
                    int indice = 32 + 3 * (i - 1);

                    if (linha.Substring(indice, 3) != "   ")
                        CAMPOS.Add(new CampoArquivoCompe(i + 7, indice + 1
                            , 3, 1, "Ocorrência", Ocorrencias.Descricao(Convert.ToInt16(linha.Substring(indice, 3)))));
                }

                CAMPOS.Add(new CampoArquivoCompe(10, 93, 42, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 135, 6, 1, "Sequencial de arquivo", ref linha));

            }
            catch (Exception erro)
            {
                throw new ApplicationException("Ocorrências de Header e Trailer: " + erro.Message);
            }
        }
    }

    public class VersoesProcessadas : DetalheArquivoCompe
    {
        public VersoesProcessadas(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 8;
                NOME_REGISTRO = "Versões Processadas";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 1, 1, "Tipo de registro", ref linha));

                for (int i = 1; i <= 11; i++)
                {
                    int indice = 1 + 12 * (i - 1);
                    int numeroCampo = 2;

                    if (linha.Substring(indice, 12) != "            ")
                    {
                        CAMPOS.Add(new CampoArquivoCompe(numeroCampo, indice + 1
                            , 3, 1, "Banco apresentante", ref linha));
                        CAMPOS.Add(new CampoArquivoCompe(numeroCampo + 1, indice + 4
                            , 2, 1, "Indicador de remessa", ref linha));
                        CAMPOS.Add(new CampoArquivoCompe(numeroCampo + 2,indice + 6
                            , 7, 1, "Número da versão", ref linha));
                    }

                        numeroCampo = numeroCampo + 3;
                }

                CAMPOS.Add(new CampoArquivoCompe(16, 134, 1, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(17, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Versões Processadas: " + erro.Message);
            }
        }
    }

    public class TrailerCEL674 : DetalheArquivoCompe
    {
        public TrailerCEL674(string linha, int numeroLinha, int quebra, int tamanhoTotal, SolidColorBrush cor)
        {
            try
            {
                Text = linha;
                NUMERO_LINHA = numeroLinha;
                QUEBRA = quebra;
                TAMANHO_TOTAL_LINHA = tamanhoTotal;

                Foreground = cor;

                TIPO = 0;
                NOME_REGISTRO = "Trailer CEL NRA 674";
                CAMPOS = new List<CampoArquivoCompe>();

                CAMPOS.Add(new CampoArquivoCompe(1, 1, 9, 0, "Tipo de registro", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(2, 10, 6, 0, "Nome do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(3, 16, 3, 1, "Banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(4, 19, 1, 0, "Dígito verificador do código do banco destino", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(5, 20, 1, 1, "Indicador de remessa", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(6, 21, 4, 1, "Versão do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(7, 25, 3, 1, "Número da prévia de processamento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(8, 28, 8, 1, "Data do movimento", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(9, 36, 8, 1, "Data da criação do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(10, 44, 4, 1, "Hora da criação do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(11, 48, 5, 0, "Origem do arquivo", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(12, 53, 3, 0, "Flag fim", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(13, 56, 79, 2, "Filler", ref linha));
                CAMPOS.Add(new CampoArquivoCompe(14, 135, 6, 1, "Sequencial de arquivo", ref linha));
            }
            catch (Exception erro)
            {
                throw new ApplicationException("Trailer: " + erro.Message);
            }
        }
    }

    public static class Ocorrencias
    {
        static Dictionary<int, string> OCORRENCIA;

        public static void InicializaOcorrencias()
        {
            OCORRENCIA = new Dictionary<int, string>();

            OCORRENCIA.Add(100, "Arquivo aceito");
            OCORRENCIA.Add(101, "Arquivo sem header");
            OCORRENCIA.Add(102, "Banco inválido");
            OCORRENCIA.Add(103, "Banco não participa");
            OCORRENCIA.Add(104, "Banco suspenso");
            OCORRENCIA.Add(105, "Arquivo inválido");
            OCORRENCIA.Add(106, "Remessa inválida");
            OCORRENCIA.Add(107, "Compe inválida");
            OCORRENCIA.Add(108, "Versão inválida");
            OCORRENCIA.Add(109, "Versão duplicada");
            OCORRENCIA.Add(111, "Data inválida");
            OCORRENCIA.Add(112, "Sequencial inválido");

            OCORRENCIA.Add(901, "Arquivo sem trailer");
            OCORRENCIA.Add(902, "Arquivo só com trailer");
            OCORRENCIA.Add(903, "Trailer diverge do header");
            OCORRENCIA.Add(904, "Valor do arquivo inválido");
            OCORRENCIA.Add(905, "Sequencial inválido");

            OCORRENCIA.Add(701, "Compe destino inválida");
            OCORRENCIA.Add(702, "Banco destino inválido");
            OCORRENCIA.Add(703, "Banco destino excluído");
            OCORRENCIA.Add(704, "Banco destino suspenso");
            OCORRENCIA.Add(705, "Banco destino não participa da câmara");
            OCORRENCIA.Add(706, "Valor do lote inválido");
            OCORRENCIA.Add(707, "Banco apresentante inválido");
            OCORRENCIA.Add(708, "Banco apresentante divergente");
            OCORRENCIA.Add(709, "Banco apresentante excluído");
            OCORRENCIA.Add(710, "Banco apresentante suspenso");
            OCORRENCIA.Add(711, "Banco apresentante não participa da câmara");
            OCORRENCIA.Add(712, "Data inválida");
            OCORRENCIA.Add(713, "Número do lote inválido");
            OCORRENCIA.Add(714, "Lote duplicado");
            OCORRENCIA.Add(715, "Número sequencial do lote inválido");
            OCORRENCIA.Add(716, "Compe inválida");
            OCORRENCIA.Add(717, "Versão inválida");
            OCORRENCIA.Add(718, "TD inválido");
            OCORRENCIA.Add(719, "TD incompatível");
            OCORRENCIA.Add(720, "Lote sem detalhe");
            OCORRENCIA.Add(721, "Lote sem fechamento");
            OCORRENCIA.Add(722, "Lote com mais de 400 detalhes");
            OCORRENCIA.Add(723, "Sequencial inválido");

            OCORRENCIA.Add(201, "Compe destino inválida");
            OCORRENCIA.Add(202, "Banco destino inválido");
            OCORRENCIA.Add(203, "Agência destino inválida");
            OCORRENCIA.Add(204, "DV2 inválido");
            OCORRENCIA.Add(205, "Conta destino inválida");
            OCORRENCIA.Add(206, "DV1 inválido");
            OCORRENCIA.Add(207, "Número do documento inválido");
            OCORRENCIA.Add(208, "DV3 inválido");
            OCORRENCIA.Add(209, "UF inválida");
            OCORRENCIA.Add(210, "Valor inválido");
            OCORRENCIA.Add(211, "Valor incompatível");
            OCORRENCIA.Add(212, "Tipificação inválida");
            OCORRENCIA.Add(230, "Tipo de conta de depósito inválido");
            OCORRENCIA.Add(231, "Tipo de conta de depósito inválido");
            OCORRENCIA.Add(229, "Motivo de devolução inválido");
            OCORRENCIA.Add(213, "Banco apresentante inválido");
            OCORRENCIA.Add(214, "Agência apresentante inválida");
            OCORRENCIA.Add(215, "Número da agência depósito inválido");
            OCORRENCIA.Add(216, "Número da conta depósito inválido");
            OCORRENCIA.Add(217, "Código do local acolhimento inválido");
            OCORRENCIA.Add(218, "Data inválida");
            OCORRENCIA.Add(219, "Número do lote inválido");
            OCORRENCIA.Add(220, "Número sequencial do lote inválido");
            OCORRENCIA.Add(221, "Número identificador inválido");
            OCORRENCIA.Add(222, "Compe inválida");
            OCORRENCIA.Add(223, "Versão inválida");
            OCORRENCIA.Add(224, "TD inválido");
            OCORRENCIA.Add(225, "TD incompatível");
            OCORRENCIA.Add(226, "Caracteres binários");
            OCORRENCIA.Add(227, "Sequencial inválido");

            OCORRENCIA.Add(301, "Agência destino inválida");
            OCORRENCIA.Add(302, "Agência apresentante inválida");
            OCORRENCIA.Add(303, "Agência apresentante inativa");
            OCORRENCIA.Add(304, "Valor incompatível com TD VLB");
            OCORRENCIA.Add(305, "TD incompatível com VLB");
            OCORRENCIA.Add(306, "TD VLB incompatível");
            OCORRENCIA.Add(307, "Devolução VLB fora do prazo");
            OCORRENCIA.Add(308, "Devolução VLB duplicada");

            OCORRENCIA.Add(401, "Total de registros inválido");
            OCORRENCIA.Add(402, "Sequencial da imagem inválido");
            OCORRENCIA.Add(403, "Sequencial da imagem inválido");
            OCORRENCIA.Add(404, "Tamanho da imagem inválida");
            OCORRENCIA.Add(405, "Tamanho da assinatura inválido");
            OCORRENCIA.Add(406, "Total de registros incompatível com o tamanho");
            OCORRENCIA.Add(407, "Indicador do tipo de imagem inválido");
            OCORRENCIA.Add(408, "Indicador do tipo de imagem inválido");
            OCORRENCIA.Add(409, "Indicador do tipo de imagem inválido");
            OCORRENCIA.Add(410, "Indicador do tipo de imagem inválido");
        }

        public static string Descricao(int codigo)
        {
            string tipo_Ocorrencia = string.Empty;

            switch(codigo.ToString().Substring(0,1))
            {
                case "1":
                    tipo_Ocorrencia = "Ocorrência de Header";
                    break;
                case "9":
                    tipo_Ocorrencia = "Ocorrência de Trailer";
                    break;
                case "7":
                    tipo_Ocorrencia = "Ocorrência de Fechamento de Lote";
                    break;
                case "2":
                    tipo_Ocorrencia = "Ocorrência de Detalhe";
                    break;
                case "3":
                    tipo_Ocorrencia = "Ocorrência de Detalhe VLB";
                    break;
                case "4":
                    tipo_Ocorrencia = "Ocorrência de Detalhe Imagem";
                    break;
            }

            return new StringBuilder(codigo.ToString()).Append(" - ").Append(tipo_Ocorrencia).Append(": ").Append(OCORRENCIA[codigo]).ToString();
        }
    }
}
