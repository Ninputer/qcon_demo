using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "1 +++ 2 *** 3";
            SourceReader sr = new SourceReader(new StringReader(input));
            var startPoint = sr.CreateRevertPoint();

            Console.WriteLine("Input String: ");
            Console.WriteLine(input);
            Console.WriteLine();

            SimpleCombinatorsTest test1 = new SimpleCombinatorsTest();
            test1.Test(sr);
            sr.Revert(startPoint);

            CpsCombinatorsTest test2 = new CpsCombinatorsTest();
            test2.Test(sr);
            sr.Revert(startPoint);

            ErrorRecoveryCombinatorsTest test3 = new ErrorRecoveryCombinatorsTest();
            test3.Test(sr);
            sr.Revert(startPoint);

            GLRCombinatorsTest test4 = new GLRCombinatorsTest();
            test4.Test(sr);
            sr.Revert(startPoint);
        }
    }
}
