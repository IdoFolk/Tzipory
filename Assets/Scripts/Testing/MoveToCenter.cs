using System.Collections;
using System.Collections.Generic;
using Tzipory.BaseSystem.TimeSystem;
using UnityEngine;

public class MoveToCenter : MonoBehaviour
{
    [SerializeField]
    float speed;
    void Update()
    {
        transform.Translate((Vector3.zero-transform.position).normalized * speed* GAME_TIME.GameTimeDelta, Space.World); //moves to center   
    }
}
