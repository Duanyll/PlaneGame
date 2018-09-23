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
    /// TeamSelectPage.xaml 的交互逻辑
    /// </summary>
    public partial class TeamSelectPage : Page
    {
        public event Action<char> Selected;
        public TeamSelectPage(string Teams)
        {
            InitializeComponent();
            foreach(var i in Teams)
            {
                Button Btn = new Button()
                {
                    Style = App.Current.Resources["MaterialDesignFlatButton"] as Style,
                    Content = "Team "+i,
                    Tag = i,
                };
                Btn.Click += Button_Click;
                WrPTeams.Children.Add(Btn);
            }
        }

        void Button_Click(object sender,RoutedEventArgs e)
        {
            if(sender is FrameworkElement element)
            {
                if(element.Tag is char ch)
                {
                    Selected?.Invoke(ch);
                    WrPTeams.IsEnabled = false;
                }
            }
        }
    }
}
