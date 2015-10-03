using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    using VBF.Compilers;
    using VBF.Compilers.Parsers;
    using VBF.Compilers.Scanners;
    using RE = VBF.Compilers.Scanners.RegularExpression;

    class GLRCombinatorsTest : ParserBase<int>
    {
        public GLRCombinatorsTest(CompilationErrorManager em) : base(em) { }

        private Token PLUS;
        private Token ASTERISK;
        private Token LEFT_PARENTHESIS;
        private Token RIGHT_PARENTHESIS;
        private Token NUMBER;
        private Token SPACE;

        protected override void OnDefineLexer(Lexicon lexicon, ICollection<Token> triviaTokens)
        {
            var lexer = lexicon.Lexer;

            PLUS = lexer.DefineToken(RE.Symbol('+'));
            ASTERISK = lexer.DefineToken(RE.Symbol('*'));
            LEFT_PARENTHESIS = lexer.DefineToken(RE.Symbol('('));
            RIGHT_PARENTHESIS = lexer.DefineToken(RE.Symbol(')'));
            NUMBER = lexer.DefineToken(RE.Range('0', '9').Many1(), "number");
            SPACE = lexer.DefineToken(RE.Symbol(' ').Many1());
        }

        protected override ProductionBase<int> OnDefineGrammar()
        {
            var T = new Production<int>();

            ProductionBase<int> Num = from n in NUMBER select Int32.Parse(n.Value.Content);

            ProductionBase<int> U =
                Num |
                from lp in LEFT_PARENTHESIS
                from exp in T
                from rp in RIGHT_PARENTHESIS
                select exp;

            var F = new Production<int>();
            F.Rule =
                U |
                from left in F
                from op in ASTERISK
                from right in U
                select left * right;

            T.Rule =
                F |
                from left in T
                from op in PLUS
                from right in F
                select left * right;

            ProductionBase<int> E = from t in T
                                    from eos in Grammar.Eos()
                                    select t;

            return E;
        }
    }
}
