using UnityEngine;

namespace MyNamespaceTzipory.Systems.VisualSystem
{
    public enum TextSpawnRepeatPatterns
    {
        PingPong,
        LoopRight,
        LoopLeft
    };

    [System.Serializable]
    public class PopUpTexter
    {
        [SerializeField] TextSpawnRepeatPatterns _repeatPattern;
        [SerializeField] GameObject _textBoxPrefab;
        [SerializeField] Transform _textSpawnPoint;

        [SerializeField] float _lineMagnitude;
        [SerializeField] float _lineStep;
        float _currentPointerPlace = 0f;

        public void SpawnPopUp(PopUpText_Config config)
        {
            //Resize Text //may need to be moved somewhere else TBD

            //Position is going to assume as the texter's position for now
            Vector3 value = _textSpawnPoint.localPosition;
            switch (_repeatPattern)
            {
                case TextSpawnRepeatPatterns.PingPong:
                    value.x = Mathf.Sin(_currentPointerPlace) * _lineMagnitude;
                    _currentPointerPlace += _lineStep;

                    break;
                case TextSpawnRepeatPatterns.LoopRight:
                    if (_currentPointerPlace >= _lineMagnitude)
                        _currentPointerPlace = _lineMagnitude * -1f;

                    value.x = _currentPointerPlace;
                    _currentPointerPlace += _lineStep;

                    break;
                case TextSpawnRepeatPatterns.LoopLeft:
                    if (_currentPointerPlace <= _lineMagnitude * -1f)
                        _currentPointerPlace = _lineMagnitude;

                    value.x = _currentPointerPlace;
                    _currentPointerPlace -= _lineStep;
                    break;
                default:
                    break;
            }

            _textSpawnPoint.localPosition = value;

            BasicPopupText popupText = GameObject
                .Instantiate(_textBoxPrefab, _textSpawnPoint.position, Quaternion.identity)
                .GetComponent<BasicPopupText>();
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

        /// <summary>
        /// In seconds
        /// </summary>
        public float timeToLive;


        //THIS IS ONLY FOR EASY CALCULATION OF SIZES THAT NEED TO BE SET MANUALLY
//#if UNITY_EDITOR
        public float damage;

        public void SetSizeRelativeToDamage()
        {
            size = LevelVisualData_Monoton.Instance.GetRelativeFontSizeForDamage(damage);
        }
//#endif
    }
}