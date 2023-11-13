using System;

namespace Tzipory.Tools.Interface
{
    public interface IObjectDisposable : IDisposable
    {
        public abstract int ObjectInstanceId { get; }
    }
}