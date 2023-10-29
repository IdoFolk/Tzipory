namespace Tzipory.Tools.Interface
{
    public interface ICopy<out T>
    {
        public T Copy();
    }
    public interface ICopy<out T,T1>
    {
        public T Copy(T1 parameter);
    }
    public interface ICopy<out T,T1,T2>
    {
        public T Copy(T1 parameter1,T2 parameter2);
    }
    public interface ICopy<out T,T1,T2,T3>
    {
        public T Copy(T1 parameter1,T2 parameter2,T3 parameter3);
    }
}
