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
            public PatternGameBoard(int w, int h) : base(w, h)
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
            int _cpt = 1;
            /// <summary>
            /// 每个玩家应该拥有多少个这种单位
            /// </summary>
            public int CountPerTeam
            {
                get
                {
                    return _cpt;
                }
                set
                {
                    if (value > 0)
                    {
                        _cpt = value;
                    }
                }
            }

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

            int TransformedX(int x, int y)
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
            int TransformedY(int x, int y)
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
            public override GameBoardBlock this[int x, int y]
            {
                get
                {
                    return base[TransformedX(x, y), TransformedY(x, y)];
                }
                protected set
                {
                    base[TransformedX(x, y), TransformedY(x, y)] = value;
                }
            }

            /// <summary>
            /// 切换指定格子机头状态
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void SwitchHead(int x, int y)
            {
                if (NotInRange(x, y))
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (this[x, y] == GameBoardBlock.ModelHead)
                {
                    HeadCount--;
                    this[x, y] = GameBoardBlock.Nothing;
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
            public void SetBody(int x, int y)
            {
                if (NotInRange(x, y))
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (this[x, y] == GameBoardBlock.ModelHead || this[x, y] == GameBoardBlock.ModelBody)
                {
                    this[x, y] = GameBoardBlock.Nothing;
                }
                else
                {
                    this[x, y] = GameBoardBlock.ModelBody;
                }
            }
        }
    }
}
