﻿using UnityEngine;

namespace Tzipory.VisualSystem.EffectSequence.EffectActionTypeSO
{
    [CreateAssetMenu(fileName = "NewColorEffectAction", menuName = "ScriptableObjects/VisualSystem/EffectAction/New color effect action", order = 0)]
    public class ColorEffectActionSO : BaseEffectActionSO
    {
        [SerializeField,Tooltip("")] private Color _color;
        [SerializeField,Tooltip("")] private float _alpha;
        [SerializeField,Tooltip("")] private float _duration;

        public Color Color => _color;

        public float Alpha => _alpha;

        public float Duration => _duration;
        public override EffectActionType ActionType => EffectActionType.Color;
    }
}