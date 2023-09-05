using System;
using SerializeData.VisualSystemSerializeData;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Systems.PoolSystem;
using Tzipory.VisualSystem.EffectSequence.EffectActionTypeSO;
using UnityEngine;

namespace Tzipory.VisualSystem.EffectSequence.EffectType
{
    public class PopUpEffectAction : BaseEffectAction, IPoolable<PopUpEffectAction>
    {
        PopUpEffectActionConfig _config;
        private float _duration;

        public override float Duration => _duration;

        public override void Init(EffectActionContainerConfig actionContainerConfig, IEntityVisualComponent visualComponent)
        {
            base.Init(actionContainerConfig, visualComponent);

            var config = GetConfig<PopUpEffectActionConfig>(actionContainerConfig.EffectActionConfig);

            _config = config;
            //_duration = config.PopUpText_Config.TTL; //Not sure this is needed
        }

        public override void StartEffectAction()
        {
            VisualComponent.PopUpTexter.SpawnPopUp(_config.PopUpText_Config);
        }

        public override void ProcessEffectAction()
        {
            //here we can process things that are intervals and not just one shots
        }

        public override void CompleteEffectAction()
        {
        }

        public override void UndoEffectAction()
        {
            VisualComponent.SpriteRenderer.color = Color.white;
        }

        public override void InterruptEffectAction()
        {
            UndoEffectAction();
        }

        #region PoolObject

        public event Action<PopUpEffectAction> OnDispose;
        public void Dispose() =>
            OnDispose?.Invoke(this);

        public void Free()
        {

        }

        #endregion
    }
}