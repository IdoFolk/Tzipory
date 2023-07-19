using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.Entitys;
using Tzipory.EntitySystem.TargetingSystem;
using Tzipory.Leval;
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
    private void Start()
    {
        StartCoroutine(nameof(CheckForFlip));
        _fakeForward = LevelManager.FakeForward;
        _targeting = _baseUnitEntity.Targeting;
        //This should be applied differently between Shamans and Enemies.
        //Enemies look in the direction they are going -> then they look at CoreTrans or their attack target.
        _tgt = _doLookAtTemple? CoreTemple.CoreTrans : null;
    }
    private void Update()
    {
        //do z fix
        float f = _fakeForward.x * transform.position.x  + _fakeForward.y * transform.position.y;

        _spriteRenderer.transform.localPosition = new Vector3(0, 0, f);

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
