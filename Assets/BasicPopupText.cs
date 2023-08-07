using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicPopupText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    /// <summary>
    /// At fixed delta time
    /// </summary>
    [SerializeField] private float _moveSpeed;
    /// <summary>
    /// Time to Live
    /// </summary>
    [SerializeField] private float _timeToLive;

    float _timeAlive;
    Vector3 _originalocalScale;
    private void Awake()
    {
        _timeAlive = 0f;
        _originalocalScale = transform.localScale;
    }
    private void FixedUpdate()
    {
        //transform.Translate(Vector3.up * _riseSpeed);
        transform.Translate(Vector3.up * LevelVisualData_Monoton.Instance.PopUpText_MoveCurve.Evaluate(_timeAlive / _timeToLive) * _moveSpeed);
        transform.localScale = _originalocalScale * (LevelVisualData_Monoton.Instance.PopUpText_ScaleCurve.Evaluate(_timeAlive / _timeToLive));
        _timeAlive += Time.fixedDeltaTime;
    }


    // public void Set(string text, PopUpText_Config popUpText_Config)
    //{
    //    _text.text = text;
    //    _text.color = popUpText_Config.color;
    //    _text.fontSize = popUpText_Config.size;
    //    _riseSpeed = popUpText_Config.riseSpeed;

    //    //Calling the kill
    //    Invoke("Unset", popUpText_Config.TTL);
    //}
    public void Set(PopUpText_Config popUpText_Config)
    {
        _text.text = popUpText_Config.text;
        _text.color = popUpText_Config.color;
        _text.fontSize = popUpText_Config.size;
        _moveSpeed = popUpText_Config.riseSpeed;
        _timeToLive = popUpText_Config.timeToLive;
        //Calling the kill
        //Invoke("Unset", popUpText_Config.TTL);

        StartCoroutine(nameof(KillMe));
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
        //_riseSpeed = 0;

        gameObject.SetActive(false);
    }

}
