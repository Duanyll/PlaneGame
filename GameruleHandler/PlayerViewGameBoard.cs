using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    partial class GameBoard
    {
        /// <summary>
        /// 表示玩家看对方的游戏版(有迷雾什么的)
        /// </summary>
        [Serializable]
        public class PlayerViewGameBoard : GameBoard
        {
            public PlayerViewGameBoard(string[] vs) : base(vs)
            {
            }

            PlayerViewGameBoard(int w, int h) : base(w, h)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        Blocks[i][j] = GameBoardBlock.Unknown;
                    }
                }
                BlockRangeChanged?.Invoke(0, 0, Height - 1, Width - 1);
            }

            public void SetBlock(GameBoardBlock block, int x, int y)
            {
                this[x, y] = block;
            }
        }
    }
}
