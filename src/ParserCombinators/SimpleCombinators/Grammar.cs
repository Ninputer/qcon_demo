using System;
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
                        l.Value.Content, scanner);
                }
                throw new Exception(
                    "Expect token: " + token.Description +
                    " at: " + l.Value.Span.StartLocation);
            };
        }

        //X → ε
        public static Parse<T> Empty<T>(T value)
        {
            return scanner => new Result<T>(value, scanner);
        }

        //X → A | B
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
                catch (Exception ex)
                {
                    return parse2(scanner);
                }
            };
        }
        //X → A B
        public static Parse<TR> SelectMany<T1, T2, TR>(
            this Parse<T1> parse1, 
            Func<T1, Parse<T2>> parse2Selector, 
            Func<T1, T2, TR> resultSelector)
        {
            return scanner =>
            {
                var r1 = parse1(scanner);
                var r2 = parse2Selector(r1.Value)(r1.Rest);
                return new Result<TR>(
                    resultSelector(r1.Value, r2.Value),
                    r2.Rest);
            };
        }

        //X → A
        public static Parse<TR> Select<T, TR>(
            this Parse<T> parse1,
            Func<T, TR> resultSelector)
        {
            return scanner =>
            {
                var r = parse1(scanner);
                return new Result<TR>(
                   resultSelector(r.Value),
                   r.Rest);
            };
        }

        //X → $
        public static Parse<string> Eos()
        {
            return scanner =>
            {
                var l = scanner.Read();
                if (l.IsEndOfStream)
                {
                    return new Result<string>(
                        l.Value.Content, scanner);
                }
                throw new Exception(
                    "Expect end of stream" +
                    " at: " + l.Value.Span.StartLocation);
            };
        }
    } 
}
