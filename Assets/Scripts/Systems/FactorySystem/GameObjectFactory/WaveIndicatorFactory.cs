using Tzipory.GameplayLogic.UI.WaveIndicator;

namespace Tzipory.Systems.FactorySystem.GameObjectFactory
{
    public class WaveIndicatorFactory : BaseGameObjectFactory<WaveIndicator>
    {
        protected override string GameObjectPath => "Prefabs/Visual/GameEvents/WaveIndicatorUI";
    }
}