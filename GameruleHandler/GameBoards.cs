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

        protected bool NotInRange(int x, int y)
        {
            return x >= Height || y >= Width || x < 0 || y < 0;
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
            /// 表示游戏版逆时针旋转
            /// </summary>
            public enum RoationMode
            {
                None,
                Turn90,
                Turn180,
                Turn270
            }

            /// <summary>
            /// 表示游戏版的翻转（先旋转再翻转）
            /// </summary>
            [Flags]
            public enum FlipMode
            {
                None = 0,
                FlipX = 1,
                FlipY = 2
            }

            public string Name { get; set; } = "No Name";

            /// <summary>
            /// 初始化一个指定大小的PatternGameBoard。元素将会初始化为Nothing
            /// </summary>
            /// <param name="w"></param>
            /// <param name="h"></param>
            public PatternGameBoard(int w,int h):base(w,h)
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

            public PatternGameBoard(string[] vs) : base(vs)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (Blocks[i][j] == GameBoardBlock.ModelHead)
                        {
                            HeadCount++;
                        }
                    }
                }
            }

            /// <summary>
            /// 获得当前机头数量
            /// </summary>
            public int HeadCount { get; private set; }
            public RoationMode Roation { get; set; } = RoationMode.None;
            public FlipMode Flip { get; set; } = FlipMode.None;

            /// <summary>
            /// 表示可能旋转后的宽度
            /// </summary>
            public override int Width
            {
                get
                {
                    if (Roation == RoationMode.Turn270 || Roation == RoationMode.Turn90)
                    {
                        return base.Height;
                    }
                    else
                    {
                        return base.Width;
                    }
                }
            }

            /// <summary>
            /// 表示可能旋转后的高度
            /// </summary>
            public override int Height
            {
                get
                {
                    if (Roation == RoationMode.Turn270 || Roation == RoationMode.Turn90)
                    {
                        return base.Width;
                    }
                    else
                    {
                        return base.Height;
                    }
                }
            }

            int TransformedX(int x,int y)
            {
                switch (Roation)
                {
                    case RoationMode.None:
                        break;
                    case RoationMode.Turn90:
                        x = y;
                        break;
                    case RoationMode.Turn180:
                        x = base.Height - x - 1;
                        break;
                    case RoationMode.Turn270:
                        x = base.Width - y - 1;
                        break;
                }
                if ((Flip & FlipMode.FlipX) == FlipMode.FlipX)
                {
                    x = Height - 1 - x;
                }
                return x;
            }
            int TransformedY(int x,int y)
            {
                switch (Roation)
                {
                    case RoationMode.None:
                        break;
                    case RoationMode.Turn90:
                        y = base.Height - x - 1;
                        break;
                    case RoationMode.Turn180:
                        y = base.Width - y - 1;
                        break;
                    case RoationMode.Turn270:
                        y = x;
                        break;
                }
                if ((Flip & FlipMode.FlipY) == FlipMode.FlipY)
                {
                    y = Width - 1 - y;
                }
                return y;
            }
            //TODO:完善与旋转有关的索引器与width，height
            /// <summary>
            /// 返回旋转/翻转后的格子
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public override GameBoardBlock this[int x,int y]
            {
                get
                {
                    return base[TransformedX(x,y), TransformedY(x,y)];
                }
                protected set
                {
                    base[TransformedX(x,y), TransformedY(x,y)] = value;
                }
            }

            /// <summary>
            /// 切换指定格子机头状态
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void SwitchHead(int x,int y)
            {
                if (NotInRange(x, y))
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
                if (NotInRange(x, y))
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

            public FullPlayerGameBoard(string[] vs) : base(vs)
            {
            }

            Dictionary<char,string> PatternChars = new Dictionary<char,string>();
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
                if (PatternChars.ContainsValue(pattern.Name))
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
                    PatternChars.Add(pchar, pattern.Name);
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
                BlockRangeChanged?.Invoke(x, y, x + pattern.Height, y + pattern.Width);
                return true;
            }

            bool NothingIn(int x,int y)
            {
                if (NotInRange(x, y))
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

            /// <summary>
            /// 尝试攻击某格。
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns>攻击结果</returns>
            public GameBoardBlock Attack(int x,int y)
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
                    }
                    return this[x,y];
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

        /// <summary>
        /// 表示玩家看对方的游戏版
        /// </summary>
        public class MaskedGameBoard : GameBoard
        {
            MaskedGameBoard(int w,int h) : base(w, h)
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

            public void SetBlock(GameBoardBlock block,int x,int y)
            {
                this[x, y] = block;
            }
        }
    }
}
