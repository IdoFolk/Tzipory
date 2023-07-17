using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_OnetimeZFox : MonoBehaviour
{

    [SerializeField] SpriteRenderer _spriteRenderer;

    void Start()
    {
        Vector3 v = Tzipory.Leval.LevelManager.FakeForward;
        float f = v.x * transform.position.x + v.y * transform.position.y;

        _spriteRenderer.transform.localPosition += new Vector3(0, 0, f);

        Destroy(this);
    }
}
