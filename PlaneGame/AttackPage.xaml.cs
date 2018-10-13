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
using System.Windows.Media.Animation;

namespace PlaneGame
{
    /// <summary>
    /// AttackPage.xaml 的交互逻辑
    /// </summary>
    public partial class AttackPage : Page
    {
        public AttackPage(GameBoard.FullPlayerGameBoard yourboard)
        {
            InitializeComponent();
            YourBoard = yourboard;
            GVMain.Board = YourBoard;
            LBTeam.Items.Add("Your Team");
        }

        public event Action<int, (int, int)> Attack;

        Dictionary<string, GameBoard.PatternGameBoard> _upr = new Dictionary<string, GameBoard.PatternGameBoard>();
        public Dictionary<string, GameBoard.PatternGameBoard> UnitPreview
        {
            get
            {
                return _upr;
            }
            set
            {
                _upr = value;
                foreach(var i in value.Keys)
                {
                    LBUnit.Items.Add(i);
                }
            }
        }

        private void LBUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBUnit.SelectedItem != null)
            {
                GVUnit.Board = UnitPreview[LBUnit.SelectedItem as string];
            }
        }

        public Dictionary<int, GameBoard.PlayerViewGameBoard> Boards = new Dictionary<int, GameBoard.PlayerViewGameBoard>();
        public GameBoard.FullPlayerGameBoard YourBoard;

        public void AddBoard(int Team,GameBoard.PlayerViewGameBoard board)
        {
            Boards.Add(Team, board);
            LBTeam.Items.Add("Team " + Team.ToString());
        }

        private void LBTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LBTeam.SelectedIndex == 0)
            {
                GVMain.Board = YourBoard;
                TBTeam.Text = "Your Team";
            }
            else
            {
                if (LBTeam.SelectedIndex != -1)
                {
                    GVMain.Board = Boards[LBTeam.SelectedIndex - 1];
                    TBTeam.Text = "Team " + LBTeam.SelectedIndex.ToString();
                }
            }
        }

        Storyboard SBTimer = new Storyboard();

        public void StartGetPoint(int count,int time)
        {
            BtnAttack.IsEnabled = true;
            BdgFireCount.Badge = count;
            SBTimer = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation(0, 100, new Duration(TimeSpan.FromSeconds(time)));
            Storyboard.SetTarget(animation, PrBTimer);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(ProgressBar.Value)"));
            SBTimer.Children.Add(animation);
            SBTimer.Begin();
        }

        public void StopGetPoint()
        {
            SBTimer.Stop();
            BtnAttack.IsEnabled = false;
            GVMain.ClearSelection();
        }

        private void BtnAttack_Click(object sender, RoutedEventArgs e)
        {
            if (LBTeam.SelectedIndex >= 1&&GVMain.SelectedPoint!=(-1,-1))
            {
                Attack?.Invoke(LBTeam.SelectedIndex - 1, GVMain.SelectedPoint);
                StopGetPoint();
            }
        }

        public void UpdetePoint(int team,int x,int y,GameBoardBlock block,string name)
        {
            Boards[team].SetBlock(block, x, y,name);
        }
    }
}
