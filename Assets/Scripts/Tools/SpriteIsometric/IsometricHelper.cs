using UnityEngine;

namespace Tzipory.Tools.SpriteIsometric
{
    public class IsometricHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private float _scale = 1;
        [SerializeField, Range(0, 1)] private float _isoOffSet = 1;

        private void OnValidate()
        {
            transform.localScale = new Vector3(_scale, _scale * _isoOffSet, 0);
        }
#endif
    }
}