using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace ErrorRecoveryCombinators
{
    public class ParserRunner<T>
    {
        private Parse<T> m_parseFunc;

        public ParserRunner(ProductionBase<T> parseFunc)
        {
            m_parseFunc = parseFunc.BuildParse(FinalFuture);
        }

        public T Execute(ForkableScanner scanner)
        {
            var result = m_parseFunc(scanner);

            return result.GetResult();
        }

        private Parse<T> FinalFuture(T value)
        {
            return scanner => new StopResult<T>(value);
        }
    }
}
