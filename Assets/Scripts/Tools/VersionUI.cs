using TMPro;
using UnityEngine;

namespace Tzipory.Tools
{
    public class VersionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            _text.text = $"{Application.productName} V_{Application.version}";
            Destroy(this);
        }
    }
}