﻿using System;
using VBF.Compilers.Scanners;

namespace ErrorRecoveryCombinators
{
    public static class Grammar
    {
        public static Result<T> Best<T>(Result<T> r1, Result<T> r2)
        {
            StopResult<T> stop1 = r1 as StopResult<T>;
            StopResult<T> stop2 = r2 as StopResult<T>;

            if (stop1 != null)
            {
                return stop1;
            }
            else if (stop2 != null)
            {
                return stop2;
            }

            StepResult<T> step1 = r1 as StepResult<T>;
            StepResult<T> step2 = r2 as StepResult<T>;

            if (step1.Cost < step2.Cost)
            {
                return step1;
            }
            else if (step1.Cost > step2.Cost)
            {
                return step2;
            }
            else
            {
                // 两个错误等级相同，再多Parse一步再做决定
                return new StepResult<T>(Math.Max(step1.Cost, step2.Cost), 
                    () => Best(step1.GetNextResult(), step2.GetNextResult()));
            }

        }

        public static SyntaxError RecoverByDeletion(Lexeme deleted)
        {
            return new SyntaxError(SyntaxError.UnexpectedToken,
                "Unexpected token: " + deleted.Value.Content,
                deleted.Value.Span);
        }

        public static SyntaxError RecoverByInsertion(Lexeme inserted)
        {
            return new SyntaxError(SyntaxError.MissingToken,
                "Missing token: " + inserted.Value.Content,
                inserted.Value.Span);
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

        public static ProductionBase<string> Eos()
        {
            return new EosTerminal();
        }
    }
}
