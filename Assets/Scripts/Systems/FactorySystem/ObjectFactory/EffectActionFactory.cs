using Tzipory.GameplayLogic.VisualSystem.EffectType;
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
}