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
            TeamOf.Clear();
            GameBoards.Clear();
            Teams.Clear();
            RemainUnits.Clear();
            TeamsOKToAttack.Clear();
            for (int i = 0; i < Info.TeamCount; i++)
            {
                Teams.Add(new List<string>());
                GameBoards.Add(Info.Mask.GetFullPlayerGameBoard());
                RemainUnits.Add(GetNewUnitCount());
            }
            foreach(string Name in OnlinePlayers)
            {
                GetTeam(Name);
            }
        }

        /// <summary>
        /// 允许用户放置单位
        /// </summary>
        /// <param name="UserName"></param>
        void AllowPU(string UserName)
        {
            Server.SendTo(UserName, "MPSZ|" + Info.Mask.Width + '|' + Info.Mask.Height);
            Server.SendTo(UserName, "SPUS|");
            foreach (var Unit in RemainUnits[TeamOf[UserName]])
            {
                Server.SendTo(UserName, "UNIT|" + Unit.Key + '|' + Unit.Value +'|' + Info.Patterns[Unit.Key].ToString());
            }
            Server.SendTo(UserName, "GBRD|" + GameBoards[TeamOf[UserName]].ToString());
        }

        void ResetBoard(string UserName)
        {
            if (TeamOf.Keys.Contains(UserName) && !TeamsOKToAttack.Contains(TeamOf[UserName]))
            {
                RemainUnits[TeamOf[UserName]] = GetNewUnitCount();
                GameBoards[TeamOf[UserName]] = Info.Mask.GetFullPlayerGameBoard();
                foreach(string name in Teams[TeamOf[UserName]])
                {
                    Server.SendTo(name, "SBAR|已清空棋盘");
                    Server.SendTo(name, "GBRD|" + GameBoards[TeamOf[UserName]].ToString());
                    foreach(var Unit in RemainUnits[TeamOf[UserName]])
                    {
                        Server.SendTo(name, "UCNT|" + Unit.Key + '|' + Unit.Value);
                    }
                }
            }
        }

        Dictionary<string,int> GetNewUnitCount()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach(var i in Info.Patterns)
            {
                ret.Add(i.Key, i.Value.CountPerTeam);
            }
            return ret;
        }

        /// <summary>
        /// 存储每个队伍的剩余单位数量
        /// </summary>
        List<Dictionary<string, int>> RemainUnits = new List<Dictionary<string, int>>(); 
        /// <summary>
        /// 处理用户的放置单位请求并返回消息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PatternName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void PlaceUnit(string UserName, string PatternName, int x, int y, GameBoard.PatternGameBoard.RoationMode roation = GameBoard.PatternGameBoard.RoationMode.None ,GameBoard.PatternGameBoard.FlipMode flip = GameBoard.PatternGameBoard.FlipMode.None)
        {
            if (TeamOf.Keys.Contains(UserName) && !TeamsOKToAttack.Contains(TeamOf[UserName]))
            {
                Info.Patterns[PatternName].Roation = roation;
                Info.Patterns[PatternName].Flip = flip;
                if (RemainUnits[TeamOf[UserName]][PatternName] > 0)
                {
                    if (GameBoards[TeamOf[UserName]].PutPatern(Info.Patterns[PatternName], x, y, Info.cornorMode))
                    {
                        RemainUnits[TeamOf[UserName]][PatternName]--;
                        foreach (string name in Teams[TeamOf[UserName]])
                        {
                            Server.SendTo(name, "SBAR|已放置单位");
                            Server.SendTo(name, "GBRD|" + GameBoards[TeamOf[UserName]].ToString());
                            Server.SendTo(name, "UCNT|" + PatternName + '|' + RemainUnits[TeamOf[UserName]][PatternName]);
                        }
                    }
                    else
                    {
                        Server.SendTo(UserName, "SBAR|放置失败，空间不足");
                    }
                }
                else
                {
                    Server.SendTo(UserName, "SBAR|放置失败，剩余单位不足");
                }
            }
        }

        /// <summary>
        /// 放置好棋盘的队伍
        /// </summary>
        List<int> TeamsOKToAttack = new List<int>();
        /// <summary>
        /// 检查这个队伍是否放置好棋盘并返回消息，锁定棋盘
        /// </summary>
        /// <param name="UserName"></param>
        void FinishPUState(string UserName)
        {
            if (TeamOf.Keys.Contains(UserName) && !TeamsOKToAttack.Contains(TeamOf[UserName]))
            {
                bool TeamOK = true;
                foreach(var i in RemainUnits[TeamOf[UserName]].Values)
                {
                    if (i > 0)
                    {
                        TeamOK = false;
                        break;
                    }
                }
                if (!TeamOK)
                {
                    Server.SendTo(UserName, "SBAR|还有单位未放置");
                }
                else
                {
                    TeamsOKToAttack.Add(TeamOf[UserName]);
                    Server.BroadCastToAll("SLOG|" + TeamOf[UserName] + "队已准备好棋盘");
                    if(TeamsOKToAttack.Count == Info.TeamCount)
                    {
                        StartAttackState();
                    }
                }
            }
        }
    }
}
