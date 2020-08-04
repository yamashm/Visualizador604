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
    /// Interaction logic for Pesquisa.xaml
    /// </summary>
    public partial class Pesquisa : Window
    {
        //List<string> _Lista;
        ArquivoCELNRA604 _registros604;

        bool _alterou;
        int _indicepesquisa;

        //List<Dado604> _dadospesquisados = null;

        public delegate void Proximo(List<int> ItemsEncontrados);
        public event Proximo OnProximo;

        public Pesquisa(Arquivo dados)
        {
            InitializeComponent();

            _registros604 = (ArquivoCELNRA604)dados;

            //_Lista = Lista;

            txbValor.Focus();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
            else if (e.Key == Key.Enter)
                EncontraProximo();
        }

        private void btnProximo_Click(object sender, RoutedEventArgs e)
        {
            EncontraProximo();
        }

        private void EncontraProximo()
        {
            if (txbValor.Text != string.Empty)
            {
                List<int> indicesEncontrados = new List<int>();

                if (_alterou)
                {
                    //_dadospesquisados = _Lista.FindAll(item => item.Text.Contains(txbValor.Text) || item.CMC7 == (txbValor.Text) || item.CRC == (txbValor.Text.ToUpper()));

                    if (_registros604.HEADER.Text.Contains(txbValor.Text))
                        indicesEncontrados.Add(_registros604.HEADER.NUMERO_LINHA- 1);

                    if(_registros604.TRAILER.Text.Contains(txbValor.Text))
                        indicesEncontrados.Add(_registros604.TRAILER.NUMERO_LINHA - 1);

                    foreach (LoteCEL604 l in _registros604.LOTES)
                    {
                        if(l.FECHAMENTO.Text.Contains(txbValor.Text))
                            indicesEncontrados.Add(l.FECHAMENTO.NUMERO_LINHA - 1);

                        foreach (Cheque c in l.CHEQUES)
                        {
                            foreach (DetalheArquivoCompe d in c.DADOS_FRENTE)
                            {
                                if(d.Text.Contains(txbValor.Text))
                                    indicesEncontrados.Add(d.NUMERO_LINHA - 1);
                            }

                            foreach (DetalheArquivoCompe d in c.DADOS_VERSO)
                            {
                                if (d.Text.Contains(txbValor.Text))
                                    indicesEncontrados.Add(d.NUMERO_LINHA - 1);
                            }
                        }
                    }

                    _indicepesquisa = 0;
                    _alterou = false;
                }

                if (indicesEncontrados.Count() > 0)
                {
                    if (_indicepesquisa == indicesEncontrados.Count())
                        _indicepesquisa = 0;

                    if (OnProximo != null)
                        OnProximo(indicesEncontrados);

                    _indicepesquisa++;
                }
            }
        }

        private void txbValor_TextChanged(object sender, TextChangedEventArgs e)
        {
            _alterou = true;
        }
    }
}
