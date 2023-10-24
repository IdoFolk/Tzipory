using System;
using System.Globalization;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Systems.StatusSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.Systems.VisualSystem.PopUpSystem
{
    public enum TextSpawnRepeatPatterns
    {
        PingPong,
        LoopRight,
        LoopLeft
    };

    [Serializable]
    public class PopUpTexter
    {
        [SerializeField] TextSpawnRepeatPatterns _repeatPattern;
        [SerializeField] private BasicPopupText _textBoxPrefab;
        [SerializeField] private Transform _textSpawnPoint;

        [SerializeField] private float _lineMagnitude;
        [SerializeField] private float _lineStep;
        private float _currentPointerPlace = 0f;

        public void SpawnPopUp(StatChangeData config)
        {
            PopUpTextConfig popUpTextConfig;
            
            popUpTextConfig = config.UsePopUpTextConfig ? config.PopUpTextConfig : PopUpTextManager.Instance.DefaultPopUpConfig;
            
            switch (popUpTextConfig.PopUpTextType)
            {
                case PopUpTextType.ShowDelta:
                    popUpTextConfig.Text = config.Delta.ToString(CultureInfo.CurrentCulture);
                    break;
                case PopUpTextType.ShowNewValue:
                    popUpTextConfig.Text = config.NewValue.ToString(CultureInfo.CurrentCulture);
                    break;
                case PopUpTextType.ShowText:
                    popUpTextConfig.Text = config.StatEffectName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
                
            if (!popUpTextConfig.OverrideSize)
                popUpTextConfig.StartSize = PopUpTextManager.Instance.GetRelativeFontSizeForDamage(config.Delta);
            
            BasicPopupText popupText =
                Object.Instantiate(_textBoxPrefab, _textSpawnPoint.position, Quaternion.identity);
            popupText.Init(popUpTextConfig);
            
            //Old!!!
            //Resize Text //may need to be moved somewhere else TBD

            //Position is going to assume as the texter's position for now
            // Vector3 value = _textSpawnPoint.localPosition;
            // switch (_repeatPattern)
            // {
            //     case TextSpawnRepeatPatterns.PingPong:
            //         value.x = Mathf.Sin(_currentPointerPlace) * _lineMagnitude;
            //         _currentPointerPlace += _lineStep;
            //
            //         break;
            //     case TextSpawnRepeatPatterns.LoopRight:
            //         if (_currentPointerPlace >= _lineMagnitude)
            //             _currentPointerPlace = _lineMagnitude * -1f;
            //
            //         value.x = _currentPointerPlace;
            //         _currentPointerPlace += _lineStep;
            //
            //         break;
            //     case TextSpawnRepeatPatterns.LoopLeft:
            //         if (_currentPointerPlace <= _lineMagnitude * -1f)
            //             _currentPointerPlace = _lineMagnitude;
            //
            //         value.x = _currentPointerPlace;
            //         _currentPointerPlace -= _lineStep;
            //         break;
            // }
            //
            // _textSpawnPoint.localPosition = value;
            //
            // BasicPopupText popupText = GameObject
            //     .Instantiate(_textBoxPrefab, _textSpawnPoint.position, Quaternion.identity)
            //     .GetComponent<BasicPopupText>();
            // popupText.Set(config.PopUpTextConfig);
        }
    }
}