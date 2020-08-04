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
    public partial class ValoresLotes : Window
    {
        public ValoresLotes(List<string> _BancosValores)
        {
            InitializeComponent();

            StringBuilder sb = new StringBuilder();

            _BancosValores.Sort();

            List<short> Bancos = new List<short>();
            List<decimal> Valores = new List<decimal>();

            foreach (string s in _BancosValores)
            {
                Bancos.Add(Convert.ToInt16(s.Substring(0, 3)));
                Valores.Add(Convert.ToDecimal(s.Substring(3, 17)));
            }

            short aux = Bancos[0];

            Dictionary<short, decimal> ValorBanco = new Dictionary<short, decimal>();

            decimal valortotalbanco = 0;

            for (int x = 0; x < Bancos.Count(); x++)
            {
                if (Bancos[x] == aux)
                {
                    valortotalbanco = valortotalbanco + Valores[x];
                }
                else
                {
                    ValorBanco.Add(Bancos[x-1], valortotalbanco);
                    valortotalbanco = Valores[x];
                }

                if (Bancos.Count() - 1 == x)
                {
                    ValorBanco.Add(Bancos[x], valortotalbanco);
                }

                aux = Bancos[x];
            }

            foreach (var item in ValorBanco)
            {
                sb.Append("Banco: ").Append(item.Key.ToString()).Append("\t").Append("Valor: ").Append(item.Value.ToString().Insert(item.Value.ToString().Length-2,",")).Append("\r\n");
            }

            txbValoresLotes.Text = sb.ToString();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
