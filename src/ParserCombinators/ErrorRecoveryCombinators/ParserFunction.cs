﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace ErrorRecoveryCombinators
{

    // 结果类的基类
    public abstract class Result<T>
    {
        public abstract T GetResult(IList<SyntaxError> errors);
    }

    // 解析阶段的结果
    public class StepResult<T> : Result<T>
    {
        private Func<Result<T>> m_nextResultFuture;
        public int Cost { get; }
        private SyntaxError m_error;
        public StepResult(int cost, Func<Result<T>> nextResultFuture, SyntaxError err = null)
        {
            Cost = cost;
            m_nextResultFuture = nextResultFuture;
            m_error = err;
        }
        public override T GetResult(IList<SyntaxError> errors)
        {
            if (m_error != null)
            {
                errors.Add(m_error);
            }
            return m_nextResultFuture().GetResult(errors);
        }

        public Result<T> GetNextResult() =>
            m_nextResultFuture();

    }

    // 解析结束时的结果
    public class StopResult<T> : Result<T>
    {
        private T m_result;
        public StopResult(T result) { m_result = result; }
        public override T GetResult(IList<SyntaxError> errors) => m_result;
    }

    public delegate Result<T> Parse<T>(ForkableScanner s);

    // Continuation函数原型
    public delegate Parse<TFuture> Future<in T, TFuture>(T value);
}
