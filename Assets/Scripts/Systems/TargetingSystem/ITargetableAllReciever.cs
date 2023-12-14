using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.Systems.TargetingSystem
{
    public interface ITargetableAllReciever : ITargetableCollisionReciever,ITargetableExitReciever,ITargetableEntryReciever
    {
    }

    public interface ITargetableEntryReciever : ITargetableReciever
    {
        void RecieveTargetableEntry(ITargetAbleEntity targetable);
    }
    
    public interface ITargetableExitReciever : ITargetableReciever
    {
        void RecieveTargetableExit(ITargetAbleEntity targetable);
    }
    
    public interface ITargetableCollisionReciever : ITargetableReciever
    {
        void RecieveCollision(Collider2D other, IOType ioType);
    }

    public interface ITargetableReciever
    {
    }
}