using UnityEngine;
using Image = UnityEngine.UI.Image;

public class TotemPlacementUIHandler : MonoBehaviour
{
    [SerializeField] private Image _splash;
    private bool _isActive;

    private void Update()
    {
        if (_isActive)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void ToggleSprite(bool state)
    {
        _splash.enabled = state;
        _isActive = state;
    }
}
