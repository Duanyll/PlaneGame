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
using MaterialDesignThemes.Wpf;

namespace PlaneGame
{
    /// <summary>
    /// MaterialDesignMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialDesignMessageBox : Window
    {
        public MaterialDesignMessageBox()
        {
            InitializeComponent();
        }

        MessageBoxResult result;

        public static MessageBoxResult Show(string content,string title = "提示",MessageBoxButton button = MessageBoxButton.OK,PackIconKind icon = PackIconKind.Information,Window owner = null)
        {
            MaterialDesignMessageBox messageBox = new MaterialDesignMessageBox();
            messageBox.TBContent.Text = content;
            messageBox.TBTitle.Text = title;
            messageBox.Owner = owner;
            if(messageBox.Owner == null)
            {
                messageBox.Owner = Application.Current.MainWindow;
            }
            switch (button)
            {
                case MessageBoxButton.OK:
                    messageBox.BtnCancel.Visibility = Visibility.Collapsed;
                    messageBox.BtnNo.Visibility = Visibility.Collapsed;
                    messageBox.BtnOK.Visibility = Visibility.Visible;
                    messageBox.BtnYes.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.OKCancel:
                    messageBox.BtnCancel.Visibility = Visibility.Visible;
                    messageBox.BtnNo.Visibility = Visibility.Collapsed;
                    messageBox.BtnOK.Visibility = Visibility.Visible;
                    messageBox.BtnYes.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNo:
                    messageBox.BtnCancel.Visibility = Visibility.Collapsed;
                    messageBox.BtnNo.Visibility = Visibility.Visible;
                    messageBox.BtnOK.Visibility = Visibility.Collapsed;
                    messageBox.BtnYes.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    messageBox.BtnCancel.Visibility = Visibility.Visible;
                    messageBox.BtnNo.Visibility = Visibility.Visible;
                    messageBox.BtnOK.Visibility = Visibility.Collapsed;
                    messageBox.BtnYes.Visibility = Visibility.Visible;
                    break;
            }
            messageBox.PIIcon.Kind = icon;
            messageBox.ShowDialog();
            return messageBox.result;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Cancel;
            Close();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.OK;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Yes;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.No;
            Close();
        }
    }
}
