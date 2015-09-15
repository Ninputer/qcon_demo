using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace CpsCombinators
{
    // 结果类的基类
    public abstract class Result<T>
    {
        public abstract T GetResult();
    }

    // 解析阶段的结果
    public class StepResult<T> : Result<T>
    {
        private Func<Result<T>> m_nextResultFuture;
        public bool IsValid { get; }
        public StepResult(bool isValid, Func<Result<T>> nextResultFuture)
        {
            IsValid = isValid;
            m_nextResultFuture = nextResultFuture;
        }
        public override T GetResult() => 
            m_nextResultFuture().GetResult();
    }

    // 解析结束时的结果
    public class StopResult<T> : Result<T>
    {
        private T m_result;
        public StopResult(T result) { m_result = result;}
        public override T GetResult() => m_result;
    }

    public delegate Result<T> Parse<T>(ForkableScanner s);

    // Continuation函数原型
    public delegate Parse<TFuture> Future<in T, TFuture>(T value);
}
