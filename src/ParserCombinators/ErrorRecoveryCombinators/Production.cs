using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorRecoveryCombinators
{
    // 用来构建Parse<T>的构建器
    public abstract class ProductionBase<T>
    {
        // CPS风格的构建函数
        public abstract Parse<TFuture> BuildParse<TFuture>(
            Future<T, TFuture> future // Continuation
        );
    }
}
