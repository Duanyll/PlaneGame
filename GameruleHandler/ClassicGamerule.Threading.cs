using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
	/// <summary>
    /// 这一部分包含多线程处理的方法
    /// </summary>
    partial class ClassicGamerule
    { 
        public override void StartGame()
        {
            if (Info == null)
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < Info.PlayerCount; i++)
            {
                GameBoards.Add(new GameBoard.FullPlayerGameBoard(Info.Mask.Width, Info.Mask.Height));
            }

            Server.MessageRecieved += Server_MessageRecieved;
            Server.UserLoggedIn += Server_UserLoggedIn;
            Server.UserLoggedOut += Server_UserLoggedOut;
            Server.StartService();
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
    }
}
