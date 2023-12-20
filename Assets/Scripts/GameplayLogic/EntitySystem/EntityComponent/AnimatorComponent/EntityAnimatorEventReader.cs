using System;
using UnityEngine;

public class EntityAnimatorEventReader : MonoBehaviour
{
    public event Action OnDeathAnimationEnded;
    public void DeathAnimationEnded()
    {
        OnDeathAnimationEnded?.Invoke();
    }
}
