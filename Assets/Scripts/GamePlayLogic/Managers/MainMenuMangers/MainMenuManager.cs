using Tzipory.Systems.SceneSystem;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData.Managers.MainMenuMangers
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Play()
        {
            GameManager.SceneHandler.LoadScene(SceneType.Game);
        }
    }
}