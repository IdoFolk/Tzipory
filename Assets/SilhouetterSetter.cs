using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.Entitys;
using UnityEngine;

//Not sure if temp -> more like "experimental"?
public class SilhouetterSetter : MonoBehaviour
{
    BaseUnitEntity _unit;
    //[SerializeField] PolygonCollider2D _polyCol;

    private void Start()
    {
        //gameObject.AddComponent<PolygonCollider2D>(); //this happens so the polygon sets itself automatically to whatever sprite may be set in Awake()/Init()
    }

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
