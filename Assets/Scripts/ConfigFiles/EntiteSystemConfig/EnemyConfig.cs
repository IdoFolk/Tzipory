using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityConfigSystem
{
    [CreateAssetMenu(fileName = "New enemy config", menuName = "ScriptableObjects/Entity/New enemy config", order = 0)]
    public class EnemyConfig : BaseUnitEntityConfig
    {
        [SerializeField,TabGroup("AI")] private float _decisionInterval;//temp
        [SerializeField,TabGroup("AI")] private float _aggroLevel;//temp
        [SerializeField,TabGroup("AI")] private float _returnLevel;//temp

        public float DecisionInterval => _decisionInterval;

        public float AggroLevel => _aggroLevel;

        public float ReturnLevel => _returnLevel;
    }
}