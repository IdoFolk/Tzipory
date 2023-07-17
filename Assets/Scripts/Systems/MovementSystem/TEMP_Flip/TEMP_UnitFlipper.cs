using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_UnitFlipper : MonoBehaviour
{
    [SerializeField] private FlipPrefs _flipPrefs; //TEMP! SHOULD NOT BE SERIALIZED BUT SET VIA THE UNIT's SCRIPT!
    [SerializeField] private bool _doLookAtTemple; 
    
    [SerializeField] private SpriteRenderer _spriteRenderer;

    //Should probably be the targetting module instead
    [SerializeField] private Transform _tgt;

    //TEMP! MUST USE INIT INSTEAD!
    private void Start()
    {
        StartCoroutine(nameof(CheckForFlip));

        //This should be applied differently between Shamans and Enemies.
        //Enemies look in the direction they are going -> then they look at CoreTrans or their attack target.
        _tgt = _doLookAtTemple? CoreTemple.CoreTrans : null;
    }
    public void Init(FlipPrefs fp)
    {
        _flipPrefs = fp;

        //start timer/coroutine/whatever repeating invoke or what have you
        StartCoroutine(nameof(CheckForFlip));
    }

    //TEMP - should be TIMER based
    IEnumerator CheckForFlip()
    {
        Vector3 lastPos;
        while(true)
        {
            lastPos = transform.position;
            yield return new WaitForSeconds(1f / _flipPrefs.TestFreq);
            var deltaV = transform.position - lastPos;
            if (deltaV.sqrMagnitude >= _flipPrefs.DeadZone)
            {
                _spriteRenderer.flipX = deltaV.x >= 0;
            }
            else
            {
                if(_tgt)
                {
                    _spriteRenderer.flipX = (_tgt.position - transform.position).x >0;

                }
            }
            //lastPos = transform.position; //it'll happen either way when it loops back around before it yields to wait
        }
    }
}
