using Tzipory.Systems.FactorySystem;
using Tzipory.GameplayLogic.StatusEffectTypes;
using Tzipory.GameplayLogic.StatusEffectTypes.EffectType;

namespace Tzipory.Factory
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