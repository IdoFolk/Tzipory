using Tzipory.EntitySystem;
using UnityEngine;

namespace GameplayLogic.UI.HPBar
{
    public class TEMP_UNIT_HPBarConnector : MonoBehaviour
    {
        [SerializeField] TEMP_HP_Bar hP_Bar;

        [SerializeField] private GameObject _objWithUnit; //TEMP!
        BaseUnitEntity _unit;

        public void Init(BaseUnitEntity unit)
        {
            _unit = unit;
            hP_Bar.Init(_unit.Health.BaseValue);

        }


        public void SetBarToHealth(float _health)
        {
            hP_Bar.SetBarValue(_health);
        }
    }
}