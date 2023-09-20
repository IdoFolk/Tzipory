using System;
using Tzipory.Helpers;
using Tzipory.Nodes.Config;
using Tzipory.Tools.Enums;
using Tzipory.Tools.Interface;
using UnityEngine;

public class NodeObject : MonoBehaviour , IInitialization<BattleMapNodeState> , IInitialization //need to make Generic and inheritance
{
    public event Action<BattleMapNodeStateConfig> OnNodeClick;

    [SerializeField] private BaseNodeConfig _baseNodeConfig;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ClickHelper _clickHelper;

    private BattleMapNodeStateConfig _stateConfig;
    public int NodeId => _baseNodeConfig.ObjectId;

    public BaseNodeConfig BaseNodeConfig => _baseNodeConfig;

    public bool IsUnlock => _baseNodeConfig.IsUnlock;//temp
    
    public bool IsInitialization { get; private set; }
    
    public void Init()
    {
        _clickHelper.OnClick += ActivateLevel;
        
        var config = (BattleMapNodeConfig)_baseNodeConfig;
        _stateConfig = config.GetBattleMapNodeStateConfigByState(config.StartingNodeState);
        _spriteRenderer.sprite = _stateConfig.OverrideSprite;
        IsInitialization = true;
    }
    
    public void Init(BattleMapNodeState parameter)
    {
        _clickHelper.OnClick += ActivateLevel;

        var config = (BattleMapNodeConfig)_baseNodeConfig;
        _stateConfig = config.GetBattleMapNodeStateConfigByState(parameter);
        _spriteRenderer.sprite = _stateConfig.OverrideSprite;
        IsInitialization = true;
    }

    private void ActivateLevel()
    {
        Debug.Log($"On level selected LevelName: {_stateConfig.LevelToOpen.name}");
        OnNodeClick?.Invoke(_stateConfig);//temp
    }
    
    private void OnValidate()
    {
        if (_clickHelper == null)
            _clickHelper = GetComponent<ClickHelper>();
        if (_spriteRenderer  == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    private void OnDestroy()
    {
        _clickHelper.OnClick -= ActivateLevel;
    }
}
