using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    class ClassicGamerule:GameruleBase
    {
        public override void StartGame()
        {
            if (Info == null)
            {
                throw new InvalidOperationException();
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

        public override void Work()
        {
            throw new NotImplementedException();
        }
    }
}
