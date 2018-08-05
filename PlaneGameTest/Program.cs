using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameruleHandler;

namespace PlaneGameTest
{
    public class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            DoTest();
#endif
        }

        public static void DoTest()
        {
            Console.WriteLine("已启动测试");

            //测试GameBoard的构造与ToStrings
            Console.WriteLine("输入原始PlayerGB");
            int w, h;
            string[] tmp = Console.ReadLine().Split(' ');
            w = int.Parse(tmp[0]);
            h = int.Parse(tmp[1]);
            string[] map = new string[h];
            //for(int i = 0; i < h; i++)
            //{
            //    map[i] = Console.ReadLine();
            //}
            GameBoard.FullPlayerGameBoard board = new GameBoard.FullPlayerGameBoard(w,h);
            map = board.ToStrings();
            foreach (var i in map)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("输入PatternGB");
            tmp = Console.ReadLine().Split(' ');
            w = int.Parse(tmp[0]);
            h = int.Parse(tmp[1]);
            map = new string[h];
            for (int i = 0; i < h; i++)
            {
                map[i] = Console.ReadLine();
            }
            GameBoard.PatternGameBoard pboard = new GameBoard.PatternGameBoard(map);
            pboard.Roation = GameBoard.PatternGameBoard.RoationMode.Turn270;
            map = pboard.ToStrings();
            foreach (var i in map)
            {
                Console.WriteLine(i);
            }

            while (true)
            {
                Console.WriteLine("输入插入x,y坐标");
                tmp = Console.ReadLine().Split(' ');
                w = int.Parse(tmp[0]);
                h = int.Parse(tmp[1]);
                Console.WriteLine( board.PutPatern(pboard, w, h,GameBoard.CornorMode.All));
                map = board.ToStrings();
                foreach (var i in map)
                {
                    Console.WriteLine(i);
                }
            }

            Console.ReadKey();
        }
    }
}
