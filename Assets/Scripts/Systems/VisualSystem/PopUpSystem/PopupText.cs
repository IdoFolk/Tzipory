using System;
using System.Collections;
using TMPro;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopupText : MonoBehaviour , IInitialization<PopUpTextConfig> , IPoolable<PopupText>
{
    [SerializeField] private TMP_Text _text;
    
    private float _riseSpeed;
    private float _timeToLive;

    private float _timeAlive;

    private AnimationCurve _moveSpeedAnimationCurve;
    private AnimationCurve _scaleAnimationCurve;
    private AnimationCurve _alphaAnimationCurve;

    private Vector3 _startSize;

    private float _moveOffSet;
    
    public bool IsInitialization { get; private set; }
    
    private void Awake()
    {
        IsInitialization = false;
        _timeAlive = 0f;
    }
    private void Update()
    {
        if (!IsInitialization)
            return;
        
        transform.Translate(Vector3.up * ((_moveSpeedAnimationCurve.Evaluate(_timeAlive / _timeToLive) + _moveOffSet) * _riseSpeed * Time.deltaTime));
        transform.localScale = _startSize + Vector3.one * (_scaleAnimationCurve.Evaluate(_timeAlive / _timeToLive));
        _text.alpha = 100 - 100 * _alphaAnimationCurve.Evaluate(_timeAlive / _timeToLive);
        _timeAlive += Time.deltaTime;
    }
    
    public void Init(PopUpTextConfig parameter)
    {
        _startSize = parameter.OverrideStartSize ? parameter.StartSize : Vector2.zero;
        transform.localScale = _startSize;
        _text.text = parameter.Text;
        _text.color = parameter.Color;
        _text.fontSize = parameter.FontSize;
        
        if (parameter.OverrideRiseSpeedAndTTL)
        {
            _riseSpeed = parameter.RiseSpeed;
            _timeToLive = parameter.TimeToLive;
        }
        else
        {
            _riseSpeed = PopUpTextManager.Instance.DefaultPopUpConfig.RiseSpeed;
            _timeToLive = PopUpTextManager.Instance.DefaultPopUpConfig.TimeToLive;
        }
        

        if (parameter.OverrideAnimationCurve)
        {
            _moveSpeedAnimationCurve  = parameter.PopUpTextMoveCurve;
            _scaleAnimationCurve = parameter.PopUpTextScaleCurve;
            _alphaAnimationCurve = parameter.PopUpTextAlphaCurve;
        }
        else
        {
            _moveSpeedAnimationCurve = PopUpTextManager.Instance.PopUpTextMoveCurve;
            _scaleAnimationCurve = PopUpTextManager.Instance.PopUpTextScaleCurve;
            _alphaAnimationCurve = PopUpTextManager.Instance.PopUpTextAlphaCurve;
        }
        
        if (parameter.AddRandomMoveOffSet)
            _moveOffSet = Random.Range(parameter.MoveOffSet.x, parameter.MoveOffSet.y);
        else
            _moveOffSet = 0;
        
        StartCoroutine(nameof(KillMe));
        
        IsInitialization = true;
    }
    
    IEnumerator KillMe() //THIS SHOULD GO BACK IN THE POOL INSTEAD!
    {
        yield return new WaitForSeconds(_timeToLive);
        Destroy(gameObject);
    }

    #region ObjectPool
    
    public event Action<PopupText> OnDispose;
    public void Dispose()
    {
        OnDispose?.Invoke(this);
    }

    public void Free()
    {
        OnDispose?.Invoke(this);
        Destroy(gameObject);
    }
    
    #endregion
    
    
}
