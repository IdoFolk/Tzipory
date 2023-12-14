using System.Collections.Generic;
using Tzipory.GamePlayLogic.EntitySystem;
using UnityEngine;

public class Silhouetter : MonoBehaviour
{
    [SerializeField] private UnitEntity _unit;   
    [SerializeField] private SpriteRenderer _silhouetteSpriteRenderer;

    private List<float> _obstaclesZs = new List<float>();

    #region Unity Callbacks (OnEnable and OnDisable subs)
    private void OnEnable()
    {
        // _unit.OnSetSprite += SetSprite;
        // _unit.OnSpriteFlipX += SetFlipX;
    }
    private void OnDisable()
    {
        // _unit.OnSetSprite -= SetSprite;
        // _unit.OnSpriteFlipX -= SetFlipX;
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// This is the method to sub to a BaseUnitEntity's OnSpriteChange
    /// </summary>
    /// <param name="toSet"></param>
    private void SetSprite(Sprite toSet)
    {
        _silhouetteSpriteRenderer.sprite = toSet;
    }
    private void SetFlipX(bool doFlip)
    {
        _silhouetteSpriteRenderer.flipX= doFlip;
    }

    private float GetCurrentTopObstacleZ()
    {
        float toReturn = float.PositiveInfinity;
        if (_obstaclesZs == null || _obstaclesZs.Count == 0)
            return toReturn;

        foreach (var item in _obstaclesZs)
        {
            if (item < toReturn)
                toReturn = item;
        }
        return toReturn;
    }
    #endregion

    #region Public Methods (Receive and Remove Z values)
    public void AddObstacleZ(float z)
    {
        _obstaclesZs.Add(z);
        _silhouetteSpriteRenderer.material.SetFloat("_ObstacleZ", GetCurrentTopObstacleZ());
    }
    public void RemoveObstacleZ(float z)
    {
        _obstaclesZs.Remove(z);
        _silhouetteSpriteRenderer.material.SetFloat("_ObstacleZ", GetCurrentTopObstacleZ());
    }
    #endregion
}
