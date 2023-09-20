using Tzipory.GameplayLogic.EntitySystem.TempleCore;
using UnityEngine;

namespace GameplayLogic.UI.HPBar
{
    public class TEMP_Temple_HPBarConnector : MonoBehaviour
    {
        [SerializeField] TEMP_HP_Bar hP_Bar;

        //IEntityHealthComponent healthComponent; //TBF after IEntityHealthComponent has its own method for subbing to an OnValueChanged
        [SerializeField] CoreTemple coreTemple;

        private void Awake()
        {
            hP_Bar.Init(coreTemple.Health.BaseValue);
        }

        private void OnEnable()
        {
            coreTemple.OnHealthChanged += SetBarToHealth;
        }

        private void OnDisable()
        {
            coreTemple.OnHealthChanged -= SetBarToHealth;
        }

        void SetBarToHealth()
        {
            //hP_Bar.SetBarValueSmoothly(coreTemple.Health.CurrentValue);
            hP_Bar.SetBarValue(coreTemple.Health.CurrentValue);
        }


    }
}