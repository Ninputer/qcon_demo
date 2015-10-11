namespace ErrorRecoveryCombinators
{
    //X → ε
    public class EmptyProduction<T> : ProductionBase<T>
    {
        private T m_value;
        public EmptyProduction(T value)
        {
            m_value = value;
        }

        public override Parse<TFuture> BuildParse<TFuture>(Future<T, TFuture> future)
        {
            return scanner => future(m_value)(scanner);

        }
    }
}
