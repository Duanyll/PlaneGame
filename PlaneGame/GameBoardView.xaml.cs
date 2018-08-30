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
    /// MaskGameBoardView.xaml 的交互逻辑
    /// </summary>
    public partial class GameBoardView : UserControl
    {
        public enum GameBoardClickMode
        {
            DoNothing,
            SwitchHead,
            SwitchBody,
            SwitchBarrier,
            PutUnit,
            Attack
        }
        public GameBoardClickMode ClickMode { get; set; }

        GameBoard board;
        public GameBoard Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
                board.BlockChanged += Board_BlockChanged;
                Refresh();
            }
        }

        List<List<GameBoardBlockView>> BlockViews = new List<List<GameBoardBlockView>>();

        double BlockHeight = Properties.Settings.Default.GBGridHeight;
        double BlockWidth = Properties.Settings.Default.GBGridWidth;
        /// <summary>
        /// 清空并重新加载内容
        /// </summary>
        private void Refresh()
        {
            GridMain.Children.Clear();
            GridMain.RowDefinitions.Clear();
            BlockViews.Clear();
            for(int i = 0; i < board.Height; i++)
            {
                GridMain.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(BlockHeight)
                });
            }
            GridMain.ColumnDefinitions.Clear();
            for(int i = 0; i < board.Width; i++)
            {
                GridMain.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(BlockWidth)
                });
            }
            for(int i = 0; i < board.Height; i++)
            {
                BlockViews.Add(new List<GameBoardBlockView>());
                for(int j = 0; j < board.Width; j++)
                {
                    GameBoardBlockView block = new GameBoardBlockView(Board[i,j]);
                    block.SetValue(Grid.RowProperty, i);
                    block.SetValue(Grid.ColumnProperty, j);
                    block.XPos = i;
                    block.YPos = j;
                    block.Click += Block_Click;
                    BlockViews[i].Add(block);
                    GridMain.Children.Add(block);
                }
            }
        }

        private void Block_Click(int x, int y, object sender)
        {
            switch (ClickMode)
            {
                case GameBoardClickMode.DoNothing:
                    break;
                case GameBoardClickMode.SwitchHead:
                    if(Board is GameBoard.PatternGameBoard pattern)
                    {
                        pattern.SwitchHead(x, y);
                    }
                    break;
                case GameBoardClickMode.SwitchBody:
                    if(Board is GameBoard.PatternGameBoard patter)
                    {
                        patter.SwitchBody(x, y);
                    }
                    break;
                case GameBoardClickMode.SwitchBarrier:
                    if(Board is GameBoard.MaskedGameBoard mask)
                    {
                        mask.SwitchBarrier(x, y);
                    }
                    break;
                case GameBoardClickMode.Attack:
                    throw new NotImplementedException();
                case GameBoardClickMode.PutUnit:
                    throw new NotImplementedException();
            }
        }

        private void Board_BlockChanged(int x1, int y1)
        {
            BlockViews[x1][y1].Block = board[x1,y1];
        }

        public GameBoardView()
        {
            InitializeComponent();
        }

        public GameBoardView(GameBoard a)
        {
            InitializeComponent();
            Board = a;
        }
    }
}
