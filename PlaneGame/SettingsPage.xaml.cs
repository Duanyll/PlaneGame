using System;
using System.Collections.Generic;
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

namespace PlaneGame
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            TgBDarkMode.IsChecked = Properties.Settings.Default.UseDarkMode;
            SlBlockSize.Value = Properties.Settings.Default.GBGridHeight;
        }

        private void TgBDarkMode_Click(object sender, RoutedEventArgs e)
        {
            new MaterialDesignThemes.Wpf.PaletteHelper().SetLightDark(TgBDarkMode.IsChecked.Value);
            Properties.Settings.Default.UseDarkMode = TgBDarkMode.IsChecked.Value;
            Properties.Settings.Default.Save();
        }

        private void BtnSaveBlockSize_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GBGridHeight = Properties.Settings.Default.GBGridWidth = SlBlockSize.Value;
            Properties.Settings.Default.Save();
        }
    }
}
