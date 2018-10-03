﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GameruleHandler
{
	/// <summary>
    /// 这一部分包含多线程处理的方法
    /// </summary>
    public partial class ClassicGamerule
    {
        public delegate void LogEventHandler(string Content);

        public event LogEventHandler Log;

        public ClassicGamerule(GameInfo Info)
        {
            this.Info = Info;
            Server.Log += (str) =>
            {
                Log?.Invoke("(网络服务)"+str);
            };
        }

        public override void StartGame()
        {
            if (Info == null)
            {
                throw new InvalidOperationException();
            }

            Log?.Invoke("游戏已开始运行");

            StartPUState();

            Server.MessageRecieved += Server_MessageRecieved;
            Server.UserLoggedIn += Server_UserLoggedIn;
            Server.UserLoggedOut += Server_UserLoggedOut;
            Server.StartService();
        }

        public override void AbortGame()
        {
            Log?.Invoke("已强制结束游戏");
            if (MainThread != null)
            {
                if (MainThread.IsAlive == true)
                {
                    MainThread.Abort();
                }
            }
            Server.StopService();
        }

        private void StartAttackState()
        {
            if (MainThread!=null&&MainThread.IsAlive)
            {
                MainThread.Abort();
            }
            MainThread = new Thread(Work);
            MainThread.Start();
        }
    }
}
