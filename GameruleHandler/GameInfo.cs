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
    [Serializable]
    public class GameInfo
    {
        const int MAX_PLAYER_COUNT = NetworkServer.MAX_CONNECTIONS;
        /// <summary>
        /// 表示最大队伍数。为什么是这个值？因为我喜欢。
        /// </summary>
        const int MAX_TEAM_COUNT = 8;
        int _pcnt = 2;
        /// <summary>
        /// 要求的玩家数量。设为0表示任意
        /// </summary>
        public int PlayerCount
        {
            get
            {
                return _pcnt;
            }
            set
            {
                if ((value >= 2&&value<=MAX_PLAYER_COUNT)||value==0)
                {
                    _pcnt = value;
                }
            }
        }

        int _tcnt = 2;
        /// <summary>
        /// 要求的队伍数量
        /// </summary>
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
        int _mxpint = 8;
        public int MaxPersonInATeam
        {
            get
            {
                return _mxpint;
            }
            set
            {
                if (value >= 1 && value <= NetworkServer.MAX_CONNECTIONS)
                {
                    _mxpint = value;
                }
            }
        }
        /// <summary>
        /// 表示可用的单位种类，按名称索引
        /// </summary>
        public Dictionary<string, GameBoard.PatternGameBoard> Patterns = new Dictionary<string, GameBoard.PatternGameBoard>();
        /// <summary>
        /// 表示每队玩家的基础游戏版
        /// </summary>
        public GameBoard.MaskedGameBoard Mask { get; set; }
        int _fpr = 1;
        /// <summary>
        /// 如果BindFPRWithHeadCount为false，则是玩家每回合的开火次数
        /// </summary>
        public int FirePerRound
        {
            get
            {
                return _fpr;
            }
            set
            {
                if (value > 0)
                {
                    _fpr = value;
                }
            }
        }

        /// <summary>
        /// 是否将开火次数与生命值相关联
        /// </summary>
        public bool BindFPRWithHeadCount { get; set; } = false;
        int _mxfpr = 1;
        int _mnfpr = 1;
        /// <summary>
        /// 最多开火次数
        /// </summary>
        public int MaxFPR
        {
            get
            {
                return _mxfpr;
            }
            set
            {
                if (value >= MinFPR)
                {
                    _mxfpr = value;
                }
            }
        }
        /// <summary>
        /// 最少开火次数
        /// </summary>
        public int MinFPR
        {
            get
            {
                return _mnfpr;
            }
            set
            {
                if (value > 0 && value <= _mxfpr)
                {
                    _mnfpr = value;
                }
            }
        }
        
        public enum RoundOrderType
        {
            /// <summary>
            /// 按队伍控制出牌顺序，轮到每个队伍出牌时每个队员都能出
            /// </summary>
            TeamTogether,
            /// <summary>
            /// 玩家按队伍顺序排列，每个玩家单独出牌
            /// </summary>
            PlayerWithTeam,
            /// <summary>
            /// 玩家按任意顺序排列，每个玩家单独出牌
            /// </summary>
            Shuffle,
            /// <summary>
            /// 即时混战
            /// </summary>
            Battle
        }
        public RoundOrderType RoundOrder { get; set; } = RoundOrderType.TeamTogether;
        public bool ShowKindWhileShoot { get; set; } = false;
        public GameBoard.CornorMode cornorMode
        {
            get
            {
                var mode = GameBoard.CornorMode.All;
                if (NoCornor)
                {
                    mode |= GameBoard.CornorMode.NoCornor;
                }
                if (NoEdge)
                {
                    mode |= GameBoard.CornorMode.NoEdge;
                }
                return mode;
            }
        }
        public bool AllowFlip { get; set; } = false;
        public bool AllowRoation { get; set; } = false;
        public bool NoCornor { get; set; } = true;
        public bool NoEdge { get; set; } = false;
        /// <summary>
        /// 等待时的超时(秒)
        /// </summary>
        public int TimeOut { get; set; } = 60;
    }
}
