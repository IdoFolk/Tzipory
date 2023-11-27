using PathCreation;
using Tzipory.GameplayLogic.EntitySystem.Enemies;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.EntityComponents;
using UnityEngine;

namespace Tzipory.Systems.MovementSystem
{
    public class MovementOnPath : MonoBehaviour
    {
        #region TempRefs

        [SerializeField] TEMP_BasicMoveComponent _tempBasicMoveComponent; //will be set with init

        [SerializeField] private float privateRabbitIncrement; //will be set with config
        [SerializeField] private float acceptableDistanceFromPath; //?
        [SerializeField] private float acceptableDistanceToCompletion = 2f; //??

        [SerializeField] private float finalLoopSpeed; //I think I need to clean this spot a bit more

        private float privateRabbitProgress; //this is actually fine... the name can change

        private Vector3
            _currentPointOnPath; //considering caging this in some #if UNITY_EDITOR with elses just so these don't bother us later...

        private PathCreator pathCreator; //will be set with init
        private PathCreator finalDestinaion; //will be set with init

        #endregion

        public Vector3 CurrentPointOnPath => _currentPointOnPath;

#if UNITY_EDITOR
        [SerializeField] private float rabbitGizmoBoxSize = 1f;
        [SerializeField,
         Tooltip(
             "Set to true if you want this Unit's Rabbit-gizmo to draw at all times (Default: false, only draws gizmo for this Unit when it is Selected in the editor (inspector)")]
        private bool alwaysShowGizmo = false;
#endif

        /// </summary>
        /// <param name="pc"></param>
        /// <param name="finalDest"></param>
        public void SetPath(PathCreator pc)
        {
            privateRabbitProgress = 0;
            pathCreator = pc;
            //finalDestinaion = finalDest;
            //attackTarget = target;
        }


        public void AdvanceOnPath()
        {
            if (pathCreator == null)
                return;

            _currentPointOnPath = pathCreator.path.GetPointAtDistance(privateRabbitProgress, EndOfPathInstruction.Stop);

            _tempBasicMoveComponent.SetDestination(_currentPointOnPath, MoveType.Guided);

            Vector3 closestPointOnPath = pathCreator.path.GetClosestPointOnPath(transform.position);

            //if (Vector3.Distance(transform.position, pointOnPath) <= acceptableDistanceFromPath)
            if (Vector3.Distance(transform.position, closestPointOnPath) <= acceptableDistanceFromPath)
            {
                privateRabbitProgress += privateRabbitIncrement;
                if (privateRabbitProgress > pathCreator.path.length &&
                    Vector3.Distance(transform.position, _currentPointOnPath) <= acceptableDistanceToCompletion)
                {
                    finalDestinaion = LevelManager.CoreTemplete.PatrolPath;
                    CircleFinalDestination();
                }
            }
        }

        private void CircleFinalDestination()
        {
            _currentPointOnPath =
                finalDestinaion.path.GetPointAtDistance(privateRabbitProgress, EndOfPathInstruction.Loop);
            if (Vector3.Distance(transform.position, finalDestinaion.path.GetClosestPointOnPath(transform.position)) <=
                acceptableDistanceToCompletion)
            {
                privateRabbitProgress += finalLoopSpeed;
            }

            _tempBasicMoveComponent.SetDestination(_currentPointOnPath, MoveType.Free);

            //TEMP!!!!!!
            Enemy enemy = GetComponent<Enemy>();
            enemy.EntityTargetingComponent.SetAttackTarget(LevelManager.CoreTemplete);
            enemy.IsAttckingCore = true;
        }

        #region Callbacks

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (alwaysShowGizmo || UnityEditor.Selection.activeGameObject == gameObject)
                Gizmos.DrawCube(_currentPointOnPath, Vector3.one * rabbitGizmoBoxSize);
        }

#endif

        #endregion
    }
}