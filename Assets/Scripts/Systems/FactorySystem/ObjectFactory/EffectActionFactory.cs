using Tzipory.Systems.FactorySystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.GameplayLogic.VisualSystem.EffectType.ColorEffect;
using Tzipory.GameplayLogic.VisualSystem.EffectType.PopUpEffect;
using Tzipory.GameplayLogic.VisualSystem.EffectType.SoundEffect;
using Tzipory.GameplayLogic.VisualSystem.EffectType.TransformEffect;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;

namespace Tzipory.Systems.FactorySystem.ObjectFactory
{
    public class EffectSequenceFactory :  IFactory<EffectSequence>
    {
        public EffectSequence Create() 
        {
            return new EffectSequence();
        }
    }
    
    public class ColorEffectActionFactory :  IFactory<ColorEffectAction>
    {
        public ColorEffectAction Create() 
        {
            return new ColorEffectAction();
        }
    }
    
    public class SoundEffectActionFactory :  IFactory<SoundEffectAction>
    {
        public SoundEffectAction Create()
        {
            return  new SoundEffectAction();
        }
    }
    
    public class TransformEffectActionFactory :  IFactory<TransformEffectAction>
    {
        public TransformEffectAction Create()
        {
            return new TransformEffectAction();
        }
    }
    public class PopUpEffectActionFactory :  IFactory<PopUpEffectAction>
    {
        public PopUpEffectAction Create()
        {
            return new PopUpEffectAction();
        }
    }

}