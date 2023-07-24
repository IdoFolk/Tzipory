using Sirenix.OdinInspector;
using UnityEngine;

namespace SerializeData.StatSerializeData
{
    [System.Serializable]
    public class StatConfig
    {
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private int _id;
        [SerializeField] private float _baseValue;

        public string Name
        {
            get => _name;
#if UNITY_EDITOR
            set => _name = value;
#endif
        }
        public int Id
        {
            get => _id;
#if UNITY_EDITOR
            set => _id = value;
#endif
        }
        public float BaseValue => _baseValue;
    }
}