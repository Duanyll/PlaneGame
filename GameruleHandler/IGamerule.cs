using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetServer;

namespace GameruleHandler
{
    /// <summary>
    /// 开局后（游戏选项设置完毕）游戏规则处理的接口
    /// </summary>
    public interface IGamerule
    {
        NetServer.NetServer Server { set; }
        GameInfo Info { set; }
        void Work();
        void StartGame();
        void AbortGame();
    }
}
