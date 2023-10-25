using Tzipory.ConfigFiles.Player;
using Tzipory.SerializeData;
using Tzipory.Systems.CameraSystem;
using Tzipory.Systems.DataManager;
using Tzipory.Systems.SceneSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
    public class GameManager : MonoBehaviour
    {
        public static ISceneHandler SceneHandler { get; private set; }

        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private SceneHandler _sceneHandler;
        
        public static GameData GameData { get; private set; }
        public static PlayerManager PlayerManager { get; private set; }

        public static CameraHandler CameraHandler { get; set; }         

        private void Awake()
        {
            if (SceneHandler == null)
                SceneHandler = _sceneHandler;

            CameraHandler = FindObjectOfType<CameraHandler>();//May need to change 
            GameData = new GameData();
        }

        void Start()
        {
            SceneHandler.LoadScene(SceneType.MainMenu);
            
            var playerSerializeData = DataManager.DataRequester.GetSerializeData<PlayerSerializeData>(_playerConfig);
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

        private void OnMouseDown()
        {
            //lock the cursor inside the screen
            Screen.lockCursor = true;
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}