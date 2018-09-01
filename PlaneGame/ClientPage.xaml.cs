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
            //throw new NotImplementedException();
        }

        private void Client_MessageRecieved(string msg)
        {
            //throw new NotImplementedException();
        }

        private void Client_FailureCaused(string msg)
        {
            //throw new NotImplementedException();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            client.Stop();
        }
    }
}
