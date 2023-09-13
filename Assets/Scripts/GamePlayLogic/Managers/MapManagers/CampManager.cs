using System;
using Helpers.Consts;
using Systems.CampSystem;
using Tools.Enums;
using Tzipory.ConfigFiles.WaveSystemConfig;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.MapManagers
{
    public class CampManager : MonoBehaviour, IInitialization<CampSerializeData>
    {
        public event Action OnGraphicsRefresh;
        public event Action OnCampDataChanged;
        
        #region Buildings

        [SerializeField] private Canvas _campScreen;
        

        [Header("Buildings")]
        private CampBuildingObject[] _campBuildingObjects;
        #endregion
        
        private CampSerializeData _campSerializeData;
        
        public CampSerializeData CampSerializeData => _campSerializeData;
        
        public bool IsInitialization { get; private set; }

        private void Awake()
        {
            _campScreen.gameObject.SetActive(false);
        }

        //Here for TESTING!!
        private void Start()
        {
            Init(GameManager.PlayerManager.PlayerSerializeData.CampSerializeData);
        }

        public void OpenCamp()
        {
            //Open Camp
            _campScreen.gameObject.SetActive(true);
            Debug.Log("Open camp");
        }

        public void CloseCamp()
        {
            //Close Camp
            _campScreen.gameObject.SetActive(false);
            Debug.Log("Close camp");
        }

        public void Init(CampSerializeData parameter)
        {
            _campSerializeData = parameter;
            IsInitialization = true;
            OnGraphicsRefresh?.Invoke();
        }

        #region Buildings
        
        public void RefreshCampGraphics()
        {
            foreach (CampBuildingObject campBuildingObject in _campBuildingObjects)
            {
                CampBuildingSerializeData campBuildingSerializeData =
                    _campSerializeData.GetCampBuildingData(campBuildingObject.campBuildingType);
                campBuildingObject.RefreshGraphic(campBuildingSerializeData.HighestFacilityLevel);
            }
        }

        public bool CheckIfGotEnoughResourcesForUpgrade(CampBuildingType campBuildingType, int facilityID)
        {
            return true;
        }
        
        public void UpgradeCampBuildingFacility(CampBuildingType campBuildingType, int facilityID)
        {
            if (CheckIfGotEnoughResourcesForUpgrade(campBuildingType, facilityID))
            {
                _campSerializeData.UpgradeBuilding(campBuildingType, facilityID);
            }
            else
            {
                Debug.Log("Tried to upgrade with no resources");
            }
            
            OnCampDataChanged?.Invoke();
            OnGraphicsRefresh?.Invoke();
        }

        [ContextMenu("Upgrade Workshop Items Facility")]
        public void UpgradeWorkshopItemsTest()
        {
            //Simulating an upgrade from the UI for items facility in the workhshop building
            _campSerializeData.UpgradeBuilding(CampBuildingType.Workshop, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY);
            Debug.Log(_campSerializeData.GetCampBuildingFacilityData(CampBuildingType.Workshop, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY).Level);
        }

        #endregion

        #region Party
        
        public void ModifyPartyMember(int targetShamanID, CollectionActionType actionType)
        {
            GameManager.PlayerManager.PlayerSerializeData.ModifyPartyMember(targetShamanID, actionType);
        }

        // List<int> GetSelectedShamans()
        // {
        //     //Change to whatever the ui is setting which shamans to take
        //     List<int> selectedShamans = new List<int>();
        //     
        //     foreach (ShamanPartyMemberSelectUI shamanPartyMemberSelectUI in _campUIManager.shamanToggles)
        //     {
        //         if (shamanPartyMemberSelectUI.toggle.isOn)
        //         {
        //             selectedShamans.Add(shamanPartyMemberSelectUI.AssociatedShamanID);
        //         }
        //     }
        //
        //     return selectedShamans;
        // }
        #endregion

        #region ShamanManagment

        public void ToggleItemOnShaman(int targetShamanID, int targetItemInstanceID,
            CollectionActionType actionType)
        {
            GameManager.PlayerManager.PlayerSerializeData
                .ToggleItemOnShaman(targetShamanID, targetItemInstanceID, actionType);

        }
        
        public void AttachItemToShamanTest()
        {
            //ToggleItemOnShaman(_campUIManager.shamanToggles[0].AssociatedShamanID, testingItem.ItemInstanceId, CollectionActionType.Add);
        }
        
        public void RemoveItemFromShamanTest()
        {
            //ToggleItemOnShaman(_campUIManager.shamanToggles[0].AssociatedShamanID, testingItem.ItemInstanceId, CollectionActionType.Remove);
        }

        #endregion
        
        public void Dispose()
        {
             
        }
    }
}