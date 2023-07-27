using Tzipory.Systems.SceneSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneHandler _sceneHandler;
    
    //playerData
    
    void Start()
    {
        _sceneHandler.LoadScene(SceneType.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Test
    [ContextMenu("LoadMap")]
    public void LoadScene()
    {
        _sceneHandler.LoadScene(SceneType.Map);
    }

    #endregion

    private void OnValidate()
    {
        if (_sceneHandler == null)
            _sceneHandler = FindObjectOfType<SceneHandler>();
    }
}
