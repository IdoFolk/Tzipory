using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class PopUpTexter 
{
    [SerializeField] GameObject _textBoxPrefab;
    [SerializeField] Transform _textSpawnPoint;

    [SerializeField] Gradient _textGrad;
    

    public void SpawnPopUp(string text, PopUpText_Config config)
    {
        //Position is going to assume as the texter's position for now
        BasicPopupText popupText = GameObject.Instantiate(_textBoxPrefab, _textSpawnPoint.position, Quaternion.identity).GetComponent<BasicPopupText>();
        popupText.Set(text, config);
    }

}

[System.Serializable]
public struct PopUpText_Config
{
    public Color color;
    public float size;
    public float riseSpeed;
    public float TTL;
}

//[CreateAssetMenu()]
//public class PopUpText_Config_SO : ScriptableObject
//{
//    public PopUpText_Config config;
//}


