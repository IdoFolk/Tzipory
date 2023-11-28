using Tzipory.Tools.ZFixTool;
using UnityEngine;

namespace Tzipory.Helpers
{
    public class TEMP_SoloZFixer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private bool _doGoOn;
        
        private void Start()
        {
            FixZ();

            if (!_doGoOn)
                Destroy(this);
        }

        private void Update()=>
            FixZ();

        private void FixZ()
        {
            var localPosition = _spriteRenderer.transform.localPosition;
            localPosition = new Vector3(localPosition.x,
                
                localPosition.y, TEMP_UnitFlipAndZFix.GetZForLocalPosition(transform));
            _spriteRenderer.transform.localPosition = localPosition;
        }
    }
}