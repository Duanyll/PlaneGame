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
using System.Windows.Shapes;
using NetClient;

namespace PlaneGame
{
    /// <summary>
    /// ConnectToServerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectToServerWindow : Window
    {
        NetworkClient client;
        bool IsConnected = false;
        public ConnectToServerWindow(NetworkClient client)
        {
            InitializeComponent();
            TBIPAddress.Text = Properties.Settings.Default.LastIPAddress;
            TBUserName.Text = Properties.Settings.Default.LastUserName;
            this.client = client;
            client.Connected += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    Properties.Settings.Default.LastIPAddress = TBIPAddress.Text;
                    Properties.Settings.Default.LastUserName = TBUserName.Text;
                    Properties.Settings.Default.Save();
                    IsConnected = true;
                    Close();
                });
            };
            client.FailureCaused += (msg) =>
            {
                Dispatcher.Invoke(() =>
                {
                    BtnOK.IsEnabled = true;
                    TBResult.Text = msg;
                });
            };
        }

        /// <summary>
        /// 展示为对话框
        /// </summary>
        /// <returns>连接是否成功</returns>
        public new bool ShowDialog()
        {
            base.ShowDialog();
            return IsConnected;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            BtnOK.IsEnabled = false;
            client.Connect(TBUserName.Text, TBIPAddress.Text);
        }
    }
}
