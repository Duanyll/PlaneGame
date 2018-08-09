using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 这一部分包括攻击阶段逻辑的方法
    /// </summary>
    partial class ClassicGamerule:GameruleBase
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

        /// <summary>
        /// 当各队伍都准备好棋盘后，新启线程调用这个开局
        /// </summary>
        public override void Work()
        {
            switch (Info.RoundOrder)
            {
                case GameInfo.RoundOrderType.PlayerWithTeam:
                    List<string> order = new List<string>();
                    foreach(List<string> i in Teams)
                    {
                        order.AddRange(i);
                    }
                    StartByPlayerOrder(order);
                    break;
                case GameInfo.RoundOrderType.Shuffle:
                    List<string> ShOrder = new List<string>();
                    foreach (List<string> i in Teams)
                    {
                        ShOrder.AddRange(i);
                    }
                    Reshuffle(ref ShOrder);
                    StartByPlayerOrder(ShOrder);
                    break;
            }
        }

        /// <summary>
        /// 打乱一个序列
        /// </summary>
        /// <param name="listtemp"></param>
        private void Reshuffle<T>(ref List<T> listtemp)
        {
            //随机交换
            Random ram = new Random();
            int currentIndex;
            T tempValue;
            for (int i = 0; i < listtemp.Count; i++)
            {
                currentIndex = ram.Next(0, listtemp.Count - i);
                tempValue = listtemp[currentIndex];
                listtemp[currentIndex] = listtemp[listtemp.Count - 1 - i];
                listtemp[listtemp.Count - 1 - i] = tempValue;
            }
        }

        /// <summary>
        /// 按玩家顺序的形式开局
        /// </summary>
        /// <param name="order">玩家的顺序</param>
        void StartByPlayerOrder(List<string> order)
        {
            AliveTeam.Clear();
            for(int i = 0; i < Info.TeamCount; i++)
            {
                AliveTeam.Add(i);
            }
            TellGameStart();
            int now = 0;
            int DieOrder = 0;   //记录已经死了几个队伍
            while (true)
            {
                string Name = order[now];   //当前玩家的名字
                if (!AliveTeam.Contains(TeamOf[Name]))  //跳过已经输了的队伍
                {
                    continue;
                }
                ShowRound(Name, TeamOf[Name]);
                List<FirePoints> points = GetPoints(Name, GetFireCount(Name));  //从客户端获取攻击目标
                //依次处理攻击目标
                foreach(FirePoints i in points)
                {
                    TellAttact(i);
                    GameBoardBlock result = GameBoards[i.Team].Attack(i.X, i.Y);
                    TellResult(TeamOf[Name], result);
                    if (GameBoards[i.Team].HeadCount == 0)  
                    {
                        if (AliveTeam.Contains(i.Team))
                        {
                            //处理队伍挂了的情况
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
                //处理终局的计分情况
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
    }
}
