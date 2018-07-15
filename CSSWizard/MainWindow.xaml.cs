using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

using static CSSWizard.Colors;

namespace CSSWizard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void CalculateBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var mCol = SourceBox.Text.HexToRgb();

            ResultBox.Text += mCol.RgbToHex() + Environment.NewLine;
            ResultBox.Text += mCol.RgbToHsb().HsbToString() + Environment.NewLine;
            ResultBox.Text += mCol.RgbToHsb().InvertValue().HsbToString() + Environment.NewLine;
            ResultBox.Text += mCol.RgbaToString() + Environment.NewLine;
            ResultBox.Text += mCol.RgbToHsb().HsbToRgb().RgbToHex() + Environment.NewLine;
        }
    }
}
