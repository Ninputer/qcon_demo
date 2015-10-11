using VBF.Compilers.Scanners;

namespace ErrorRecoveryCombinators
{
    //X → $
    public class EosTerminal : ProductionBase<string>
    {
        public override Parse<TFuture> BuildParse<TFuture>(Future<string, TFuture> future)
        {
            Parse<TFuture> parse = null;
            parse = scanner =>
            {
                Lexeme l = scanner.Read();

                if (l.IsEndOfStream)
                {
                    return new StepResult<TFuture>(0, () => future(l.Value.Content)(scanner));
                }

                return new StepResult<TFuture>(
                    1,
                    () => parse(scanner),
                    Grammar.RecoverByDeletion(l));
            };

            return parse;
        }
    }
}
