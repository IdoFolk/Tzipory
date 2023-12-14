using UnityEngine;

public class MaskSpriteHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteMask _spriteMask;

    private void Start()
    {
        _spriteMask.sprite = _spriteRenderer.sprite;
        Destroy(this);
    }

    private void OnValidate()
    {
        _spriteRenderer ??= GetComponent<SpriteRenderer>();
        _spriteMask ??= GetComponent<SpriteMask>();
        
        _spriteMask.sprite = _spriteRenderer.sprite;
    }
}
