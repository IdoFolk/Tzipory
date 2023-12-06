using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

public class Shadow : MonoBehaviour, IEntityStatComponent
{
    [SerializeField] private SpriteRenderer _shadowRenderer;
    [SerializeField] private SpriteRenderer _proximityRenderer;
    [SerializeField] private SpriteMask _mask;

    [SerializeField] private LineRenderer _lineRenderer;


    public bool IsOn;

    private Transform _shamanTrans;

    public Shaman Shaman { get; private set; }

    public void SetShadow(Shaman shaman,Transform shamanTrans, Sprite shadowSprite, float range)
    {
        StatHandler = new StatHandler(this);
        StatHandler = shaman.StatHandler; //fix this!!!

        gameObject.SetActive(true);
        IsOn = true;
        //_agentNavMesh = agentNavMesh;
        Shaman = shaman;
        Stats = shaman.Stats;
        _shamanTrans = shamanTrans;
        _shadowRenderer.sprite = shadowSprite;
        _mask.sprite = shadowSprite;
        _lineRenderer.gameObject.SetActive(true);
        _shadowRenderer.gameObject.SetActive(true);
        _proximityRenderer.transform.localScale = new Vector3(range, range, 1);
        //_agent.transform.position = _shamanTrans.position;

        //_agent.speed = 0; //make sure it doesnt really move
        //_agent.SetDestination(transform.position);
    }

    public void ClearShadow()
    {
        IsOn = false;
        _lineRenderer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsOn) // need to be IsOn
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new Vector3[] { _shamanTrans.position, transform.position });
            //TEMP!
            //END TEMP!
        }
    }

    public int EntityInstanceID { get; }
    public Transform EntityTransform { get; }
    public BaseGameEntity GameEntity { get; }
    public Dictionary<int, Stat> Stats { get; private set; }
    public IEnumerable<IStatHolder> GetNestedStatHolders()
    {
        List<IStatHolder> statHolders = new List<IStatHolder>() {this};

        return statHolders;
    }

    public StatHandler StatHandler { get; private set; }
}
