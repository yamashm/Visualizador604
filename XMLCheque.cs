using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Visualizador604
{
    public static class XMLCheque
    {
        public static bool Gera(string pathentra, string pathsai, string agencia, string numero)
        {
            ArrayList imagens = new ArrayList();

            imagens = ListaArquivosTif(@pathentra);

            if (imagens.Count == 0)
                return false;

            foreach (FileInfo imgnome in imagens)
            {
                if (!imgnome.Name.Contains("VERSO"))
                {
                    StringBuilder sb = new StringBuilder();
                    byte[] frente = null;
                    byte[] verso = null;
                    DateTime criacao = imgnome.CreationTime;

                    sb.Append(pathentra).Append("\\").Append(imgnome.Name);
                    if(File.Exists(sb.ToString()))
                        frente = File.ReadAllBytes(sb.ToString());

                    sb.Clear();
                    sb.Append(pathentra).Append("\\").Append(imgnome.Name.Insert(imgnome.Name.Length - 4, "VERSO"));
                    if(File.Exists(sb.ToString()))
                        verso = File.ReadAllBytes(sb.ToString());

                    if (!GeraXML(imgnome.Name.Substring(0, imgnome.Name.Length - 4), frente, verso, pathsai, agencia, numero))
                        return false;
                }
            }

            return true;
        }

        public static bool Le(string path, out string cmc7,ref byte[] frente, ref byte[] verso)
        {
            cmc7 = "";
            XmlDocument cheque = new XmlDocument();

            if (File.Exists(path))
            {
                cheque.Load(path);

                try
                {
                    cmc7 = cheque.SelectSingleNode("//Cheque//CMC7").InnerText;
                    frente = Convert.FromBase64String(cheque.SelectSingleNode("//Cheque//Frente").InnerText);
                    verso = Convert.FromBase64String(cheque.SelectSingleNode("//Cheque//Verso").InnerText);
                }
                catch
                {
                    return false;
                }

                return true;
            }
            else
                return false;
        }

        public static bool LeInfos(string path, out string agencia, out string terminal, out string passada,
   out string tipocheque, out string datamovimento, out string cmc7, out string statuscmc7, out string valor, out string canal)
        {
            agencia = "";
            terminal = "";
            passada = "";
            tipocheque = "";
            datamovimento = "";
            cmc7 = "";
            statuscmc7 = "";
            valor = "";
            canal = "";

            XmlDocument cheque = new XmlDocument();

            try
            {
                if (File.Exists(path))
                {
                    cheque.Load(path);

                    agencia = cheque.SelectSingleNode("//Cheque//Agencia").InnerText;
                    terminal = cheque.SelectSingleNode("//Cheque//Terminal").InnerText;
                    passada = cheque.SelectSingleNode("//Cheque//Passada").InnerText;
                    tipocheque = cheque.SelectSingleNode("//Cheque//TipoCheque").InnerText;
                    datamovimento = cheque.SelectSingleNode("//Cheque//DataMovimento").InnerText;
                    cmc7 = cheque.SelectSingleNode("//Cheque//CMC7").InnerText;
                    statuscmc7 = cheque.SelectSingleNode("//Cheque//StatusCMC7").InnerText;
                    valor = cheque.SelectSingleNode("//Cheque//Valor").InnerText;

                    if (cheque.SelectSingleNode("//Cheque//Canal") != null)
                    {
                        canal = cheque.SelectSingleNode("//Cheque//Canal").InnerText;
                    }
                    else
                        canal = "0";

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static byte[] ObterBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        private static ArrayList ListaArquivosTif(string path)
        {
            ArrayList resultado = new ArrayList();
            DirectoryInfo d = new DirectoryInfo(path);

            foreach (FileInfo f in d.GetFiles())
            {
                if(f.Extension == ".tif")
                    resultado.Add(f);
            }

            return resultado;
        }

        private static bool GeraXML(string valorcmc7, byte[] valorfrente, byte[] valorverso, string pathsaida, string agencia, string numero)
        {
            bool retorno = true;

            XmlDocument cheque = new XmlDocument();

            XmlNode principal = cheque.CreateNode(XmlNodeType.Element, "Cheque", null);

            cheque.AppendChild(principal);

            XmlNode dadoscheque = cheque.SelectSingleNode("//Cheque");

            XmlNode cmc7 = cheque.CreateNode(XmlNodeType.Element, "CMC7", null);
            XmlNode frente = cheque.CreateNode(XmlNodeType.Element, "Frente", null);
            XmlNode verso = cheque.CreateNode(XmlNodeType.Element, "Verso", null);

            cmc7.InnerText = valorcmc7;
            if(valorfrente != null)
                frente.InnerText = Convert.ToBase64String(valorfrente);
            if(valorverso != null)
                verso.InnerText = Convert.ToBase64String(valorverso);

            dadoscheque.AppendChild(cmc7);
            dadoscheque.AppendChild(frente);
            dadoscheque.AppendChild(verso);

            StringBuilder nomearquivo = new StringBuilder();
            nomearquivo.Append(pathsaida).Append("\\");
            nomearquivo.Append(DateTime.Now.Year.ToString()).Append(DateTime.Now.Month.ToString().PadLeft(2, '0')).Append(DateTime.Now.Day.ToString().PadLeft(2, '0'));
            nomearquivo.Append(agencia.PadLeft(5, '0')).Append(numero.PadLeft(3, '0'));
            nomearquivo.Append(valorcmc7);
            nomearquivo.Append(".xml");

            try
            {
                cheque.Save(nomearquivo.ToString());
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }
    }
}
