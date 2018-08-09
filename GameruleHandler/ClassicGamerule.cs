using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    class ClassicGamerule:GameruleBase
    {
        /// <summary>
        /// 当前所有在线玩家的用户名
        /// </summary>
        List<string> OnlinePlayers = new List<string>();
        /// <summary>
        /// 每个玩家及其分数
        /// </summary>
        Dictionary<string, int> Score = new Dictionary<string, int>();
        /// <summary>
        /// 每个队伍拥有的玩家
        /// </summary>
        List<List<string>> Teams = new List<List<string>>();
        /// <summary>
        /// 每个玩家所属的队伍
        /// </summary>
        Dictionary<string, int> TeamOf = new Dictionary<string, int>();
        List<GameBoard.FullPlayerGameBoard> GameBoards = new List<GameBoard.FullPlayerGameBoard>();
        /// <summary>
        /// 当前存活的队伍编号
        /// </summary>
        List<int> AliveTeam = new List<int>();

        public override void StartGame()
        {
            if (Info == null)
            {
                throw new InvalidOperationException();
            }

            for(int i = 0; i < Info.PlayerCount; i++)
            {
                GameBoards.Add(new GameBoard.FullPlayerGameBoard(Info.Mask.Width, Info.Mask.Height));
            }

            Server.MessageRecieved += Server_MessageRecieved;
            Server.UserLoggedIn += Server_UserLoggedIn;
            Server.UserLoggedOut += Server_UserLoggedOut;
            Server.StartService();
        }

        private void Server_UserLoggedOut(string UserName)
        {
            throw new NotImplementedException();
        }

        private void Server_UserLoggedIn(string UserName)
        {
            throw new NotImplementedException();
        }

        private void Server_MessageRecieved(string UserName, string msg)
        {
            throw new NotImplementedException();
        }

        public override void AbortGame()
        {
            if (MainThread != null)
            {
                if (MainThread.IsAlive == true)
                {
                    MainThread.Abort();
                }
            }
            Server.StopService();
        }

        /// <summary>
        /// 当各队伍都准备好棋盘后，新启线程调用这个开局
        /// </summary>
        public override void Work()
        {
            
        }

        /// <summary>
        /// 按玩家顺序的形式开局
        /// </summary>
        /// <param name="order">玩家的顺序</param>
        void StartByPlayerOrder(List<string> order)
        {
            for(int i = 0; i < Info.TeamCount; i++)
            {
                AliveTeam.Add(i);
            }
            TellGameStart();
            int now = 0;
            int DieOrder = 0;
            while (true)
            {
                string Name = order[now];
                if (!AliveTeam.Contains(TeamOf[Name]))
                {
                    continue;
                }
                ShowRound(Name, TeamOf[Name]);
                List<FirePoints> points = GetPoints(Name, GetFireCount(Name));
                foreach(FirePoints i in points)
                {
                    TellAttact(i);
                    GameBoardBlock result = GameBoards[i.Team].Attack(i.X, i.Y);
                    TellResult(TeamOf[Name], result);
                    if (GameBoards[i.Team].HeadCount == 0)
                    {
                        if (AliveTeam.Contains(i.Team))
                        {
                            DieOrder++;
                            foreach(string name in Teams[i.Team])
                            {
                                Score[name] += DieOrder;
                                UpdateScore(name);
                            }
                            TellFailure(i.Team);
                            AliveTeam.Remove(i.Team);
                        }
                    }
                }
                if (AliveTeam.Count <= 1)
                {
                    DieOrder++;
                    foreach (string name in Teams[0])
                    {
                        Score[name] += DieOrder;
                        UpdateScore(name);
                    }
                    break;
                }
                now++;
                now %= Info.PlayerCount;
            }
            TellGameEnd();
        }

        /// <summary>
        /// 广播name的分数改变了
        /// </summary>
        /// <param name="name"></param>
        private void UpdateScore(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 宣布游戏结束
        /// </summary>
        private void TellGameEnd()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 宣布这支队伍输了
        /// </summary>
        /// <param name="team"></param>
        private void TellFailure(int team)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 告知队伍攻击结果
        /// </summary>
        /// <param name="v"></param>
        /// <param name="result"></param>
        private void TellResult(int v, GameBoardBlock result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 广播攻击阶段开始
        /// </summary>
        private void TellGameStart()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 告知被攻击者攻击位置
        /// </summary>
        /// <param name="i"></param>
        private void TellAttact(FirePoints i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取name玩家的攻击目标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private List<FirePoints> GetPoints(string name, int fc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回now应该开火多少次
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        private int GetFireCount(string now)
        {
            if (Info.BindFPRWithHeadCount)
            {
                int hcnt = GameBoards[TeamOf[now]].HeadCount;
                if (hcnt > Info.MaxFPR)
                {
                    return Info.MaxFPR;
                }
                if(hcnt < Info.MinFPR)
                {
                    return Info.MinFPR;
                }
                return hcnt;
            }
            else
            {
                return Info.FirePerRound;
            }
        }

        /// <summary>
        /// 宣布当前应该now走，now属于team队
        /// </summary>
        /// <param name="now"></param>
        /// <param name="team"></param>
        private void ShowRound(string now, int team)
        {
            throw new NotImplementedException();
        }
    }
}
