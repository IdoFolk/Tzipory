using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silhouetter : MonoBehaviour
{
    //[SerializeField] private GameObject _unitGO;
    //private Tzipory.EntitySystem.Entitys.BaseUnitEntity _unit; //this would be set on Awake with GetComponent from _unitGO
    //since it is abstract
    [SerializeField] private Shamans.Shaman _shaman;
    [SerializeField] private SpriteRenderer _silhouetteSpriteRenderer;

    private List<float> _obstaclesZs = new List<float>();
    //The current front-most (highest z value) obstacle

    private void OnEnable()
    {
        _shaman.OnSetSprite += SetSprite;
    }
    private void OnDisable()
    {
        _shaman.OnSetSprite -= SetSprite;
    }

    private void SetSprite(Sprite toSet)
    {
        _silhouetteSpriteRenderer.sprite = toSet;
    }

    float GetCurrentTopObstacleZ()
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
}
