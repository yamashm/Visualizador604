using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Visualizador604
{
    /// <summary>
    /// Interaction logic for ValoresLotes.xaml
    /// </summary>
    public partial class DadosLotes : Window
    {
        public DadosLotes(List<string> _DadosLotes)
        {
            InitializeComponent();

            StringBuilder sb = new StringBuilder();

            foreach (string s in _DadosLotes)
            {
                string[] dados = s.Split('&');
                string valor = (Convert.ToDecimal(dados[2])/100).ToString("N2");
                sb.Append("Lote: ").Append(dados[0]).Append("\t").Append("Nº do Lote: ").Append(dados[1]).Append("\t").Append("Quantidade de cheques: ").Append(dados[3]).Append("\t\t").Append("Valor: ").Append(valor).Append("\r\n");
            }

            txbDadosLotes.Text = sb.ToString();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
