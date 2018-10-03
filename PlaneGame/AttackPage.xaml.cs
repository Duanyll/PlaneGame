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
    /// AttackPage.xaml 的交互逻辑
    /// </summary>
    public partial class AttackPage : Page
    {
        public AttackPage(GameBoard.FullPlayerGameBoard yourboard)
        {
            InitializeComponent();
            YourBoard = yourboard;
            GVMain.Board = YourBoard;
            LBTeam.Items.Add("Your");
        }

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
            LBTeam.Items.Add(Team.ToString());
        }
    }
}
