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
                GameBoards.Add(Info.Mask.GetFullPlayerGameBoard());
                
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
            Server.SendTo(UserName, "SPUS|");
        }

        void ResetBoard(string UserName)
        {
            if (TeamOf.Keys.Contains(UserName))
            {
                GameBoards[TeamOf[UserName]] = Info.Mask.GetFullPlayerGameBoard();
                foreach(string name in Teams[TeamOf[UserName]])
                {
                    Server.SendTo(name, "SBAR|已清空棋盘");
                    Server.SendTo(name, "GBRD|" + GameBoards[TeamOf[UserName]].ToString());
                }
            }
        }

        /// <summary>
        /// 处理用户的放置单位请求并返回消息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PatternName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void PlaceUnit(string UserName, string PatternName, int x, int y, GameBoard.PatternGameBoard.RoationMode roation = GameBoard.PatternGameBoard.RoationMode.None ,GameBoard.PatternGameBoard.FlipMode flip = GameBoard.PatternGameBoard.FlipMode.None)
        {
            if (TeamOf.Keys.Contains(UserName))
            {
                Info.Patterns[PatternName].Roation = roation;
                Info.Patterns[PatternName].Flip = flip;
                GameBoards[TeamOf[UserName]].PutPatern(Info.Patterns[PatternName], x, y, Info.cornorMode);
                foreach (string name in Teams[TeamOf[UserName]])
                {
                    Server.SendTo(name, "SBAR|已放置单位");
                    Server.SendTo(name, "GBRD|" + GameBoards[TeamOf[UserName]].ToString());
                }
            }
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
