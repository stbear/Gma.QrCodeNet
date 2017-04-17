using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gma.QrCode.Net.Silverlight.Demo
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            //qrCodeImgControl1.Text = textBox1.Text;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            qrCodeGeoControl1.Text = textBox1.Text;
        }

    }
}
