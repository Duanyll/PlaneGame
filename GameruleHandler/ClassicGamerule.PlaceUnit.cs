using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 包括玩家布置棋盘部分的代码
    /// </summary>
    partial class ClassicGamerule
    {
        void StartPUState()
        {
            //初始化
            GameBoards.Clear();
            for (int i = 0; i < Info.PlayerCount; i++)
            {
                GameBoards.Add(new GameBoard.FullPlayerGameBoard(Info.Mask.Width, Info.Mask.Height));
            }
            Teams.Clear();
            for (int i = 0; i < Info.TeamCount; i++)
            {
                Teams.Add(new List<string>());
            }
        }
    }
}
