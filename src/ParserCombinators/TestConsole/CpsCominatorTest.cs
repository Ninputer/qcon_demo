using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    using CpsCombinators;
    using System.IO;
    using VBF.Compilers;
    using VBF.Compilers.Scanners;
    using RE = VBF.Compilers.Scanners.RegularExpression;

    class CpsCombinatorsTest
    {
        private ScannerInfo m_scannerInfo;

        private Token PLUS;
        private Token ASTERISK;
        private Token LEFT_PARENTHESIS;
        private Token RIGHT_PARENTHESIS;
        private Token NUMBER;
        private Token SPACE;

        private void SetUpScanner()
        {
            var lexion = new Lexicon();

            var lexer = lexion.Lexer;

            PLUS = lexer.DefineToken(RE.Symbol('+'));
            ASTERISK = lexer.DefineToken(RE.Symbol('*'));
            LEFT_PARENTHESIS = lexer.DefineToken(RE.Symbol('('));
            RIGHT_PARENTHESIS = lexer.DefineToken(RE.Symbol(')'));
            NUMBER = lexer.DefineToken(RE.Range('0', '9').Many1(), "number");
            SPACE = lexer.DefineToken(RE.Symbol(' ').Many1());

            m_scannerInfo = lexion.CreateScannerInfo();
        }

        private ProductionBase<int> SetUpParser()
        {
            ProductionBase<int> T = null;

            ProductionBase<int> Num = from n in NUMBER.AsTerminal() select Int32.Parse(n);

            ProductionBase<int> U = Grammar.Union(
                Num,
                from lp in LEFT_PARENTHESIS.AsTerminal()
                from exp in T
                from rp in RIGHT_PARENTHESIS.AsTerminal()
                select exp
                );

            ProductionBase<IEnumerable<int>> F1 = null;
            F1 = Grammar.Union(
                from op in ASTERISK.AsTerminal()
                from u in U
                from f1 in F1
                select new[] { u }.Concat(f1),
                Grammar.Empty(Enumerable.Empty<int>())
                );

            ProductionBase<int> F =
                from u in U
                from f1 in F1
                select f1.Aggregate(u, (a, i) => a * i);

            ProductionBase<IEnumerable<int>> T1 = null;
            T1 = Grammar.Union(
                from op in PLUS.AsTerminal()
                from f in F
                from t1 in T1
                select new[] { f }.Concat(t1),
                Grammar.Empty(Enumerable.Empty<int>())
                );

            T =
                from f in F
                from t1 in T1
                select t1.Aggregate(f, (a, i) => a + i);

            ProductionBase<int> E = from t in T
                from eos in Grammar.Eos()
                select t;

            return E;
        }

        public void Test()
        {
            SetUpScanner();
            var production = SetUpParser();

            ForkableScannerBuilder fsb = new ForkableScannerBuilder(m_scannerInfo);
            fsb.SetTriviaTokens(SPACE.Index);

            SourceReader sr = new SourceReader(new StringReader("1 + 2 * 3"));
            var scanner = fsb.Create(sr);

            var runner = new ParserRunner<int>(production);

            var result = runner.Execute(scanner);

            ;
        }
    }
}
