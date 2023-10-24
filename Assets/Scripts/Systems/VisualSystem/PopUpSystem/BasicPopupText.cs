using System.Collections;
using TMPro;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

public class BasicPopupText : MonoBehaviour , IInitialization<PopUpTextConfig>
{
    [SerializeField] private TMP_Text _text;
    
    private float _moveSpeed;
    private float _timeToLive;

    private float _timeAlive;
    private Vector3 _originalLocalScale;

    private AnimationCurve _moveSpeedAnimationCurve;
    private AnimationCurve _scaleAnimationCurve;
    private AnimationCurve _alphaAnimationCurve;

    private float _moveOffSet;
    
    public bool IsInitialization { get; private set; }
    
    private void Awake()
    {
        _timeAlive = 0f;
        _originalLocalScale = transform.localScale;
    }
    private void Update()
    {
        transform.Translate(Vector3.up * ((_moveSpeedAnimationCurve.Evaluate(_timeAlive / _timeToLive) + _moveOffSet) * _moveSpeed * Time.deltaTime));
        transform.localScale = _originalLocalScale * (_scaleAnimationCurve.Evaluate(_timeAlive / _timeToLive) * Time.deltaTime);
        _text.alpha = _alphaAnimationCurve.Evaluate(_timeAlive / _timeToLive) * Time.deltaTime;
        _timeAlive += Time.deltaTime;
    }
    
    public void Init(PopUpTextConfig parameter)
    {
        _text.text = parameter.Text;
        _text.color = parameter.Color;
        _text.fontSize = parameter.StartSize;
        _moveSpeed = parameter.RiseSpeed;
        _timeToLive = parameter.TimeToLive;

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

    public void UnSet()
    {
        _text.text = "";
        _text.color = Color.black;
        _text.fontSize = 1;
     
        gameObject.SetActive(false);
    }
}
