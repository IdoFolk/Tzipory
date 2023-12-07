using Tzipory.GameplayLogic.EntitySystem.Shamans;
using UnityEngine;

public class Shadow : MonoBehaviour
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
        gameObject.SetActive(true);
        IsOn = true;
        //_agentNavMesh = agentNavMesh;
        Shaman = shaman;
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
}
