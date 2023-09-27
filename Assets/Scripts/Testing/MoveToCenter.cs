using System.Collections;
using System.Collections.Generic;
using Tzipory.Tools.TimeSystem;
using UnityEngine;


namespace Tzipory.Testing
{
    public class MoveToCenter : MonoBehaviour
    {
        [SerializeField] float speed;

        void Update()
        {
            transform.Translate((Vector3.zero - transform.position).normalized * speed * GAME_TIME.GameDeltaTime,
                Space.World); //moves to center   
        }
    }
}