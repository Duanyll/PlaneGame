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
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;

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
            new PaletteHelper().SetLightDark(Properties.Settings.Default.UseDarkMode);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ToHomePage();
        }

        Page _pge;
        Page NowPage
        {
            get => _pge;
            set
            {
                _pge = value;
                //Content = value;
                TransitionerSlide slide = new TransitionerSlide();
                slide.Content = new Frame()
                {
                    Content = value
                };
                TstMain.Items.Add(slide);
                TstMain.SelectedIndex++;
                if (TstMain.Items.Count > 3)
                {
                    TstMain.Items.RemoveAt(0);
                }
            }
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
            page.BtnJoinGame.Click += (s, args) =>
            {
                ToJoinGamePage();
            };
            NowPage = page;
        }

        private void ToNewGamePage()
        {
            NewGamePage1 page = new NewGamePage1();
            page.BtnStartGame.Click += (s, args) =>
            {
                ToServerPage(page.Info);
            };
            page.BtnBack.Click += (s, args) =>
            {
                ToHomePage();
            };
            NowPage = page;
        }

        private void ToServerPage(GameruleHandler.GameInfo info)
        {
            ServerPage page = new ServerPage(info);
            page.BtnBack.Click += (s, args) =>
            {
                page.Gamerule.AbortGame();
                ToHomePage();
            };
            NowPage = page;
        }

        private void ToSettingsPage()
        {
            SettingsPage page = new SettingsPage();
            page.BtnBack.Click += (s, args) =>
            {
                ToHomePage();
            };
            NowPage = page;
        }

        private void ToJoinGamePage()
        {
            ClientPage page = new ClientPage();
            page.BtnBack.Click += (s, args) =>
            {
                ToHomePage();
            };
            page.PageExitEvent += (s, args) =>
            {
                ToHomePage();
            };
            NowPage = page;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {            
            if(NowPage is ServerPage page)
            {
                if(MaterialDesignMessageBox.Show("是否要关闭运行中的服务器？","提示",MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    page.Gamerule.AbortGame();
                }
            }
            else
            {
                if(NowPage is ClientPage p)
                {
                    if (MaterialDesignMessageBox.Show("是否要退出游戏？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        p.client.Stop();
                    }
                }
            }
        }
    }
}
