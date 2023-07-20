using Tzipory.SerializeData.LevalSerializeData;
using UnityEngine;

public class TEMP_SoloZFixer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    /// <summary>
    /// Tick this V to keep this script in the scene. If this is false -> the script will destory itself after start
    /// </summary>
    [SerializeField] bool doGoOn; 
    void Start()
    {
        Vector3 v = Level.FakeForward;
        float f = v.x * transform.position.x + v.y * transform.position.y;

        _spriteRenderer.transform.localPosition += new Vector3(0, 0, f);
        if(!doGoOn)
            Destroy(this);
    }

    private void Update()
    {
        Vector3 v = Level.FakeForward;
        float f = v.x * transform.position.x + v.y * transform.position.y;

        _spriteRenderer.transform.localPosition = new Vector3(_spriteRenderer.transform.localPosition.x, _spriteRenderer.transform.localPosition.y, f);
    }
}