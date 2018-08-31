using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 这一部分包括关于用户登入登出，收到信息的方法
    /// </summary>
    partial class ClassicGamerule
    {
        /// <summary>
        /// 当前所有在线玩家的用户名
        /// </summary>
        public List<string> OnlinePlayers = new List<string>();
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
        private void Server_UserLoggedOut(string UserName)
        {
            Server.BroadCastToAll("ULGO|" + UserName);
            OnlinePlayers.Remove(UserName);
            if (TeamOf.Keys.Contains(UserName))
            {
                //移除有关该玩家的信息
                Teams[TeamOf[UserName]].Remove(UserName);
                TeamOf.Remove(UserName);
            }
            //throw new NotImplementedException();
        }

        private void Server_UserLoggedIn(string UserName)
        {
            OnlinePlayers.Add(UserName);
            Score.Add(UserName, 0);
            foreach(var i in Score)
            {
                UpdateScore(i.Key);
            }
            TellNewUser(UserName);
            Server.SendTo(UserName,"TCLR|");
            foreach(var i in TeamOf)
            {
                Server.SendTo(UserName, "TMIF|" + i.Key + '|' + i.Value);
            }
            GetTeam(UserName);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 向指定玩家询问加入哪个队伍，并自动添加到队伍中
        /// </summary>
        /// <param name="UserName"></param>
        private void GetTeam(string UserName)
        {
            string AvaliableTeams = "";
            for (int i = 0; i < Info.TeamCount; i++)
            {
                if (Teams[i].Count < Info.MaxPersonInATeam)
                {
                    AvaliableTeams += i;
                }
            }
            Server.SendTo(UserName, "GTEM|" + AvaliableTeams);
        }

        private void GetTeam_Callback(string UserName, int Team)
        {
            if (Teams[Team].Count < Info.MaxPersonInATeam)
            {
                Server.BroadCastToAll("TMIF|" + UserName + '|' + Team);
                TeamOf.Add(UserName, Team);
                Teams[Team].Add(UserName);
                AllowPU(UserName);
            }
        }
    }
}
