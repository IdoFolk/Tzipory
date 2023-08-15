#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnValidate_SeeThroughMaskFixer : MonoBehaviour
{
    [SerializeField]
    SpriteMask sm;
    [SerializeField]
    SpriteRenderer sr;
    SpriteMask _SpriteMask => sm ? sm : GetComponent<SpriteMask>();
    SpriteRenderer _SpriteRenderer => sr ? sr : GetComponent<SpriteRenderer>();

    void OnValidate()
    {
        _SpriteMask.sprite = _SpriteRenderer.sprite;
    }
}
#endif