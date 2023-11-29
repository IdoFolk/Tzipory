using Tzipory.Tools.ZFixTool;
using UnityEngine;

namespace Tzipory.Helpers
{
    public class TEMP_SoloZFixer : MonoBehaviour
    {
        [SerializeField] private bool _destroyOnStart;
        
        private void Start()
        {
            FixZ();

            if (_destroyOnStart)
                Destroy(this);
        }

        private void Update()=>
            FixZ();

        private void FixZ()
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x,
                
                localPosition.y, TEMP_UnitFlipAndZFix.GetZForLocalPosition(transform));
            transform.localPosition = localPosition;
        }
    }
}