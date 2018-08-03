using System;
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
        /// 如果只是修改单个格子，应使用this索引器
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
        /// 表示某个特定单位模板，特色功能是可以任意设置某个格子
        /// 机身默认是a，机头默认是A
        /// </summary>
        public class PatternGameBoard : GameBoard
        {
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
        }
    }
}
