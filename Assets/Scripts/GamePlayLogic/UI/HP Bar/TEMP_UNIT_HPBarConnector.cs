using Tzipory.EntitySystem.Entitys;
using UnityEngine;

public class TEMP_UNIT_HPBarConnector : MonoBehaviour
{
    [SerializeField]
    TEMP_HP_Bar hP_Bar;

    [SerializeField] private GameObject _objWithUnit; //TEMP!
    BaseUnitEntity _unit;

    //private void Start()
    //{
    //    _unit = _objWithUnit.GetComponent<BaseUnitEntity>();
    //    if (_unit == null)
    //        return;

    //    hP_Bar.Init(_unit.Health.BaseValue);
    //    _unit.OnHealthChanged += SetBarToHealth;

    //}
    public void Init(BaseUnitEntity unit)
    {
        _unit = unit;
        hP_Bar.Init(_unit.Health.BaseValue);
        //_unit.OnHealthChanged += SetBarToHealth;
    }

    //private void OnEnable()
    //{
    //    if(_unit)
    //    _unit.OnHealthChanged += SetBarToHealth;

    ////}
    // private void OnDisable()
    // {
    //     _unit.OnHealthChanged -= SetBarToHealth;
    // }
    public void SetBarToHealth(float _health)
    {
        hP_Bar.SetBarValue(_health);
    }
}
