using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.Entitys;
using Tzipory.EntitySystem.TargetingSystem;
using Tzipory.Leval;
using Tzipory.SerializeData.LevalSerializeData;
using UnityEngine;

public class TEMP_UnitFlipAndZFix : MonoBehaviour
{
    [SerializeField] private FlipPrefs _flipPrefs; //TEMP! SHOULD NOT BE SERIALIZED BUT SET VIA THE UNIT's SCRIPT!
    [SerializeField] private bool _doLookAtTemple; 
    
    [SerializeField] private SpriteRenderer _spriteRenderer;

    //Should probably be the targetting module instead
    [SerializeField] private Transform _tgt;

    Vector3 _fakeForward;

    [SerializeField] BaseUnitEntity _baseUnitEntity;
    TargetingHandler _targeting;
    //TEMP! Should USE Init(BaseUnitEntity) INSTEAD 

    Vector3 _cachedScaledMapSize;
    private void Start()
    {
        StartCoroutine(nameof(CheckForFlip));
        _targeting = _baseUnitEntity.Targeting;

        _fakeForward = Level.FakeForward;
        _cachedScaledMapSize = Level.MapSize * .01f;
        //This should be applied differently between Shamans and Enemies.
        //Enemies look in the direction they are going -> then they look at CoreTrans or their attack target.
        _tgt = _doLookAtTemple? CoreTemple.CoreTrans : null; // MUST change TBD

    }
    private void Update()
    {
        //Vector3 v = 
        //do z fix
        float f = _fakeForward.x * transform.position.x  + _fakeForward.y * transform.position.y;
        float offset = _fakeForward.x * _cachedScaledMapSize.x + _fakeForward.y * _cachedScaledMapSize.y; //should cause the bottom most point to be the flat-height
        _spriteRenderer.transform.localPosition = new Vector3(0, 0, -f);
        if (_targeting.HaveTarget)
            _tgt = _targeting.CurrentTarget.EntityTransform;
        else
            _tgt = null;
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
