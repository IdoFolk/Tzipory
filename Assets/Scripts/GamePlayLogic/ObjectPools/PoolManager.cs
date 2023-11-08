using System;
using Tzipory.GameplayLogic.EntitySystem.Enemies;
using Tzipory.GameplayLogic.VisualSystem.EffectType;
using Tzipory.Systems.FactorySystem.GameObjectFactory;
using Tzipory.Systems.FactorySystem.ObjectFactory;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem.Indicators;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;

namespace Tzipory.GamePlayLogic.ObjectPools
{
    public class PoolManager
    {
        public static VisualSystemPool  VisualSystemPool { get; private set; }
        public static ObjectPool<Enemy> EnemyPool { get; private set; }
        public static ObjectPool<UIIndicator> IndicatorPool { get; private set; }
        
        public static ObjectPool<PopupText> PopUpTextPool { get; private set; }
        
        //modifyStatEffect pool

        public PoolManager()
        {
            VisualSystemPool  = new VisualSystemPool();
            EnemyPool = new ObjectPool<Enemy>(new EnemyFactory(),50);
            IndicatorPool  = new ObjectPool<UIIndicator>(new IndicatorFactory());
            PopUpTextPool = new ObjectPool<PopupText>(new PopUpTextFactory(),50);
        }
    }

    public class VisualSystemPool
    {
        private ObjectPool<EffectSequence> EffectActionPool { get; } = new(new EffectSequenceFactory(),10);
        private ObjectPool<ColorEffectAction> ColorEffectPool { get; } = new(new ColorEffectActionFactory(),15);
        private ObjectPool<TransformEffectAction> TransformEffectPool { get; } = new(new TransformEffectActionFactory(),15);
        private ObjectPool<SoundEffectAction> SoundEffectPool { get; } = new(new SoundEffectActionFactory(),15);

        // textpopup pool

        public BaseEffectAction GetEffectAction(EffectActionContainerConfig actionContainerConfig)
        {
            return actionContainerConfig.EffectActionConfig.ActionType switch
            {
                EffectActionType.Transform => TransformEffectPool.GetObject(),
                EffectActionType.Color => ColorEffectPool.GetObject(),
                EffectActionType.Outline => throw new NotImplementedException(),
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