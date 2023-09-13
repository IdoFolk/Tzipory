using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.Entitys;
using Tzipory.EntitySystem.TargetingSystem;
using Tzipory.Leval;
using Tzipory.ConfigFiles.WaveSystemConfig;
using UnityEngine;

public class TEMP_UnitFlipAndZFix : MonoBehaviour
{
    [SerializeField] private FlipPrefs _flipPrefs; //TEMP! SHOULD NOT BE SERIALIZED BUT SET VIA THE UNIT's SCRIPT!
    [SerializeField] private bool _doLookAtTemple; 
    
    [SerializeField] private SpriteRenderer _spriteRenderer;

    //Should probably be the targetting module instead
    [SerializeField] private Transform _tgt;
    [SerializeField] private float _zDistanceModifier;

    

    [SerializeField] BaseUnitEntity _baseUnitEntity;
    TargetingHandler _targeting;
    //TEMP! Should USE Init(BaseUnitEntity) INSTEAD 

    static Vector3 cachedScaledMapSize => Level.MapSize * .01f; //fix take from consts
    static float zDistanceModifier = .5f; //fix take from consts

    public static float GetZForLocalPosition(Transform t)
    {
        float newZ = Level.FakeForward.x * t.position.x + Level.FakeForward.y * t.position.y;
        float mapOffset = Mathf.Abs(Level.FakeForward.x) * cachedScaledMapSize.x + Mathf.Abs(Level.FakeForward.y) * cachedScaledMapSize.y; //should cause the bottom most point to be the flat-height

        newZ += mapOffset;
        newZ *= zDistanceModifier;
        return -newZ;
    }

    private void Start()
    {
        StartCoroutine(nameof(CheckForFlip));
        _targeting = _baseUnitEntity.TargetingHandler;


        //This should be applied differently between Shamans and Enemies.
        //Enemies look in the direction they are going -> then they look at CoreTrans or their attack target.
        _tgt = _doLookAtTemple? CoreTemple.CoreTrans : null; // MUST change TBD

    }
    private void Update()
    {
        _spriteRenderer.transform.localPosition = new Vector3(_spriteRenderer.transform.localPosition.x, _spriteRenderer.transform.localPosition.y, GetZForLocalPosition(transform));

        if (_targeting.HaveTarget)
            _tgt = _targeting.CurrentTarget.EntityTransform;
        else
            _tgt = null;
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
        }
    }
}
