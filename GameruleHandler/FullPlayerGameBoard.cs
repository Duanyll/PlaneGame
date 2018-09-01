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
        /// 表示一队玩家的完整游戏版，特色是可以判断与放置PatternGameBoard
        /// </summary>
        [Serializable]
        public class FullPlayerGameBoard : GameBoard
        {
            public FullPlayerGameBoard(int w, int h) : base(w, h)
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

            public FullPlayerGameBoard(string[] vs) : base(vs)
            {
            }

            Dictionary<char, string> PatternChars = new Dictionary<char, string>();
            char NewPatternChar = 'a';

            public int PlaneCount { get; private set; } = 0;
            public int HeadCount { get; private set; } = 0;

            /// <summary>
            /// 尝试在给定坐标处放置单位
            /// </summary>
            /// <param name="pattern"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="cornor"></param>
            /// <returns>放置是否成功</returns>
            public bool PutPatern(PatternGameBoard pattern, int x, int y, CornorMode cornor = CornorMode.All)
            {
                if (x > Height || y > Width || x < 0 || y < 0)
                {
                    return false;
                }
                if (x + pattern.Height > Height || y + pattern.Width > Width)
                {
                    return false;
                }
                for (int iout = x; iout < x + pattern.Height; iout++)
                {
                    int iin = iout - x;
                    for (int jout = y; jout < y + pattern.Width; jout++)
                    {
                        int jin = jout - y;
                        if (pattern[iin, jin] == GameBoardBlock.ModelBody || pattern[iin, jin] == GameBoardBlock.ModelHead)
                        {
                            if (!CheckSurroundings(iout, jout, cornor))
                            {
                                return false;
                            }
                        }
                    }
                }
                PlaneCount++;
                HeadCount += pattern.HeadCount;
                char pchar = 'a';
                if (PatternChars.ContainsValue(pattern.Name))
                {
                    foreach (char i in PatternChars.Keys)
                    {
                        if (PatternChars[i].Equals(pattern))
                        {
                            pchar = i;
                            break;
                        }
                    }
                }
                else
                {
                    pchar = NewPatternChar;
                    PatternChars.Add(pchar, pattern.Name);
                    NewPatternChar++;
                }
                for (int iout = x; iout < x + pattern.Height; iout++)
                {
                    int iin = iout - x;
                    for (int jout = y; jout < y + pattern.Width; jout++)
                    {
                        int jin = jout - y;
                        if (pattern[iin, jin] == GameBoardBlock.ModelBody)
                        {
                            this[iout, jout] = (GameBoardBlock)pchar;
                        }
                        if (pattern[iin, jin] == GameBoardBlock.ModelHead)
                        {
                            this[iout, jout] = (GameBoardBlock)char.ToUpper(pchar);
                        }
                    }
                }
                BlockRangeChanged?.Invoke(x, y, x + pattern.Height, y + pattern.Width);
                return true;
            }

            bool NothingIn(int x, int y)
            {
                if (NotInRange(x, y))
                {
                    return true;
                }
                return this[x, y] == GameBoardBlock.Nothing;
            }

            bool CheckSurroundings(int x, int y, CornorMode mode)
            {
                if (!NothingIn(x, y))
                {
                    return false;
                }
                if ((mode & CornorMode.NoEdge) == CornorMode.NoEdge)
                {
                    if (!(NothingIn(x + 1, y) && NothingIn(x - 1, y) && NothingIn(x, y + 1) && NothingIn(x, y - 1)))
                    {
                        return false;
                    }
                }
                if ((mode & CornorMode.NoCornor) == CornorMode.NoCornor)
                {
                    if (!(NothingIn(x + 1, y + 1) && NothingIn(x - 1, y + 1) && NothingIn(x + 1, y - 1) && NothingIn(x - 1, y - 1)))
                    {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>
            /// 尝试攻击某格。
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns>攻击结果</returns>
            public GameBoardBlock Attack(int x, int y)
            {
                if (NotInRange(x, y))
                {
                    return GameBoardBlock.Null;
                }
                else
                {
                    if (char.IsUpper((char)this[x, y]))
                    {
                        HeadCount--;
                        this[x, y] = GameBoardBlock.Killed;
                    }
                    return this[x, y];
                }
            }

            /// <summary>
            /// 获取指定block的名字
            /// </summary>
            /// <param name="block"></param>
            /// <returns></returns>
            public string CheckName(GameBoardBlock block)
            {
                if (char.IsLetter((char)block))
                {
                    if (char.IsUpper((char)block))
                    {
                        block = (GameBoardBlock)char.ToLower((char)block);
                    }
                    if (PatternChars.ContainsKey((char)block))
                    {
                        return PatternChars[(char)block];
                    }
                    else
                    {
                        return "Unknown Plane";
                    }
                }
                else
                {
                    return block.ToString();
                }
            }
        }
    }
}
