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
    [SerializeField] private float _riseSpeed;
    /// <summary>
    /// Time to Live
    /// </summary>
    [SerializeField] private float _ttl;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * _riseSpeed);
    }


     public void Set(string text, PopUpText_Config popUpText_Config)
    {
        _text.text = text;
        _text.color = popUpText_Config.color;
        _text.fontSize = popUpText_Config.size;
        _riseSpeed = popUpText_Config.riseSpeed;

        //Calling the kill
        Invoke("Unset", popUpText_Config.TTL);
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
