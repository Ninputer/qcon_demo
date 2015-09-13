using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace SimpleCombinators
{
    public class Result<T>
    {
        public T Value { get; set; }
        public Result(T value) { Value = value; }
    }

    public delegate Result<T> Parse<T>(ForkableScanner s);
}
