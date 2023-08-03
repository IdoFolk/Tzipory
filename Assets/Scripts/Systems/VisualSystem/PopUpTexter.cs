using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TextSpawnRepeatPatterns {PingPong, LoopRight, LoopLeft };
[System.Serializable]
public class PopUpTexter 
{
    [SerializeField] TextSpawnRepeatPatterns _repeatPattern;
    [SerializeField] GameObject _textBoxPrefab;
    [SerializeField] Transform _textSpawnPoint;

    [SerializeField] float _lineMagnitude;
    [SerializeField] float _lineStep;
    float _currentSinePlace = 0f;
    
     public void SpawnPopUp(PopUpText_Config config)
    {
        //Position is going to assume as the texter's position for now
        Vector3 value = _textSpawnPoint.localPosition;
        switch (_repeatPattern)
        {
            case TextSpawnRepeatPatterns.PingPong:
                value.x = Mathf.Sin(_currentSinePlace) * _lineMagnitude;
                _currentSinePlace += _lineStep;

                break;
            case TextSpawnRepeatPatterns.LoopRight:
                if (_currentSinePlace >= _lineMagnitude)
                    _currentSinePlace = _lineMagnitude * -1f;

                value.x = _currentSinePlace;
                _currentSinePlace += _lineStep;

                break;
            case TextSpawnRepeatPatterns.LoopLeft:
                if (_currentSinePlace <= _lineMagnitude *-1f)
                    _currentSinePlace = _lineMagnitude;

                value.x = _currentSinePlace;
                _currentSinePlace -= _lineStep;
                break;
            default:
                break;
        }
        _textSpawnPoint.localPosition = value;
        
        BasicPopupText popupText = GameObject.Instantiate(_textBoxPrefab, _textSpawnPoint.position, Quaternion.identity).GetComponent<BasicPopupText>();
        popupText.Set(config);
    }

}

[System.Serializable]
public struct PopUpText_Config
{
    public string text;
    public Color color;
    public float size;
    public float riseSpeed;
    public float TTL;
}
