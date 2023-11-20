using Tzipory.Systems.Entity;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace GameplayLogic.UI.HPBar
{
    public class TEMP_UNIT_HPBarConnector : MonoBehaviour
    {
        [SerializeField] TEMP_HP_Bar hP_Bar;

        [SerializeField] private GameObject _objWithUnit; //TEMP!

        public void Init(float maxHP)
        {
            hP_Bar.Init(maxHP);

        }


        public void SetBarToHealth(StatChangeData statChangeData)
        {
            hP_Bar.SetBarValue(statChangeData.NewValue);
        }
    }
}