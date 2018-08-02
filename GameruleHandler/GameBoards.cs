using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 表示一个格子的内容。其实是个char类型
    /// </summary>
    public enum GameBoardBlock
    {
        Null,
        Nothing,
        Unknown
    }
    /// <summary>
    /// 表示一个玩家的游戏板
    /// </summary>
    public class GameBoard
    {
        private List<List<GameBoardBlock>> Blocks;
        public readonly int Width;
        public readonly int Height;
        /// <summary>
        /// 获取某个特定格子的状态
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GameBoardBlock this[int x, int y]
        {
            get
            {
                return Blocks[x][y];
            }
        }
        /// <summary>
        /// 构造一个具有指定大小的GameBoard
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public GameBoard(int w, int h)
        {
            Width = w;
            Height = h;
            Blocks = new List<List<GameBoardBlock>>();
        }
        /// <summary>
        /// 从现有字符串数组加载GameBoard
        /// </summary>
        /// <param name="vs"></param>
        public GameBoard(string[] vs)
        {
            Height = vs.Length;
            Width = int.MaxValue;
            for (int i = 0; i < Height; i++)
            {
                Width = Math.Min(Width, vs[i].Length);
            }
            Blocks = new List<List<GameBoardBlock>>(Height);
            for (int i = 0; i < Height; i++)
            {
                Blocks.Add(new List<GameBoardBlock>(Width));
                for (int j = 0; j < Width; j++)
                {
                    switch (vs[i][j])
                    {
                        case ' ':
                            Blocks[i].Add(GameBoardBlock.Nothing);
                            break;
                        case '?':
                            Blocks[i].Add(GameBoardBlock.Unknown);
                            break;
                        case '!':
                            Blocks[i].Add(GameBoardBlock.Null);
                            break;
                        default:
                            Blocks[i].Add((GameBoardBlock)vs[i][j]);
                            break;
                    }
                }
            }
        }
        public string[] ToStrings()
        {
            string[] vs = new string[Height];
            for (int i = 0; i < Height; i++)
            {
                char[] tmp = new char[Width];
                for (int j = 0; j < Width; j++)
                {
                    switch (Blocks[i][j])
                    {
                        case GameBoardBlock.Null:
                            tmp[j] = '!';
                            break;
                        case GameBoardBlock.Nothing:
                            tmp[j] = ' ';
                            break;
                        case GameBoardBlock.Unknown:
                            tmp[j] = '?';
                            break;
                        default:
                            tmp[j] = (char)Blocks[i][j];
                            break;
                    }
                }
                vs[i] = new string(tmp);
            }
            return vs;
        }
    }
}
