using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.Helpers;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.Systems.SceneSystem;
using UnityEngine;

public class TEMP_NodeObject : MonoBehaviour
{
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private ShamanConfig[] _shamanConfigs;
    
    [SerializeField] private TEMP_NodeObject[] _nextNodes;
    [SerializeField] private ClickHelper _clickHelper;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public bool IsUnlock { get; private set; }
    public bool IsCompleted { get; private set; }

    private void Awake()
    {
        _clickHelper.OnClick += OnNodeClick;
    }

    public void Init(bool isCompleted, bool isUnLock)
    {
        IsUnlock = isUnLock;
        IsCompleted = isCompleted;

        gameObject.SetActive(IsUnlock);
        
        _spriteRenderer.color = IsCompleted ? Color.green : Color.red;
    }

    public void Lock()
    {
        gameObject.SetActive(false);
        IsUnlock = false;
    }

    private void OnDestroy()
    {
        _clickHelper.OnClick -= OnNodeClick;
    }

    public void Unlock()
    {
        _spriteRenderer.color = Color.red;
        gameObject.SetActive(true);
        GameManager.GameData.SetLockNodeStat(_levelConfig.LevelId,true);
        IsUnlock = true;
    }

    public void Completed()
    {
        _spriteRenderer.color = Color.green;
        
        foreach (var node in _nextNodes)
            node.Unlock();
    }

    private void OnNodeClick()
    {
        GameManager.GameData.LevelConfig  = _levelConfig;
        GameManager.PlayerManager.PlayerSerializeData.SetPartyData(_shamanConfigs);
        GameManager.SceneHandler.LoadScene(SceneType.Game);//temp!
    }
}