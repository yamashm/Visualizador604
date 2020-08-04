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
    /// Interaction logic for Autenticacao.xaml
    /// </summary>
    public partial class Autenticacao : Window
    {
        string _ususario = "orion";
        string _senha = "Admin@123";

        public Autenticacao()
        {
            InitializeComponent();
        }

        private void btnConfirma_Click(object sender, RoutedEventArgs e)
        {
            Confirma();
        }

        private void btnCancela_Click(object sender, RoutedEventArgs e)
        {
            Cancela();
        }

        private void Confirma()
        {
            //if (txbUsuario.Text.ToLower() == _ususario && psbSenha.Password == _senha)
                this.DialogResult = true;
        }

        private void Cancela()
        {
            this.DialogResult = false;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    Confirma();
                    break;

                case Key.Escape:
                    Cancela();
                    break;
            }
        }
    }
}
