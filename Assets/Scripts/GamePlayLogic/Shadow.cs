using Tzipory.GamePlayLogic.EntitySystem;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _shadowRenderer;
    [SerializeField] private SpriteRenderer _proximityRenderer;
    [SerializeField] private SpriteMask _mask;

    [SerializeField] private LineRenderer _lineRenderer;


    public bool IsOn;

    private Transform _shamanTrans;

    public UnitEntity Shaman { get; private set; }

    public void SetShadow(UnitEntity shaman, Sprite shadowSprite, float range)
    {
        gameObject.SetActive(true);
        IsOn = true;
        Shaman = shaman;
        _shamanTrans = shaman.transform;
        _shadowRenderer.sprite = shadowSprite;
        _mask.sprite = shadowSprite;
        _lineRenderer.gameObject.SetActive(true);
        _shadowRenderer.gameObject.SetActive(true);
        _proximityRenderer.transform.localScale = new Vector3(range, range, 1);
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
        }
    }
}
