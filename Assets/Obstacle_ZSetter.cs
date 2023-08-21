using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.Entitys;
using UnityEngine;

//Not sure if temp -> more like "experimental"?
public class Obstacle_ZSetter : MonoBehaviour
{
    BaseUnitEntity _unit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_unit = collision.gameObject.GetComponent<BaseUnitEntity>())
        {
            _unit.AddObstacleZ(transform.position.z);
        }
        
    } 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_unit = collision.gameObject.GetComponent<BaseUnitEntity>())
        {
            _unit.RemoveObstacleZ(transform.position.z);
        }
        
    }
    
}
