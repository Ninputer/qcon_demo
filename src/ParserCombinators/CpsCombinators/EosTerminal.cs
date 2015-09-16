using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace CpsCombinators
{
    public class EosTerminal : ProductionBase<string>
    {
        public override Parse<TFuture> BuildParse<TFuture>(Future<string, TFuture> future)
        {
            return scanner =>
            {
                Lexeme l = scanner.Read();

                return new StepResult<TFuture>(l.IsEndOfStream, () => future(l.Value.Content)(scanner));
            };
        }
    }
}
