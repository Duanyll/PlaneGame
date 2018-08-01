using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneGameTest
{
    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            PlaneGame.App.Main();
#else

#endif
        }
    }
}
