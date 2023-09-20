using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.SerializeData;
using Tzipory.Systems.SceneSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
    public class GameManager : MonoBehaviour
    {
        public static ISceneHandler SceneHandler { get; private set; }

        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private SceneHandler _sceneHandler;

        private static Camera _camera;

        public static GameData GameData { get; private set; }
        public static PlayerManager PlayerManager { get; private set; }

        public static Camera Camera => _camera == null ? Camera.main : _camera;

        private void Awake()
        {
            if (SceneHandler == null)
                SceneHandler = _sceneHandler;

            _camera = Camera.main;
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