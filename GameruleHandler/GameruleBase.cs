using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetServer;

namespace GameruleHandler
{
    /// <summary>
    /// 开局后（游戏选项设置完毕）游戏规则处理（包括等待玩家加入，玩家放置单位，正式开局等动作）的接口
    /// </summary>
    public abstract class GameruleBase
    {
        NetworkServer Server = new NetworkServer();
        GameInfo Info { get; set; }
        /// <summary>
        /// 检测某个GameInfo是否符合当前模式要求
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool CheckGameInfo(GameInfo info)
        {
            //TODO:完善Check
            throw new NotImplementedException();
        }
        public abstract void Work();
        public virtual void StartGame()
        {

        }
        public virtual void AbortGame()
        {

        }
    }
}
