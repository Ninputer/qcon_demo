using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleCombinatorsTest test1 = new SimpleCombinatorsTest();
            test1.Test();

            CpsCombinatorsTest test2 = new CpsCombinatorsTest();
            test2.Test();
        }
    }
}
