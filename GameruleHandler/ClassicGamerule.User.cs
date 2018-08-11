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
                TeamOf.Remove(UserName);
            }
            //throw new NotImplementedException();
        }

        private void Server_UserLoggedIn(string UserName)
        {
            OnlinePlayers.Add(UserName);
            Score.Add(UserName, 0);
            TellNewUser(UserName);
            //throw new NotImplementedException();
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
