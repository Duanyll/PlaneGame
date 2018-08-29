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
        /// 表示GameBoard模板
        /// </summary>
        public class MaskedGameBoard : GameBoard
        {
            public MaskedGameBoard(int w, int h) : base(w, h)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        Blocks[i][j] = GameBoardBlock.Nothing;
                    }
                }
                BlockRangeChanged?.Invoke(0, 0, Height - 1, Width - 1);
            }

            MaskedGameBoard(string[] vs) : base(vs) { }

            /// <summary>
            /// 在某个格子放置或移除障碍物
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void SwitchBarrier(int x, int y)
            {
                if (NotInRange(x, y))
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (this[x, y] == GameBoardBlock.Barrier)
                {
                    this[x, y] = GameBoardBlock.Nothing;
                }
                else
                {
                    this[x, y] = GameBoardBlock.Barrier;
                }
            }

            public FullPlayerGameBoard GetFullPlayerGameBoard() => new FullPlayerGameBoard(ToStrings());
            public PlayerViewGameBoard GetPlayerViewGameBoard() => new PlayerViewGameBoard(ToStrings());
        }
    }
}
