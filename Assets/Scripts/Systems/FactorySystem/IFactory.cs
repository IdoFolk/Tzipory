
namespace Tzipory.Systems.FactorySystem
{
    public interface IFactory<out T>
    {
            /// <summary>
        /// Creates an object of type T.
        /// </summary>
        /// <returns>The created object.</returns>
        public T Create();
    }
    
    public interface IFactory<out T1, in T2>
    {
        /// <summary>
        /// Creates an object of type T.
        /// </summary>
        /// <returns>The created object.</returns>
        public T1 Create(T2 t2);
    }
    
    public interface IFactory<out T1, in T2,in T3>
    {
        /// <summary>
        /// Creates an object of type T.
        /// </summary>
        /// <returns>The created object.</returns>
        public T1 Create(T2 t2,T3 t3);
    }
    
    public interface IFactory<out T1, in T2,in T3,in T4>
    {
        /// <summary>
        /// Creates an object of type T.
        /// </summary>
        /// <returns>The created object.</returns>
        public T1 Create(T2 t2,T3 t3,T4 t4);
    }
}