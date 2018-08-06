using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetServer;

namespace GameruleHandler
{
    /// <summary>
    /// 存储一局游戏的公开信息与选项
    /// </summary>
    public class GameInfo
    {
        const int MAX_PLAYER_COUNT = NetworkServer.MAX_CONNECTIONS;
        /// <summary>
        /// 表示最大队伍数。为什么是这个值？因为我喜欢。
        /// </summary>
        const int MAX_TEAM_COUNT = 8;
        int _pcnt = 2;
        //要求的玩家数量。
        public int PlayerCount
        {
            get
            {
                return _pcnt;
            }
            set
            {
                if (value >= 2&&value<=MAX_PLAYER_COUNT)
                {
                    _pcnt = value;
                }
            }
        }

        int _tcnt = 2;
        public int TeamCount
        {
            get
            {
                return _tcnt;
            }
            set
            {
                if (value >= 2 && value <= MAX_TEAM_COUNT)
                {
                    _tcnt = value;
                }
            }
        }
        int _mxpint = 1;
        public int MaxPersonInATeam
        {
            get
            {
                return _mxpint;
            }
            set
            {
                if (value >= 1 && value <= PlayerCount)
                {
                    _mxpint = value;
                }
            }
        }
        /// <summary>
        /// 表示可用的单位种类
        /// </summary>
        public List<GameBoard.PatternGameBoard> Patterns = new List<GameBoard.PatternGameBoard>();
        /// <summary>
        /// 表示每队玩家的基础游戏版
        /// </summary>
        public GameBoard.MaskedGameBoard Mask { get; set; }
    }
}
