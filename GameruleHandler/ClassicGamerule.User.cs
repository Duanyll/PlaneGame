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
        private void Server_UserLoggedOut(string UserName)
        {
            if (TeamOf.Keys.Contains(UserName))
            {
                //移除有关该玩家的信息
                Teams[TeamOf[UserName]].Remove(UserName);
                TeamOf.Remove(UserName);
            }
            //throw new NotImplementedException();
        }

        private async void Server_UserLoggedIn(string UserName)
        {
            OnlinePlayers.Add(UserName);
            Score.Add(UserName, 0);
            TellNewUser(UserName);
            int Team = await Task.Run(() => GetTeam(UserName));
            if (Team != -1)
            {
                TeamOf.Add(UserName, Team);
                Teams[Team].Add(UserName);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 向指定玩家询问加入哪个队伍
        /// </summary>
        /// <param name="UserName"></param>
        private int GetTeam(string UserName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 广播新玩家加入的信息
        /// </summary>
        /// <param name="userName"></param>
        private void TellNewUser(string userName)
        {
            throw new NotImplementedException();
        }

        private void Server_MessageRecieved(string UserName, string msg)
        {
            throw new NotImplementedException();
        }
    }
}
