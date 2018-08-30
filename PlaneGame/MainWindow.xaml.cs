using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ToHomePage();
        }

        private void ToHomePage()
        {
            HomePage page = new HomePage();
            page.BtnNewGame.Click += (s, args) =>
            {
                ToNewGamePage();
            };
            page.BtnSettings.Click += (s, args) =>
            {
                ToSettingsPage();
            };
            Content = page;
        }

        private void ToNewGamePage()
        {
            NewGamePage1 page = new NewGamePage1();
            page.BtnStartGame.Click += (s, args) =>
            {
                ToServerPage(page.Info);
            };
            Content = page;
        }

        private void ToServerPage(GameruleHandler.GameInfo info)
        {

        }

        private void ToSettingsPage()
        {
            SettingsPage page = new SettingsPage();
            Content = page;
        }
    }
}
