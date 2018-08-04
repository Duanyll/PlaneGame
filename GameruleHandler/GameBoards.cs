﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameruleHandler
{
    /// <summary>
    /// 表示一个格子的内容。其实是个char类型
    /// 用二十六个英文大小写字母表示不同类型单位的机身与机头
    /// </summary>
    public enum GameBoardBlock
    {
        Null,
        Nothing,
        Unknown,
        ModelHead = 'A',
        ModelBody = 'a'
    }
    /// <summary>
    /// 表示普通的游戏版
    /// </summary>
    public class GameBoard
    {
        /// <summary>
        /// 除了在构造函数中，不应直接访问此字段
        /// </summary>
        protected List<List<GameBoardBlock>> Blocks;
        public readonly int Width;
        public readonly int Height;

        /// <summary>
        /// 某个格子改变时引发的事件
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        public delegate void BlockChangedDelegate(int x1, int y1);
        public event BlockChangedDelegate BlockChanged;

        /// <summary>
        /// 某个范围改变时引发的事件
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public delegate void BlockRangeChangedDel(int x1, int y1, int x2, int y2);
        public event BlockRangeChangedDel BlockRangeChanged;

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
            protected set
            {
                Blocks[x][y] = value;
                BlockChanged?.Invoke(x,y);
            }
        }
        /// <summary>
        /// 不应该调用
        /// </summary>
        public GameBoard()
        {
            
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
            BlockRangeChanged?.Invoke(0, 0, Height - 1, Width - 1);
        }
        /// <summary>
        /// 将当前Game Board转换为字符串数组
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 是否允许两个单位连通
        /// </summary>
        [Flags]
        public enum CornorMode
        {
            /// <summary>
            /// 允许任意连通
            /// </summary>
            All = 0,
            /// <summary>
            /// 不允许边相连
            /// </summary>
            NoEdge = 1,
            /// <summary>
            /// 不允许角相连
            /// </summary>
            NoCornor = 2
        }

        /// <summary>
        /// 表示某个特定单位模板，特色功能是可以任意设置某个格子
        /// 机身默认是a，机头默认是A
        /// </summary>
        public class PatternGameBoard : GameBoard
        {
            /// <summary>
            /// 表示游戏版的旋转
            /// </summary>
            public enum RoationMode
            {
                None,
                Turn90,
                Turn180,
                Turn270
            }

            /// <summary>
            /// 表示游戏版的翻转
            /// </summary>
            [Flags]
            public enum FlipMode
            {
                None = 0,
                FlipX = 1,
                FlipY = 2
            }

            /// <summary>
            /// 初始化一个指定大小的PatternGameBoard。元素将会初始化为Nothing
            /// </summary>
            /// <param name="w"></param>
            /// <param name="h"></param>
            public PatternGameBoard(int w,int h):base(w,h)
            {
                for(int i = 0; i < Height; i++)
                {
                    for(int j = 0; j < Width; j++)
                    {
                        Blocks[i][j] = GameBoardBlock.Nothing;
                    }
                }
                BlockRangeChanged?.Invoke(0, 0, Height - 1, Width - 1);
            }

            /// <summary>
            /// 获得当前机头数量
            /// </summary>
            public int HeadCount { get; private set; }
            public RoationMode Roation { get; set; } = RoationMode.None;
            public FlipMode Flip { get; set; } = FlipMode.None;

            /// <summary>
            /// 切换指定格子机头状态
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void SwitchHead(int x,int y)
            {
                if (x > Height || y > Width || x < 0 || y < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (this[x, y] == GameBoardBlock.ModelHead)
                {
                    HeadCount--;
                    this[x,y] = GameBoardBlock.Nothing;
                }
                else
                {
                    HeadCount++;
                    this[x, y] = GameBoardBlock.ModelHead;
                }
            }

            /// <summary>
            /// 切换指定格子机身状态
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void SetBody(int x,int y)
            {
                if (x > Height || y > Width || x < 0 || y < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (this[x, y] == GameBoardBlock.ModelHead || this[x,y]==GameBoardBlock.ModelBody)
                {
                    this[x, y] = GameBoardBlock.Nothing;
                }
                else
                {
                    this[x, y] = GameBoardBlock.ModelBody;
                }
            }
        }

        /// <summary>
        /// 表示一队玩家的完整游戏版，特色是可以判断与放置PatternGameBoard
        /// </summary>
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

            Dictionary<char,PatternGameBoard> PatternChars = new Dictionary<char,PatternGameBoard>();
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
            public bool PutPatern(PatternGameBoard pattern,int x,int y,CornorMode cornor = CornorMode.All)
            {
                if (x > Height || y > Width || x < 0 || y < 0)
                {
                    return false;
                }
                if (x + pattern.Height > Height || y + pattern.Width > Width)
                {
                    return false;
                }
                for(int iout = x; iout < x + pattern.Height; iout++)
                {
                    int iin = iout - x;
                    for(int jout = y; jout < y + pattern.Width; jout++)
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
                if (PatternChars.ContainsValue(pattern))
                {
                    foreach(char i in PatternChars.Keys)
                    {
                        if(PatternChars[i].Equals(pattern))
                        {
                            pchar = i;
                            break;
                        }
                    }
                }
                else
                {
                    pchar = NewPatternChar;
                    PatternChars.Add(pchar, pattern);
                    NewPatternChar++;
                }
                for (int iout = x; iout < x + pattern.Height; iout++)
                {
                    int iin = iout - x;
                    for (int jout = y; jout < y + pattern.Width; jout++)
                    {
                        int jin = jout - y;
                        if(pattern[iin,jin] == GameBoardBlock.ModelBody)
                        {
                            this[iout, jout] = (GameBoardBlock)pchar;
                        }
                        if (pattern[iin, jin] == GameBoardBlock.ModelHead)
                        {
                            this[iout, jout] = (GameBoardBlock)char.ToUpper(pchar);
                        }
                    }
                }
                return true;
            }

            bool NothingIn(int x,int y)
            {
                if (x > Height || y > Width || x < 0 || y < 0)
                {
                    return true;
                }
                return this[x, y] == GameBoardBlock.Nothing;
            }
            bool CheckSurroundings(int x,int y,CornorMode mode)
            {
                if (!NothingIn(x,y))
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
        }
    }
}
