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

namespace PlaneGame
{
    /// <summary>
    /// NewGamePage1.xaml 的交互逻辑
    /// </summary>
    public partial class NewGamePage1 : Page
    {
        public NewGamePage1()
        {
            InitializeComponent();
        }

        public GameInfo Info { get; set; } = new GameInfo();

        private void LBUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBUnits.SelectedItem != null)
            {
                //处理删除键是否显示
                //选中基础棋盘
                if (LBUnits.SelectedIndex == 0 || LBUnits.Items.Count <= 2)
                {
                    BtnDeleteUnit.IsEnabled = false;
                }
                else
                {
                    BtnDeleteUnit.IsEnabled = true;
                }
                TBUnitName.Text = LBUnits.SelectedItem.ToString();
            }
        }

        private void TBUnitName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LBUnits.SelectedIndex != 0 && LBUnits.SelectedIndex != -1 && !LBUnits.Items.Contains(TBUnitName.Text))
            {
                int idx = LBUnits.SelectedIndex;
                if (Info.Patterns.Keys.Contains(LBUnits.SelectedItem.ToString()) && LBUnits.SelectedItem.ToString() != TBUnitName.Text)
                {
                    GameBoard.PatternGameBoard tmp = Info.Patterns[LBUnits.SelectedItem.ToString()];
                    Info.Patterns.Remove(LBUnits.SelectedItem.ToString());
                    Info.Patterns.Add(TBUnitName.Text, tmp);
                }
                LBUnits.Items[LBUnits.SelectedIndex] = TBUnitName.Text;
                LBUnits.SelectedIndex = idx;
            }
        }

        int NewUnitID = 0;
        private void BtnAddUnit_Click(object sender, RoutedEventArgs e)
        {
            LBUnits.Items.Add("单位" + ++NewUnitID);
            Info.Patterns.Add("单位" + NewUnitID, new GameBoard.PatternGameBoard(5, 5));
            LBUnits.SelectedIndex = LBUnits.Items.Count - 1;
        }

        private void BtnSaveSizeChange_Click(object sender, RoutedEventArgs e)
        {
            if(int.TryParse(TBWidth.Text,out int w)&&int.TryParse(TBHeight.Text,out int h))
            {
                Info.Mask = new GameBoard.MaskedGameBoard(w, h);
                SVGameBoard.Content = new GameBoardView(Info.Mask);
            }
        }
    }
}
