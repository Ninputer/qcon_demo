using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorRecoveryCombinators
{
    public class AlternationProduction<T> : ProductionBase<T>
    {
        private ProductionBase<T> m_p1;
        private ProductionBase<T> m_p2;

        public AlternationProduction(ProductionBase<T> p1, ProductionBase<T> p2)
        {
            m_p1 = p1;
            m_p2 = p2;
        }

        public override Parse<TFuture> BuildParse<TFuture>(Future<T, TFuture> future)
        {
            return scanner =>
            {
                return Grammar.Best(
                    m_p1.BuildParse(future)(scanner),
                    m_p2.BuildParse(future)(scanner)
                    );
            };
        }
    }
}
