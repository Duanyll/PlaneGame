﻿using System;
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
using GameruleHandler;
using MaterialDesignThemes.Wpf;
using NetClient;

namespace PlaneGame
{
    /// <summary>
    /// ClientPage.xaml 的交互逻辑
    /// </summary>
    public partial class ClientPage : Page
    {
        public NetworkClient client { get; private set; } = new NetworkClient();
        public event RoutedEventHandler PageExitEvent;
        public ClientPage()
        {
            InitializeComponent();
            SnbMain.MessageQueue = new SnackbarMessageQueue(Properties.Settings.Default.SnakeBarMessageDuration);
            client.FailureCaused += Client_FailureCaused;
            client.MessageRecieved += Client_MessageRecieved;
            client.Info += Client_Info;
            Loaded += (s, args) =>
            {
                ConnectToServerWindow window = new ConnectToServerWindow(client);
                window.Owner = Application.Current.MainWindow;
                if (!window.ShowDialog())
                {
                    PageExitEvent?.Invoke(this,null);
                }
            };
        }

        private void Client_Info(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                SnbMain.MessageQueue.Enqueue(msg);
            });            
        }

        private void Client_MessageRecieved(string msg)
        {
            //throw new NotImplementedException();
            Dispatcher.Invoke(() =>
            {
                string[] vs = msg.Split('|');
                try
                {
                    switch (vs[0])
                    {
                        case "CHAT":
                            HandleChat(vs[1], vs[2]);
                            break;
                        case "SLOG":
                            HandleChat("", vs[1]);
                            break;
                        case "SBAR":
                            SnbMain.MessageQueue.Enqueue(vs[1]);
                            break;
                        case "ULGI":
                            WrPUsers.Children.Add(new Chip()
                            {
                                Content = vs[1]
                            });
                            break;
                        case "ULGO":
                            foreach (UIElement i in WrPUsers.Children)
                            {
                                if (i is Chip ch)
                                {
                                    if (ch.Content is string str)
                                    {
                                        if (str == vs[1])
                                        {
                                            WrPUsers.Children.Remove(i);
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        case "MBOX":
                            MessageBox.Show(vs[1], "PlaneGame", MessageBoxButton.OK);
                            break;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    SnbMain.MessageQueue.Enqueue("收到无效信息，可能网络不稳定", "查看详情", () =>
                    {
                        LongTextDisplayWindow window = new LongTextDisplayWindow(msg + Environment.NewLine + e.Message);
                        window.Show();
                    }, false);
                }
            });
        }

        private void HandleChat(string UserName, string msg)
        {
            SPChat.Children.Add(new TextBlock()
            {
                Text = UserName + " " + DateTime.Now.ToString(),
                Style = Resources["MaterialDesignBody2TextBlock"] as Style,
                TextWrapping = TextWrapping.Wrap
            });
            SPChat.Children.Add(new TextBlock()
            {
                Text = msg,
                Style = Resources["MaterialDesignBody1TextBlock"] as Style,
                TextWrapping = TextWrapping.Wrap
            });
        }

        private void Client_FailureCaused(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                SnbMain.MessageQueue.Enqueue(msg);
            });
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            client.Stop();
        }

        private void BtnSendChat_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage("CHAT|" + TBSendMessage.Text.Trim());
        }

        private void BtnClearChat_Click(object sender, RoutedEventArgs e)
        {
            SPChat.Children.Clear();
        }
    }
}
