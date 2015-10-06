using System;
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
        public IEnumerable<SyntaxError> MyErrors { get; set; }
        public abstract T GetResult(IList<SyntaxError> errors);
    }

    // 解析阶段的结果
    public class StepResult<T> : Result<T>
    {
        public int Cost { get; }

        private Func<Result<T>> m_nextResultFuture;
        private SyntaxError m_error;
        private Result<T> m_nextResult;

        public StepResult(int cost, Func<Result<T>> nextResultFuture, SyntaxError err = null)
        {
            Cost = cost;
            m_nextResultFuture = nextResultFuture;
            m_error = err;

            MyErrors = Enumerable.Empty<SyntaxError>();
        }
        public override T GetResult(IList<SyntaxError> errors)
        {
            return GetNextResult().GetResult(errors);
        }

        public Result<T> GetNextResult()
        {
            if (m_nextResult == null)
            {
                m_nextResult = m_nextResultFuture();
                m_nextResult.MyErrors = 
                    MyErrors.Concat(m_nextResult.MyErrors);
                if (m_error != null)
                {
                    m_nextResult.MyErrors = 
                        m_nextResult.MyErrors.Concat(new[] { m_error });
                }
            }
            
            return m_nextResult;
        }

    }

    // 解析结束时的结果
    public class StopResult<T> : Result<T>
    {
        private T m_result;
        public StopResult(T result)
        {
            MyErrors = Enumerable.Empty<SyntaxError>();
            m_result = result;
        }
        public override T GetResult(IList<SyntaxError> errors)
        {
            foreach (var err in MyErrors)
            {
                errors.Add(err);
            }
            return m_result;
        }
    }

    public delegate Result<T> Parse<T>(ForkableScanner s);

    // Continuation函数原型
    public delegate Parse<TFuture> Future<in T, TFuture>(T value);
}
