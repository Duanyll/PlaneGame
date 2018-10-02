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
                    Click?.Invoke(x, y, (LBSelectUnit.SelectedItem as Badged).Content as string,((int)(GVUnit.Board as GameBoard.PatternGameBoard).Flip).ToString(), ((int)(GVUnit.Board as GameBoard.PatternGameBoard).Roation).ToString());
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
        public event Action<int, int,string,string,string> Click;
        Dictionary<string, Badged> UnitLabels = new Dictionary<string, Badged>();
        Dictionary<string, GameBoard.PatternGameBoard> UnitPreview = new Dictionary<string, GameBoard.PatternGameBoard>();

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

        public void UpdateUnit(string unit,int count,string unitDetail)
        {
            UpdateUnit(unit, count);
            if (!UnitPreview.Keys.Contains(unit))
            {
                UnitPreview.Add(unit, new GameBoard.PatternGameBoard(unitDetail.Split('\n')));
            }
        }

        private void LBSelectUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBSelectUnit.SelectedItem != null)
            {
                GVUnit.Board = UnitPreview[(LBSelectUnit.SelectedItem as Badged).Content as string];
            }
        }

        private void BtnRotate_Click(object sender, RoutedEventArgs e)
        {
            if(GVUnit.Board == null)
            {
                return;
            }
            (GVUnit.Board as GameBoard.PatternGameBoard).Roation += 1;
            if((int)(GVUnit.Board as GameBoard.PatternGameBoard).Roation == 4)
            {
                (GVUnit.Board as GameBoard.PatternGameBoard).Roation = GameBoard.PatternGameBoard.RoationMode.None;
            }
            GVUnit.Refresh();
        }

        private void BtnXFlip_Click(object sender, RoutedEventArgs e)
        {
            if (GVUnit.Board == null)
            {
                return;
            }
            (GVUnit.Board as GameBoard.PatternGameBoard).Flip ^= GameBoard.PatternGameBoard.FlipMode.FlipX;
            GVUnit.Refresh();
        }

        private void BtnYFlip_Click(object sender, RoutedEventArgs e)
        {
            if (GVUnit.Board == null)
            {
                return;
            }
            (GVUnit.Board as GameBoard.PatternGameBoard).Flip ^= GameBoard.PatternGameBoard.FlipMode.FlipY;
            GVUnit.Refresh();
        }
    }
}
