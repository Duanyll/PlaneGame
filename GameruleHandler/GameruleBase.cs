using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NetServer;

namespace GameruleHandler
{
    /// <summary>
    /// 开局后（游戏选项设置完毕）游戏规则处理（包括等待玩家加入，玩家放置单位，正式开局等动作）的接口
    /// </summary>
    public abstract class GameruleBase
    {
        protected NetworkServer Server = new NetworkServer();
        public GameInfo Info { get; set; }
        protected Thread MainThread;
        /// <summary>
        /// 检测某个GameInfo是否符合当前模式要求
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool CheckGameInfo(GameInfo info)
        {
            //TODO:完善Check
            if(info.Mask == null)
            {
                return false;
            }
            return true;
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 启动网络服务，并在人数足够后开局
        /// </summary>
        public abstract void StartGame();
        /// <summary>
        /// 中止网络服务与游戏进程
        /// </summary>
        public abstract void AbortGame();
    }
}
