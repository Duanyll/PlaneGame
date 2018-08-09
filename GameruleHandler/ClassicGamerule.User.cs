using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 这一部分包括关于用户登入登出的方法
    /// </summary>
    partial class ClassicGamerule
    {
        private void Server_UserLoggedOut(string UserName)
        {
            throw new NotImplementedException();
        }

        private void Server_UserLoggedIn(string UserName)
        {
            OnlinePlayers.Add(UserName);
            Score.Add(UserName, 0);
            throw new NotImplementedException();
        }

        private void Server_MessageRecieved(string UserName, string msg)
        {
            throw new NotImplementedException();
        }
    }
}
