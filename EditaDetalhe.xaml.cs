using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Drawing;
using System.Windows.Interop;

namespace Visualizador604
{
    /// <summary>
    /// Interaction logic for EditaDetalhe.xaml
    /// </summary>
    public partial class EditaDetalhe : Window
    {
        DetalheArquivoCompe _detalhe;
        DataTable _dt;

        public EditaDetalhe(DetalheArquivoCompe registro)
        {
            InitializeComponent();

            _dt = new DataTable();

            _dt.Columns.Add("Campo", typeof(int));
            _dt.Columns.Add("Descrição", typeof(string));
            _dt.Columns.Add("Posição", typeof(string));
            _dt.Columns.Add("Valor", typeof(string));

            _detalhe = registro;

            this.Title = _detalhe.NOME_REGISTRO;

            foreach (CampoArquivoCompe c in _detalhe.CAMPOS)
            {
                if (c.PERMITE_EDICAO_INDIVIDUAL) 
                {
                    _dt.Rows.Add(c.NUMERO, c.DESCRICAO, c.POSICAO, c.CONTEUDO);
                }
            }

            dtgDetalhes.ItemsSource = _dt.DefaultView;

            dtgDetalhes.Focus(); 
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void btnRegistraAlteracao_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, string> listaIndicesValores = new Dictionary<int, string>();

            foreach (DataRowView dv in dtgDetalhes.Items.SourceCollection)
            {
                listaIndicesValores.Add(Convert.ToInt32(dv.Row[0]), dv.Row[3].ToString());
            }

            StringBuilder sb = new StringBuilder();

            foreach (int i in listaIndicesValores.Keys)
            {
                CampoArquivoCompe campo = _detalhe.CAMPOS.Find(x => x.NUMERO == i);

                if (listaIndicesValores[i].Length > campo.TAMANHO)
                    campo.CONTEUDO = listaIndicesValores[i].Substring(0, campo.TAMANHO);
                else
                    campo.CONTEUDO = listaIndicesValores[i];

                if (campo.CONTEUDO != _detalhe.CAMPOS.Find(x => x.NUMERO == i).CONTEUDO)
                {
                    _detalhe.CAMPOS.Remove(campo);
                    _detalhe.CAMPOS.Insert(i - 1, campo);
                }
            }

            _detalhe.RegeraLinha();

            this.DialogResult = true;
        }
    }
}
