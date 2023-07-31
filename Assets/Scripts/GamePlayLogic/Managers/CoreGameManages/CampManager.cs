using System;
using Systems.CampSystem;
using Tzipory.SerializeData;
using Tzipory.Tools.Interface;
using UnityEngine.Events;

namespace GameplayeLogic.Managers
{
   
    public class CampManager : IDisposable, IInitialization<CampSerializeData>
    {
        private Camp _camp;

        public event Action onGraphicsRefresh;
        
        public bool IsInitialization { get; private set; }
        
        public void Init(CampSerializeData parameter)
        {
            _camp = new Camp(parameter);
            IsInitialization = true;
            onGraphicsRefresh?.Invoke();
        }

        public void UpgradeCampBuildingFacility(CampBuildingType campBuildingType, int facilityID)
        {
            _camp.UpgradeBuilding(campBuildingType, facilityID);
        }
        
        public void Dispose()
        {
             
        }
    }
}