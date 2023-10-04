using UnityEngine;

namespace Tzipory.Helpers
{
    public class TEMP_SoloZFixer : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Tick this V to keep this script in the sceneType. If this is false -> the script will destory itself after start
        /// </summary>
        [SerializeField] bool doGoOn;

        Vector3 _cachedScaledMapSize;

        void Start()
        {
            _spriteRenderer.transform.localPosition = new Vector3(_spriteRenderer.transform.localPosition.x,
                _spriteRenderer.transform.localPosition.y, TEMP_UnitFlipAndZFix.GetZForLocalPosition(transform));

            if (!doGoOn)
                Destroy(this);
        }

        private void Update()
        {

            _spriteRenderer.transform.localPosition = new Vector3(_spriteRenderer.transform.localPosition.x,
                _spriteRenderer.transform.localPosition.y, TEMP_UnitFlipAndZFix.GetZForLocalPosition(transform));
        }
    }
}