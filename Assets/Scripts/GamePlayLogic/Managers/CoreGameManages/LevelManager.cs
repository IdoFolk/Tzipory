using System;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.ConfigFiles.Level;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.GameplayLogic.UI.Indicator;
using Tzipory.SerializeData.PlayerData.Party;
using Tzipory.Systems.SceneSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Enums;
using Tzipory.Tools.GameSettings;
using Tzipory.Tools.Sound;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tzipory.GameplayLogic.Managers.CoreGameManagers
{
    [DefaultExecutionOrder(-1)]
    public class LevelManager : MonoBehaviour
    {
        public static event Action<bool> OnEndGame;

        private PoolManager _poolManager;

        public static PartyManager PartyManager { get; private set; }
        public static EnemyManager EnemyManager { get; private set; }
        public static WaveManager WaveManager { get; private set; }
        public static CoreTemple CoreTemplete { get; private set; }

        public static bool IsWon { get; private set; }

        public bool IsGameRunning { get; private set; }

        [SerializeField, TabGroup("Party manager")]
        private UnitEntityConfig[] _shamanConfigs;

        [SerializeField, TabGroup("Level manager"), Tooltip("Make the level that you can lose or win the game")]
        private bool _cantLose;

        [SerializeField, TabGroup("Level manager")]
        private Transform _levelParent;

        [SerializeField, TabGroup("Level manager")]
        private Transform _waveIndicatorParent;

        [Header("Testing")] [SerializeField, TabGroup("Level manager")]
        private LevelConfig _levelConfig;

        [SerializeField, TabGroup("Spawn parents")]
        private Transform _shamanParent;
        
        [SerializeField, TabGroup("Spawn parents")]
        private Transform _uiIndicatorParent;

        [SerializeField, TabGroup("Spawn parents")]
        private Transform _enemiesParent;
        
        [SerializeField,PropertyOrder(-1)] private UIIndicatorConfig _uiIndicatorConfig;//only for testing TEMP
        
        private UIIndicatorHandler _uiIndicatorHandler;

        private void Awake()
        {
            _poolManager = new PoolManager();
            _uiIndicatorHandler = new UIIndicatorHandler(_uiIndicatorParent,10);

            if (GameManager.GameData == null) //for Testing(Start form level scene)
            {
                var partyData = new PartySerializeData();
                partyData.Init(_shamanConfigs);
                PartyManager = new PartyManager(partyData, _shamanParent);
            }
            else
            {
                _levelConfig = GameManager.GameData.CurrentLevelConfig;
                PartyManager = new PartyManager(GameManager.PlayerManager.PlayerSerializeData.PartySerializeData,
                    _shamanParent);
            }

            Instantiate(_levelConfig.Level, _levelParent);
            
            GameManager.CameraHandler.SetCameraSettings(_levelConfig.Level.CameraBorder,_levelConfig.Level.CameraMaxZoom, _levelConfig.Level.OverrideCameraStartPositionAndZoom,_levelConfig.Level.CameraStartPosition,_levelConfig.Level.CameraStartZoom);

            #region OnlyForTesting
#if UNITY_EDITOR
               
            if (GAME_TIME.TimerHandler is null)
                Instantiate(Resources.Load<GameObject>("Prefabs/Managers/Temp/GameTimeManager"));//only for testing
#endif
            #endregion
            
            EnemyManager = new EnemyManager(_enemiesParent);
            WaveManager = new WaveManager(_levelConfig,_uiIndicatorConfig); //temp!!!
            CoreTemplete = FindObjectOfType<CoreTemple>(); //temp!!!
            CoreTemplete.Init();
            PartyManager.SpawnShaman();
        }

        private void Start()
        {
            BgMusicManager.Instance.PlayMusic();
            GameManager.CameraHandler.ToggleCameraLock(false);
            GameManager.CameraHandler.ResetCamera();
            WaveManager.StartLevel();
            GAME_TIME.SetTimeStep(1);
            UIManager.Init(UIGroup.GameUI);
            UIManager.ShowUIGroup(UIGroup.GameUI,true);
            IsGameRunning = true;
        }

        private void Update()
        {
            if (!IsGameRunning)
                return;

            WaveManager.UpdateLevel();

            if (GameSetting.CantLose)
                return;

            if (CoreTemplete.IsDestroyed && CoreTemplete.IsInitialization)
            {
                IsWon  = false;
                EndGame(IsWon);
            }

            if (WaveManager.AllWaveAreDone && EnemyManager.AllEnemiesArDead)
            {
                IsWon = true;
                EndGame(IsWon);
            }
        }

        private void OnDestroy()
        {
            // EnemyManager.Dispose();
            // PartyManager.Dispose();
            // WaveManager.Dispose();
            //
            // PartyManager = null;
            // EnemyManager = null;
            // WaveManager = null;
        }

        private void EndGame(bool isWon)
        {
            if (!IsGameRunning) return;

            GAME_TIME.SetTimeStep(0);

            if (isWon)
                GameManager.GameData?.SetCompletedNodeStat(_levelConfig.LevelId, true);

            OnEndGame?.Invoke(isWon);
            UIManager.HidUIGroup(UIGroup.GameUI);
            UIManager.ShowUIGroup(UIGroup.EndGameUI,true);
            BgMusicManager.Instance.StopMusic();
            IsGameRunning = false;
        }

        public void Continue()
        {
            GAME_TIME.SetTimeStep(1);

            if (GameManager.SceneHandler !=
                null) //for testing so you can rest the level if you start from the coreGame scene
                GameManager.SceneHandler.LoadScene(SceneType.Map);
            else
                SceneManager.LoadScene(3); //reset the CoreGame scene so you can play again
        }

#if UNITY_EDITOR
        [Button("Win")]
        public void Win()
        {
            EndGame(true);
        }

        [Button("Lose")]
        public void Lose()
        {
            EndGame(false);
        }
#endif
    }
}