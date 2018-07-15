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

using static CSSWizard.Color;

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
            if (SourceBox.Text.Length == 6)
            {
                RgbColor col = SourceBox.Text.StringToRgbColor();

                ResultBox.Text += col.ToString() + Environment.NewLine;
                ResultBox.Text += col.RgbToRgba().ToString(false) + Environment.NewLine;
                ResultBox.Text += col.RgbToHsv().ToString() + Environment.NewLine;
                ResultBox.Text += Environment.NewLine;
            }
            else if (SourceBox.Text.Length == 8)
            {
                RgbaColor col = SourceBox.Text.StringToRgbaColor();
                Title = (col.Alpha * 255).ToString();

                ResultBox.Text += col.ToString() + Environment.NewLine;
                ResultBox.Text += col.ToString(false) + Environment.NewLine;
                ResultBox.Text += col.RgbaToRgb().ToString() + Environment.NewLine;
                ResultBox.Text += col.RgbaToHsv().ToString() + Environment.NewLine;
                ResultBox.Text += Environment.NewLine;
            }
        }
    }
}
