using VBF.Compilers.Scanners;

namespace CpsCombinators
{
    //X → ‘a’
    public class Terminal : ProductionBase<string>
    {
        private Token m_token;
        public Terminal(Token token)
        {
            m_token = token;
        }

        public override Parse<TFuture> BuildParse<TFuture>(Future<string, TFuture> future)
        {
            return scanner =>
            {
                Lexeme l = scanner.Read();

                return new StepResult<TFuture>(l.TokenIndex == m_token.Index, 
                    () => future(l.Value.Content)(scanner));
            };
        }
    }
}
