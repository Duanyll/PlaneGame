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

        /// <summary>
        /// 清空并重新加载内容
        /// </summary>
        private void Refresh()
        {
            GridMain.Children.Clear();
            GridMain.RowDefinitions.Clear();
            for(int i = 0; i < board.Height; i++)
            {
                GridMain.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(Properties.Settings.Default.GBGridHeight)
                });
            }
            GridMain.ColumnDefinitions.Clear();
            for(int i = 0; i < board.Width; i++)
            {
                GridMain.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(Properties.Settings.Default.GBGridWidth)
                });
            }
            for(int i = 0; i < board.Height; i++)
            {
                for(int j = 0; j < board.Width; j++)
                {
                    GameBoardBlockView block = new GameBoardBlockView(Board[i,j]);
                    block.SetValue(Grid.RowProperty, i);
                    block.SetValue(Grid.ColumnProperty, j);
                    GridMain.Children.Add(block);
                }
            }
        }

        private void Board_BlockChanged(int x1, int y1)
        {
            throw new NotImplementedException();
        }

        public GameBoardView()
        {
            InitializeComponent();
        }

        public GameBoardView(GameBoard.MaskedGameBoard a)
        {
            InitializeComponent();
            Board = a;
        }
    }
}
