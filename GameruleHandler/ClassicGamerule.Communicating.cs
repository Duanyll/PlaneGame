using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 包括与客户端通信的代码
    /// </summary>
    partial class ClassicGamerule
    {
        /// <summary>
        /// 宣布当前应该now走，now属于team队
        /// </summary>
        /// <param name="now"></param>
        /// <param name="team"></param>
        private void ShowRound(string now, int team)
        {
            Server.BroadCastToAll("SNOW|" + now + '|' + team);
        }

        /// <summary>
        /// 广播name的分数改变了
        /// </summary>
        /// <param name="name"></param>
        private void UpdateScore(string name)
        {
            Server.BroadCastToAll("SCOR|" + name + '|' + Score[name]);
        }

        /// <summary>
        /// 宣布游戏结束
        /// </summary>
        private void TellGameEnd()
        {
            Server.BroadCastToAll("SLOG|游戏结束");
            Server.BroadCastToAll("SBAR|游戏结束");
        }

        /// <summary>
        /// 宣布这支队伍输了
        /// </summary>
        /// <param name="team"></param>
        private void TellFailure(int team)
        {
            Server.BroadCastToAll("SLOG|"+team+"队失去了所有单位");
            Server.BroadCastToAll("SBAR|"+team+"队失去了所有单位");
        }

        /// <summary>
        /// 告知队伍攻击结果
        /// </summary>
        /// <param name="v"></param>
        /// <param name="result"></param>
        private void TellResult(int Team, FirePoint point,GameBoardBlock result)
        {
            if (Info.ShowKindWhileShoot)
            {
                string msg = "AUGB|" + point.Team + '|' + point.X + '|' + point.Y + '|' + GameBoards[point.Team].CheckName(result);
                foreach (string name in Teams[Team])
                {
                    Server.SendTo(name, msg);
                }
            }
            else
            {
                string msg = "AUGB|" + point.Team + '|' + point.X + '|' + point.Y + '|';
                if (Enum.IsDefined(result.GetType(), result))
                {
                    msg += result.ToString();
                }
                else
                {
                    if (char.IsUpper((char)result))
                    {
                        msg += GameBoardBlock.Head.ToString();
                    }
                    else
                    {
                        msg += GameBoardBlock.Body.ToString();
                    }
                }
                foreach (string name in Teams[Team])
                {
                    Server.SendTo(name, msg);
                }
            }
        }

        /// <summary>
        /// 广播攻击阶段开始
        /// </summary>
        private void TellGameStart()
        {
            Server.BroadCastToAll("SGTM|");
            Server.BroadCastToAll("ASRT|");
            for(int i = 0; i < Info.TeamCount; i++)
            {
                Server.BroadCastToAll("AGBR|" + i + '|' + Info.Mask.ToString());
            }
        }

        /// <summary>
        /// 告知被攻击者攻击位置
        /// </summary>
        /// <param name="i"></param>
        private void TellAttact(FirePoint i)
        {
            foreach(string name in Teams[i.Team])
            {
                Server.SendTo(name, "SLOG|" + "您的(" + i.X + "," + i.Y + "格受到了攻击");
            }
        }

        readonly TimeSpan TimeForEachRecieve = new TimeSpan(0, 0, 0, 0, 100);
        (string Name, FirePoint Point)? LastAttack = null;
        /// <summary>
        /// 获取name玩家的攻击目标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        private List<FirePoint> GetPoints(string name, int fc)
        {
            List<FirePoint> ret = new List<FirePoint>();
            for(int i = 0; i < fc; i++)
            {
                Server.SendTo(name, "GPNT|" + (fc - i).ToString() + '|' + (int)Info.TimeOut);
                TimeSpan timePast = new TimeSpan(0);
                TimeSpan timeOut = new TimeSpan(0, 0, Info.TimeOut);
                while (timePast < timeOut)
                {
                    if (LastAttack.HasValue)
                    {
                        if (LastAttack.Value.Name == name)
                        {
                            ret.Add(LastAttack.Value.Point);
                            LastAttack = null;
                            break;
                        }
                    }
                }
                Server.SendTo(name, "SGPT|");
            }
            return ret;
        }


        /// <summary>
        /// 从list中的玩家处获得v条开火信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        private List<FirePoint> GetPoints(List<string> list, int fc)
        {
            List<FirePoint> ret = new List<FirePoint>();
            for (int i = 0; i < fc; i++)
            {
                foreach(var name in list)
                    Server.SendTo(name, "GPNT|" + (fc - i).ToString() + '|' + (int)Info.TimeOut);
                TimeSpan timePast = new TimeSpan(0);
                TimeSpan timeOut = new TimeSpan(0, 0, Info.TimeOut);
                while (timePast < timeOut)
                {
                    if (LastAttack.HasValue)
                    {
                        if (list.Contains(LastAttack.Value.Name))
                        {
                            ret.Add(LastAttack.Value.Point);
                            LastAttack = null;
                            break;
                        }
                    }
                }
                foreach(var name in list)
                    Server.SendTo(name, "SGPT|");
            }
            return ret;
        }

        private void GetPoints_Callback(string UserName, int TeamID, int x, int y)
        {
            LastAttack = (UserName, new FirePoint() { Team = TeamID, X = x, Y = y });
        }

        /// <summary>
        /// 广播现在由now队伍走
        /// </summary>
        /// <param name="now"></param>
        private void ShowRound(int now)
        {
            Server.BroadCastToAll("SNOW||" + now);
        }

        /// <summary>
        /// 广播一条文本信息
        /// </summary>
        /// <param name="msg"></param>
        private void BroadcastMessage(string msg)
        {
            Server.BroadCastToAll("SBAR|" + msg);
        }

        /// <summary>
        /// 广播新玩家加入的信息
        /// </summary>
        /// <param name="userName"></param>
        private void TellNewUser(string userName)
        {
            Server.BroadCastToAll("ULGI|" + userName);
        }

        private async void Server_MessageRecieved(string UserName, string msg)
        {
            await Task.Run(() =>
            {
                string[] Parameters = msg.Split('|');
                switch (Parameters[0])
                {
                    case "CHAT":
                        Server.BroadCastToAll("CHAT|" + UserName + '|' + Parameters[1]);
                        break;
                    case "STEM":
                        GetTeam_Callback(UserName, int.Parse(Parameters[1]));
                        break;
                    case "PUTU":
                        PlaceUnit(UserName, Parameters[1], int.Parse(Parameters[2]), int.Parse(Parameters[3]),(GameBoard.PatternGameBoard.RoationMode)int.Parse(Parameters[5]), (GameBoard.PatternGameBoard.FlipMode)int.Parse(Parameters[4]));
                        break;
                    case "PUCR":
                        ResetBoard(UserName);
                        break;
                    case "FPUS":
                        FinishPUState(UserName);
                        break;
                    case "ATCK":
                        GetPoints_Callback(UserName, int.Parse(Parameters[1]), int.Parse(Parameters[2]), int.Parse(Parameters[3]));
                        break;
                    case "REFR":
                        break;
                }
            });
        }
    }
}
