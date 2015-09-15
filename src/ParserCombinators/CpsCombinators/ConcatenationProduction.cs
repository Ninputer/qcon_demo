using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpsCombinators
{
    public class ConcatenationProduction<T1, T2, TR> : ProductionBase<TR>
    {
        private ProductionBase<T1> m_p1;
        private Func<T1, ProductionBase<T2>> m_p2Selector;
        private Func<T1, T2, TR> m_resultSelector;

        public ConcatenationProduction(
            ProductionBase<T1> p1, 
            Func<T1, ProductionBase<T2>> p2Selector,
            Func<T1, T2, TR> resultSelector)
        {
            m_p1 = p1;
            m_p2Selector = p2Selector;
            m_resultSelector = resultSelector;
        }

        public override Parse<TFuture> BuildParse<TFuture>(Future<TR, TFuture> future)
        {
            return scanner =>
                m_p1.BuildParse(
                    value1 => m_p2Selector(value1).BuildParse(
                        value2 => future(m_resultSelector(value1, value2)))(scanner);
        }
    }
}
