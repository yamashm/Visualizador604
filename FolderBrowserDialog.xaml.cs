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
    /// Interaction logic for FolderBrowserDialog.xaml
    /// </summary>
    public partial class FolderBrowserDialog : Window
    {
        //private BrowserViewModel _viewModel;

        //public BrowserViewModel ViewModel
        //{
        //    get
        //    {
        //        return _viewModel = _viewModel ?? new BrowserViewModel();
        //    }
        //}

        //public string SelectedFolder
        //{
        //    get
        //    {
        //        return ViewModel.SelectedFolder;
        //    }
        //}

        public FolderBrowserDialog(List<Dado604> Lista)
        {
            InitializeComponent();

        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.DialogResult = false;
            else if(e.Key == Key.Enter)
                this.DialogResult = true;
        }

        private void imgCabecalho_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
            else
                e.Handled = true;

        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
