using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 包括与客户端通信的代码
    /// </summary>
    partial class ClassicGamerule
    {
        /// <summary>
        /// 宣布当前应该now走，now属于team队
        /// </summary>
        /// <param name="now"></param>
        /// <param name="team"></param>
        private void ShowRound(string now, int team)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 广播name的分数改变了
        /// </summary>
        /// <param name="name"></param>
        private void UpdateScore(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 宣布游戏结束
        /// </summary>
        private void TellGameEnd()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 宣布这支队伍输了
        /// </summary>
        /// <param name="team"></param>
        private void TellFailure(int team)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 告知队伍攻击结果
        /// </summary>
        /// <param name="v"></param>
        /// <param name="result"></param>
        private void TellResult(int v, GameBoardBlock result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 广播攻击阶段开始
        /// </summary>
        private void TellGameStart()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 告知被攻击者攻击位置
        /// </summary>
        /// <param name="i"></param>
        private void TellAttact(FirePoints i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取name玩家的攻击目标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private List<FirePoints> GetPoints(string name, int fc)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 从list中的玩家处获得v条开火信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private List<FirePoints> GetPoints(List<string> list, int v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 广播现在由now队伍走
        /// </summary>
        /// <param name="now"></param>
        private void ShowRound(int now)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 广播一条文本信息
        /// </summary>
        /// <param name="msg"></param>
        private void BroadcastMessage(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
