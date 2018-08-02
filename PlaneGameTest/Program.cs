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
            int w, h;
            string[] tmp = Console.ReadLine().Split(' ');
            w = int.Parse(tmp[0]);
            h = int.Parse(tmp[1]);
            string[] map = new string[h];
            for(int i = 0; i < h; i++)
            {
                map[i] = Console.ReadLine();
            }
            GameBoard board = new GameBoard(map);
            map = board.ToStrings();
            foreach (var i in map)
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }
    }
}
