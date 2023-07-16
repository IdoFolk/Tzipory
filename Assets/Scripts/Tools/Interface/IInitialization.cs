namespace Tzipory.Tools.Interface
{
    public interface IInitialization
    {
        public bool IsInitialization { get; }

        public void Init();
    }
    
    public interface IInitialization<in T>
    {
        public bool IsInitialization { get; }
        public void Init(T parameter);
    }
    
    public interface IInitialization<in T1, in T2>
    {
        public bool IsInitialization { get; }

        public void Init(T1 parameter1,T2 parameter2);
    }
    
    public interface IInitialization<in T1, in T2, in T3>
    {
        public bool IsInitialization { get; }

        public void Init(T1 parameter1,T2 parameter2,T3 parameter3);
    }
    
    public interface IInitialization<in T1, in T2, in T3, in T4>
    {
        public bool IsInitialization { get; }

        public void Init(T1 parameter1,T2 parameter2,T3 parameter3,T4 parameter4);
    }
    
    public interface IInitialization<in T1, in T2, in T3, in T4, in T5>
    {
        public bool IsInitialization { get; }

        public void Init(T1 parameter1,T2 parameter2,T3 parameter3,T4 parameter4,T5 parameter5);
    }
}