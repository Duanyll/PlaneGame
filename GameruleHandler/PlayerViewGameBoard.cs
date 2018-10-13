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
                for (int i = 0; i < Height; i++)
                {
                    BlockDetails.Add(new List<string>());
                    for (int j = 0; j < Width; j++)
                    {
                        BlockDetails[i].Add(null);
                    }
                }
            }

            PlayerViewGameBoard(int w, int h) : base(w, h)
            {
                for (int i = 0; i < Height; i++)
                {
                    BlockDetails.Add(new List<string>());
                    for (int j = 0; j < Width; j++)
                    {
                        BlockDetails[i].Add(null);
                        Blocks[i][j] = GameBoardBlock.Unknown;
                    }
                }
                BlockRangeChanged?.Invoke(0, 0, Height - 1, Width - 1);
            }

            List<List<string>> BlockDetails = new List<List<string>>();

            public void SetBlock(GameBoardBlock block, int x, int y,string detail = null)
            {
                this[x, y] = block;
                BlockDetails[x][y] = detail;
                BlockChanged?.Invoke(x, y);
            }

            public string GetDetail(int x,int y)
            {
                return BlockDetails[x][y];
            }
        }
    }
}
