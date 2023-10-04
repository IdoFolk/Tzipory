using System;
using Tzipory.GameplayLogic.EntitySystem.Enemies;
using Tzipory.GameplayLogic.UI.WaveIndicator;
using Tzipory.GameplayLogic.VisualSystem.EffectType;
using Tzipory.Systems.FactorySystem.ObjectFactory;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.FactorySystem.GameObjectFactory;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;

namespace Tzipory.GamePlayLogic.ObjectPools
{
    public class PoolManager
    {
        public static VisualSystemPool  VisualSystemPool { get; private set; }
        public static ObjectPool<Enemy> EnemyPool { get; private set; }
        public static ObjectPool<WaveIndicator> IndicatorPool { get; private set; }
        
        //modifyStatEffect pool

        public PoolManager()
        {
            VisualSystemPool  = new VisualSystemPool();
            EnemyPool = new ObjectPool<Enemy>(new EnemyFactory(),50);
            IndicatorPool  = new ObjectPool<WaveIndicator>(new WaveIndicatorFactory());
        }
    }

    public class VisualSystemPool
    {
        private ObjectPool<EffectSequence> EffectActionPool { get; set; }
        private ObjectPool<ColorEffectAction> ColorEffectPool { get; set; }
        private ObjectPool<SoundEffectAction> SoundEffectPool { get; set; }
        private ObjectPool<TransformEffectAction> TransformEffectPool { get; set; }
        private ObjectPool<PopUpEffectAction> PopUpEffectPool { get; set; }

        // textpopup pool

        public VisualSystemPool()
        {
            EffectActionPool = new ObjectPool<EffectSequence>(new EffectSequenceFactory(),10);
            ColorEffectPool = new ObjectPool<ColorEffectAction>(new ColorEffectActionFactory(),15);
            SoundEffectPool = new ObjectPool<SoundEffectAction>(new SoundEffectActionFactory(),15);
            TransformEffectPool = new ObjectPool<TransformEffectAction>(new TransformEffectActionFactory(),15);
            PopUpEffectPool = new ObjectPool<PopUpEffectAction>(new PopUpEffectActionFactory(),30);
        }
        
        public BaseEffectAction GetEffectAction(EffectActionContainerConfig actionContainerConfig)
        {
            return actionContainerConfig.EffectActionConfig.ActionType switch
            {
                EffectActionType.Transform => TransformEffectPool.GetObject(),
                EffectActionType.Color => ColorEffectPool.GetObject(),
                EffectActionType.Outline => throw new NotImplementedException(),
                EffectActionType.PopUp => PopUpEffectPool.GetObject(),
                EffectActionType.ParticleEffects => throw new NotImplementedException(),
                EffectActionType.Sound => SoundEffectPool.GetObject(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public EffectSequence GetEffectSequence(EffectSequenceConfig sequenceConfig)
        {
            return  EffectActionPool.GetObject();
        }

        //getpopup (popupconfig)
    }
}