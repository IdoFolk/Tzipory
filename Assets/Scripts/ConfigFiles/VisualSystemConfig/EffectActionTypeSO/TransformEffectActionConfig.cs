using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.VisualSystemConfig;
using Tzipory.ConfigFiles.VisualSystemConfig.EffectType;
using UnityEngine;

namespace Tzipory.ConfigFiles.VisualSystemConfig.EffectActionTypeSO
{
    [CreateAssetMenu(fileName = "NewTransformEffectAction", menuName = "ScriptableObjects/VisualSystem/EffectAction/New transform effect action")]
    public class TransformEffectActionConfig : BaseEffectActionConfig
    {
        #region Position
        
        [SerializeField,TabGroup("Position")] private bool _haveMovement = true; 
        [SerializeField,TabGroup("Position"),ShowIf("_haveMovement")] private PositionMoveType _positionMoveTypeEnum;
        [SerializeField,TabGroup("Position"),ShowIf("_haveMovement")] private Vector3 _moveOffSet;
        [SerializeField,TabGroup("Position"),ShowIf("_haveMovement")] private Transition3D _movement;

        public bool HaveMovement => _haveMovement;

        public Transition3D Movement => _movement;

        public PositionMoveType PositionMoveTypeEnum => _positionMoveTypeEnum;

        public Vector3 MoveOffSet => _moveOffSet;

        #endregion
        
        #region Scale
        
        [SerializeField,TabGroup("Scale")] private bool _haveScale = false; 
        [SerializeField,TabGroup("Scale"),ShowIf("_haveScale")] private ScaleTypeEnum _scaleType;
        [SerializeField,TabGroup("Scale"),ShowIf("_haveScale")] private float _scaleMultiplier;
        [SerializeField,TabGroup("Scale"),ShowIf("_haveScale")] private Vector3 _scaleVector;
        [SerializeField,TabGroup("Scale"),ShowIf("_haveScale")] private Transition3D _scale;

        public ScaleTypeEnum ScaleType => _scaleType;

        public bool HaveScale => _haveScale;

        public float ScaleMultiplier => _scaleMultiplier;

        public Vector3 ScaleVector => _scaleVector;

        public Transition3D Scale => _scale;

        #endregion

        #region Rotation

        [SerializeField,TabGroup("Rotation")] private bool _haveRotation = false; 
        [SerializeField,TabGroup("Rotation"),ShowIf("_haveRotation")] private Vector3 _rotate; 
        [SerializeField,TabGroup("Rotation"),ShowIf("_haveRotation")] private Transition3D _rotation;

        public bool HaveRotation => _haveRotation;

        public Vector3 Rotate => _rotate;

        public Transition3D Rotation => _rotation;

        #endregion
        
        public override EffectActionType ActionType => EffectActionType.Transform;
        
        public enum PositionMoveType
        {
            Local,
            Word
        }
        
        public enum ScaleTypeEnum
        {
            ByFloat,
            ByVector
        };
    }
}