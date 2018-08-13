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

        /// <summary>
        /// 允许用户放置单位
        /// </summary>
        /// <param name="UserName"></param>
        void AllowPU(string UserName)
        {

        }

        /// <summary>
        /// 处理用户的放置单位请求并返回消息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PatternName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void PlaceUnit(string UserName,string PatternName,int x,int y)
        {

        }

        /// <summary>
        /// 让用户停止放置单位
        /// </summary>
        /// <param name="UserName"></param>
        void StopPU(string UserName)
        {

        }
    }
}
