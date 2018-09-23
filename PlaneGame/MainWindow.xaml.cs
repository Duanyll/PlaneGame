﻿using System;
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
            new MaterialDesignThemes.Wpf.PaletteHelper().SetLightDark(Properties.Settings.Default.UseDarkMode);
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
            page.BtnJoinGame.Click += (s, args) =>
            {
                ToJoinGamePage();
            };
            Content = page;
        }

        private void ToNewGamePage()
        {
            NewGamePage1 page = new NewGamePage1();
            page.BtnStartGame.Click += (s, args) =>
            {
                foreach(var i in page.SPRoundOrder.Children)
                {
                    if(i is FrameworkElement e)
                    {
                        page.Info.RoundOrder = (GameruleHandler.GameInfo.RoundOrderType)Enum.Parse(page.Info.RoundOrder.GetType(), e.Tag as string);
                    }
                }
                ToServerPage(page.Info);
            };
            page.BtnBack.Click += (s, args) =>
            {
                ToHomePage();
            };
            Content = page;
        }

        private void ToServerPage(GameruleHandler.GameInfo info)
        {
            ServerPage page = new ServerPage(info);
            page.BtnBack.Click += (s, args) =>
            {
                page.Gamerule.AbortGame();
                ToHomePage();
            };
            Content = page;
        }

        private void ToSettingsPage()
        {
            SettingsPage page = new SettingsPage();
            page.BtnBack.Click += (s, args) =>
            {
                ToHomePage();
            };
            Content = page;
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
            Content = page;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Content is ServerPage page)
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
                if(Content is ClientPage p)
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
