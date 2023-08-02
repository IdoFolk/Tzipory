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

        protected override float Duration => _duration;

        public override void Init(EffectActionContainerConfig actionContainerConfig, IEntityVisualComponent visualComponent)
        {
            base.Init(actionContainerConfig, visualComponent);

            var config = GetConfig<PopUpEffectActionConfig>(actionContainerConfig.EffectActionConfig);

            _config = config;
            //_duration = config.PopUpText_Config.TTL; //Not sure this is needed
        }

        protected override void OnStartEffectAction()
        {
           
            Debug.LogError("Pop up called");
            VisualComponent.PopUpTexter.SpawnPopUp(_config.PopUpText_Config);
        }

        protected override void OnProcessEffectAction()
        {
            //here we can process things that are intervals and not just one shots
        }

        protected override void OnCompleteEffectAction()
        {
        }

        protected override void OnUndoEffectAction()
        {
            VisualComponent.SpriteRenderer.color = Color.white;
        }

        protected override void OnInterruptEffectAction()
        {
            OnUndoEffectAction();
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