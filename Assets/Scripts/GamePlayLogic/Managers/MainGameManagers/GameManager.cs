using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.SerializeData;
using Tzipory.Systems.CameraSystem;
using Tzipory.Systems.SceneSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
    public class GameManager : MonoBehaviour
    {
        public static ISceneHandler SceneHandler { get; private set; }

        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private SceneHandler _sceneHandler;

        private static CameraHandler _cameraHandler;

        public static GameData GameData { get; private set; }
        public static PlayerManager PlayerManager { get; private set; }

        public static CameraHandler CameraHandler => _cameraHandler;

        private void Awake()
        {
            if (SceneHandler == null)
                SceneHandler = _sceneHandler;

            _cameraHandler = FindObjectOfType<CameraHandler>();//May need to change 
            GameData = new GameData();
        }

        void Start()
        {
            SceneHandler.LoadScene(SceneType.MainMenu);

            var playerSerializeData = DataManager.DataRequester.GetData<PlayerSerializeData>(_playerConfig);
            PlayerManager = new PlayerManager(playerSerializeData);
        }

        #region Test

        [ContextMenu("LoadMap")]
        public void LoadScene()
        {
            SceneHandler.LoadScene(SceneType.Map);
        }

        private void OnDestroy()
        {
            GameData = null;
            PlayerManager = null;
        }

        #endregion

        private void OnValidate()
        {
            if (_sceneHandler == null)
                _sceneHandler = FindObjectOfType<SceneHandler>();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}