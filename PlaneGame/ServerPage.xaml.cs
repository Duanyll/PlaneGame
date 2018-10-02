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
using MaterialDesignThemes.Wpf;
using GameruleHandler;

namespace PlaneGame
{
    /// <summary>
    /// ServerPage.xaml 的交互逻辑
    /// </summary>
    public partial class ServerPage : Page
    {
        public ServerPage(GameInfo info)
        {
            InitializeComponent();
            Gamerule = new ClassicGamerule(info);
            Gamerule.Log += (str) =>
            {
                Dispatcher.Invoke(() =>
                {
                    StackPanel stack = new StackPanel();
                    stack.Children.Add(new TextBlock()
                    {
                        Text = DateTime.Now.ToString(),
                        Style = (Style)Resources["MaterialDesignBody2TextBlock"]
                    });
                    stack.Children.Add(new TextBlock()
                    {
                        Text = str,
                        Style = (Style)Resources["MaterialDesignBody1TextBlock"]
                    });
                    ICLog.Items.Add(stack);
                });
            };
            Gamerule.StartGame();
            LBUser.ItemsSource = Gamerule.OnlinePlayers;
        }

        public ClassicGamerule Gamerule;

        private void BtnOpenClient_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window()
            {
                MinHeight = 450,
                MinWidth = 600,
            };
            ClientPage page = new ClientPage();
            page.PageExitEvent += (s, args) =>
            {
                window.Close();
            };
            page.BtnBack.Visibility = Visibility.Collapsed;
            window.Content = page;
            window.Closing += (s, args) =>
            {
                if (window.Content is ClientPage p)
                {
                    if (MaterialDesignMessageBox.Show("是否要退出游戏？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        args.Cancel = true;
                    }
                    else
                    {
                        p.client.Stop();
                    }
                }
            };
            window.Show();
        }
    }
}
