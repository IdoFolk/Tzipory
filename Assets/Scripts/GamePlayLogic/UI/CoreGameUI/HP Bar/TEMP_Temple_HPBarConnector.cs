using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace GameplayLogic.UI.HPBar
{
    public class TEMP_Temple_HPBarConnector : MonoBehaviour
    {
        [SerializeField] TEMP_HP_Bar hP_Bar;

        //IEntityHealthComponent healthComponent; //TBF after IEntityHealthComponent has its own method for subbing to an OnValueChanged
        [SerializeField] CoreTemple coreTemple;

        private void Start()
        {
            hP_Bar.Init(coreTemple.EntityHealthComponent.Health.BaseValue);
        }

        private void OnEnable()
        {
            coreTemple.EntityHealthComponent.Health.OnValueChanged += SetBarToHealth;
        }

        private void OnDisable()
        {
            coreTemple.EntityHealthComponent.Health.OnValueChanged -= SetBarToHealth;
        }

        void SetBarToHealth(StatChangeData statChangeData)
        {
            //hP_Bar.SetBarValueSmoothly(coreTemple.Health.CurrentValue);
            hP_Bar.SetBarValue(statChangeData.NewValue);
        }


    }
}