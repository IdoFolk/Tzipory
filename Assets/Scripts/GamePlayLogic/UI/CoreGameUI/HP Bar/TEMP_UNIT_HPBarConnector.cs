using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace GameplayLogic.UI.HPBar
{
    public class TEMP_UNIT_HPBarConnector : MonoBehaviour
    {
        [SerializeField] TEMP_HP_Bar hP_Bar;

        [SerializeField] private GameObject _objWithUnit; //TEMP!
        UnitEntity _unit;

        public void Init(UnitEntity unit)
        {
            _unit = unit;
            hP_Bar.Init(_unit.EntityHealthComponent.Health.BaseValue);
        }
        
        public void SetBarToHealth(StatChangeData statChangeData)
        {
            hP_Bar.SetBarValue(statChangeData.NewValue);
        }
    }
}