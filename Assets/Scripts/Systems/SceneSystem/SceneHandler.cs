using System;
using System.Collections;
using Tzipory.Tools.LoadingScreen;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tzipory.Systems.SceneSystem
{
    public class SceneHandler : MonoBehaviour , ISceneHandler
    {
        public static event Action<SceneType> OnSceneLoaded;
        
        private const int MAIN_MENU_SCENE_INDEX = 1;
        private const int MAP_SCENE_INDEX = 2;
        private const int GAME_SCENE_INDEX = 3;
        
        [SerializeField] private float _minLoadTime;
        
        [SerializeField] private LoadingScreenHandler _loadingScreenHandler;
        
        private float _loadTime;
        
        public static Scene PresistanteScene { get; private set; }
        public static Scene CurrentScene { get; private set; }

        private void Start()
        {
            PresistanteScene = SceneManager.GetActiveScene();
        }

        private IEnumerator LoadSceneAsync(SceneType sceneType)
        {
            _loadTime = 0;
            
            int sceneIndex = sceneType switch
            {
                SceneType.MainMenu => MAIN_MENU_SCENE_INDEX,
                SceneType.Map => MAP_SCENE_INDEX,
                SceneType.Game => GAME_SCENE_INDEX,
                _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
            };

            Scene preventsScene = SceneManager.GetActiveScene();

            if (preventsScene.buildIndex != 0)
            {
                SceneManager.SetActiveScene(PresistanteScene);
                Debug.Log($"Unloading scene {preventsScene.name}");

                yield return _loadingScreenHandler.FadeIn();
                
                var unloadSceneAsync = SceneManager.UnloadSceneAsync(preventsScene);
                
                yield return SceneLoaderAndUnLoader(unloadSceneAsync);
                
                Debug.Log($"Unloaded scene {preventsScene.name}");
            }

            Debug.Log($"start loading sceneType");
            
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneIndex,LoadSceneMode.Additive);
            
            loadSceneAsync.allowSceneActivation = false;

            yield return SceneLoaderAndUnLoader(loadSceneAsync);

            if (_loadTime < _minLoadTime)
            {
                var wait = new WaitForSeconds(_minLoadTime - _loadTime);
                yield return wait;
            }
            
            CurrentScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            
            yield return  _loadingScreenHandler.FadeOut();
            
            SceneManager.SetActiveScene(CurrentScene);
            
            OnSceneLoaded?.Invoke(sceneType);
            
            Debug.Log($"Loaded sceneType {CurrentScene.name}");
        }

        private IEnumerator SceneLoaderAndUnLoader(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                _loadTime += Time.deltaTime;
                
                if (asyncOperation.progress  >= 0.9f)
                    asyncOperation.allowSceneActivation = true;
                
                Debug.Log($"loading progress {asyncOperation.progress * 100}%");
                yield return null;
            }
        }

        public void LoadScene(SceneType sceneType)
        {
            StartCoroutine(LoadSceneAsync(sceneType));
        }

        private void OnValidate()
        {
            if (_loadingScreenHandler == null)
                _loadingScreenHandler = GetComponent<LoadingScreenHandler>();
        }
    }

    public interface ISceneHandler
    {
        public void LoadScene(SceneType sceneType);
    }

    public enum SceneType
    {
        MainMenu,
        Map,
        Game
    }
}