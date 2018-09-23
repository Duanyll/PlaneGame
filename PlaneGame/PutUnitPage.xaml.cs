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

namespace PlaneGame
{
    /// <summary>
    /// PutUnitPage.xaml 的交互逻辑
    /// </summary>
    public partial class PutUnitPage : Page
    {
        public PutUnitPage()
        {
            InitializeComponent();
            GVMain.ClickMode = GameBoardView.GameBoardClickMode.PutUnit;
            GVMain.Click += (x, y) =>
            {
                if (LBSelectUnit.SelectedItem != null)
                {
                    Click?.Invoke(x, y, (LBSelectUnit.SelectedItem as Badged).Content as string);
                }
            };
        }

        public GameBoard.FullPlayerGameBoard board
        {
            set
            {
                GVMain.Board = value;
            }
        }
        public event Action<int, int,string> Click;
        Dictionary<string, Badged> UnitLabels = new Dictionary<string, Badged>();

        public void UpdateUnit(string unit,int count)
        {
            if(UnitLabels.TryGetValue(unit,out Badged badged))
            {
                badged.Badge = count;
            }
            else
            {
                badged = new Badged()
                {
                    Badge = count,
                    Content = unit
                };
                UnitLabels.Add(unit, badged);
                LBSelectUnit.Items.Add(badged);
            }
        }
    }
}
