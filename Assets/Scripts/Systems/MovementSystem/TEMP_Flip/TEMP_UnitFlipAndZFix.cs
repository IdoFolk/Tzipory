using System.Collections;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

public class TEMP_UnitFlipAndZFix : MonoBehaviour
{
    [SerializeField] private FlipPrefs _flipPrefs; //TEMP! SHOULD NOT BE SERIALIZED BUT SET VIA THE UNIT's SCRIPT!
    [SerializeField] private bool _doLookAtTemple; 
    
    [SerializeField] private SpriteRenderer _spriteRenderer;

    //Should probably be the targetting module instead
    [SerializeField] private Transform _tgt;
    [SerializeField] private float _zDistanceModifier;

    

    [SerializeField] UnitEntity _baseUnitEntity;
    IEntityTargetingComponent _targeting;
    //TEMP! Should USE SetShamanData(BaseUnitEntity) INSTEAD 

    static Vector3 cachedScaledMapSize => LevelHandler.MapSize * .01f; //fix take from consts
    static float zDistanceModifier = .5f; //fix take from consts

    public static float GetZForLocalPosition(Transform t)
    {
        float newZ = LevelHandler.FakeForward.x * t.position.x + LevelHandler.FakeForward.y * t.position.y;
        float mapOffset = Mathf.Abs(LevelHandler.FakeForward.x) * cachedScaledMapSize.x + Mathf.Abs(LevelHandler.FakeForward.y) * cachedScaledMapSize.y; //should cause the bottom most point to be the flat-height

        newZ += mapOffset;
        newZ *= zDistanceModifier;
        return -newZ;
    }

    private void Start()
    {
        StartCoroutine(nameof(CheckForFlip));
        _targeting = _baseUnitEntity.EntityTargetingComponent;


        //This should be applied differently between Shamans and Enemies.
        //Enemies look in the direction they are going -> then they look at CoreTrans or their attack target.
        _tgt = _doLookAtTemple? CoreTemple.CoreTrans : null; // MUST change TBD

    }
    private void Update()
    {
        _spriteRenderer.transform.localPosition = new Vector3(_spriteRenderer.transform.localPosition.x, _spriteRenderer.transform.localPosition.y, GetZForLocalPosition(transform));

        if (_targeting.HaveTarget)
            _tgt = _targeting.CurrentTarget.GameEntity.transform;
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
                //_spriteRenderer.flipX = deltaV.x >= 0;
                _baseUnitEntity.SetSpriteFlipX( deltaV.x >= 0);
            }
            else
            {
                if (_tgt)
                {
                    //_spriteRenderer.flipX = (_tgt.position - transform.position).x > 0;
                    _baseUnitEntity.SetSpriteFlipX((_tgt.position - transform.position).x > 0);
                }
            }
        }
    }
}
