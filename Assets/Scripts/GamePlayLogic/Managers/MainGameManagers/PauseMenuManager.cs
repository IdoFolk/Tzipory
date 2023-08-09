using Tzipory.BaseSystem.TimeSystem;
using Tzipory.Systems.SceneSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlayLogic.Managers.MainGameManagers
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private EventSystem _eventSystem;

        private bool _isOpen;
        
        private void Awake()
        {
            _canvas.gameObject.SetActive(false);
            _isOpen = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !SceneHandler.IsLoading)
                OpenPauseMenu();
            
            //TODO add the ability to close the pause menu by pressing Escape
            // if (Input.GetKeyDown(KeyCode.Escape) && _isOpen)
            //     Resume();
        }

        private void OpenPauseMenu()
        {
            _isOpen = true;
            _canvas.gameObject.SetActive(true);
            GAME_TIME.Pause();
        }

        public void Resume()
        {
            _canvas.gameObject.SetActive(false);
            GAME_TIME.Play();
            _isOpen = false;
        }
    }
}
