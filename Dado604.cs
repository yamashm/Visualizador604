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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Globalization;

namespace Visualizador604
{
    public class Dado604: TextBlock
    {
        public byte[] binImagemAss;
        public string CMC7;
        public string CRC;
        public int Linha;
        public byte[] binDados;
    }
}
