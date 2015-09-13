using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace SimpleCombinators
{
    public static class Grammar
    {
        //X → ‘a’
        public static Parse<string> AsTerminal(this Token token)
        {
            return scanner =>
            {
                var l = scanner.Read();
                if (l.TokenIndex == token.Index)
                {
                    return new Result<string>(
                        scanner.Read().Value.Content);
                }
                throw new Exception(
                    "Expect token: " + token.Description +
                    " at: " + l.Value.Span.StartLocation);
            };
        }

        public static Parse<T> Empty<T>(T value)
        {
            return scanner => new Result<T>(value);
        }

        public static Parse<T> Union<T>(
            this Parse<T> parse1, 
            Parse<T> parse2)
        {
            return scanner =>
            {
                try
                {
                    return parse1(scanner);
                }
                catch (Exception)
                {
                    var r = parse2(scanner);

                    if (r == null) throw;
                    return r;
                }
            };
        }

        public static Parse<TR> SelectMany<T1, T2, TR>(
            this Parse<T1> parse1, 
            Func<T1, Parse<T2>> parse2Selector, 
            Func<T1, T2, TR> resultSelector)
        {
            return scanner =>
            {
                var r1 = parse1(scanner);
                var r2 = parse2Selector(r1.Value)(scanner);
                return new Result<TR>(
                    resultSelector(r1.Value, r2.Value));
            };
        }
    } 
}
