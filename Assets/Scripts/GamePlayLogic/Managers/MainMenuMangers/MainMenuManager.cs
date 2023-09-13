using Tzipory.Systems.SceneSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.WaveSystemConfig.Managers.MainMenuMangers
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Play()
        {
            GameManager.SceneHandler.LoadScene(SceneType.Map);
        }
    }
}