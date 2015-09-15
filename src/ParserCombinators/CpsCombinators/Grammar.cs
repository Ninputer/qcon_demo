using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace CpsCombinators
{
    public static class Grammar
    {
        public static Result<TFuture> Best<TFuture>(Result<TFuture> parse1, Result<TFuture> parse2)
        {
            throw new NotImplementedException();
        }

        //X → ‘a’
        public static ProductionBase<string> AsTerminal(this Token token)
        {
            return new Terminal(token);
        }

        //X → ε
        public static ProductionBase<T> Empty<T>(T value)
        {
            return new EmptyProduction<T>(value);
        }

        //X → A
        //X → B
        public static ProductionBase<T> Union<T>(
            this ProductionBase<T> p1,
            ProductionBase<T> p2)
        {
            return new AlternationProduction<T>(p1, p2);
        }
        //X → A B
        public static ProductionBase<TR> SelectMany<T1, T2, TR>(
            this ProductionBase<T1> p1,
            Func<T1, ProductionBase<T2>> p2Selector,
            Func<T1, T2, TR> resultSelector)
        {
            return new ConcatenationProduction<T1, T2, TR>(p1, p2Selector, resultSelector);
        }

        //X → A
        public static ProductionBase<TR> Select<T, TR>(
            this ProductionBase<T> p1,
            Func<T, TR> resultSelector)
        {
            return new MappingProduction<T, TR>(p1, resultSelector);
        }
    }
}
