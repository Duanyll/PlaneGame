using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        Barrier,
        Killed,
        ModelHead = 'A',
        ModelBody = 'a'
    }
    /// <summary>
    /// 表示普通的游戏版
    /// </summary>
    [Serializable]
    public abstract partial class GameBoard:ICloneable
    {
        /// <summary>
        /// 除了在构造函数中，不应直接访问此字段
        /// </summary>
        protected List<List<GameBoardBlock>> Blocks;
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }

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
        public virtual GameBoardBlock this[int x, int y]
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
        /// 构造一个具有指定大小的GameBoard，每个格子初始化为null
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public GameBoard(int w, int h)
        {
            Width = w;
            Height = h;
            Blocks = new List<List<GameBoardBlock>>();
            for (int i = 0; i < Height; i++)
            {
                Blocks.Add(new List<GameBoardBlock>());
                for (int j = 0; j < Width; j++)
                {
                    Blocks[i].Add(GameBoardBlock.Null);
                }
            }
            //BlockRangeChanged?.Invoke(0, 0, Height - 1, Width - 1);
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
                    switch (this[i,j])
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
                            tmp[j] = (char)this[i,j];
                            break;
                    }
                }
                vs[i] = new string(tmp);
            }
            return vs;
        }

        public override string ToString()
        {
            string[] vs = ToStrings();
            string ret = "";
            foreach(string i in vs)
            {
                ret += i + Environment.NewLine;
            }
            return ret.Trim();
        }

        protected bool NotInRange(int x, int y)
        {
            return x >= Height || y >= Width || x < 0 || y < 0;
        }

        public object Clone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream);
            }
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


    }
}
