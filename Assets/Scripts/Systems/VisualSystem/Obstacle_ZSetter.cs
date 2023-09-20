using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.Entitys;
using UnityEngine;

//Not sure if temp -> more like "experimental"?
public class Obstacle_ZSetter : MonoBehaviour
{
    Silhouetter _silhouetter;

    [SerializeField] Transform gfxTrans;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (_unit = collision.gameObject.GetComponent<BaseUnitEntity>())
        if (_silhouetter = collision.gameObject.GetComponent<Silhouetter>())
        {
            _silhouetter.AddObstacleZ(gfxTrans.position.z);
        }
        
    } 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_silhouetter = collision.gameObject.GetComponent<Silhouetter>())
        {
            _silhouetter.RemoveObstacleZ(gfxTrans.position.z);
        }
        
    }
    
}
