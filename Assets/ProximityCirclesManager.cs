using UnityEngine;

public class ProximityCirclesManager : MonoBehaviour
{
    [SerializeField] private Transform[] _circleScalers;

    public void ScaleCircles(float range1, float range2, float range3)
    {
        
        for (int i = 0; i < _circleScalers.Length; i++)
        {
            switch (i)
            {
            
            }
            //_circleScalers[i].localScale = new Vector3(range, range, circle.localScale.z);
        }
    }
}