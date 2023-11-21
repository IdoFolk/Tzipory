using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.Tools.Interface
{
    public interface ITargetableAllReciever : ITargetableCollisionReciever,ITargetableExitReciever,ITargetableEntryReciever
    {
    }

    public interface ITargetableEntryReciever : ITargetableReciever
    {
        void RecieveTargetableEntry(IEntityTargetAbleComponent targetable);
    }
    
    public interface ITargetableExitReciever : ITargetableReciever
    {
        void RecieveTargetableExit(IEntityTargetAbleComponent targetable);
    }
    
    public interface ITargetableCollisionReciever : ITargetableReciever
    {
        void RecieveCollision(Collider2D other, IOType ioType);
    }

    public interface ITargetableReciever
    {
    }
}