using Systems.UISystem;
using Tzipory.SerializeData;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIHandler : BaseUIElement , IInitialization<ShamanDataContainer>
{
    [SerializeField] private Image _characterImage;
    
    public bool IsInitialization { get; }
    public void Init(ShamanDataContainer parameter)
    {
        _characterImage.sprite = parameter.UnitEntityVisualConfig.Sprite;
    }
}
