using Tzipory.GameplayLogic.AbilitySystem.AbilityEntity.UI.WaveIndicator;

namespace Systems.FactorySystem
{
    public class WaveIndicatorFactory : BaseGameObjectFactory<WaveIndicator>
    {
        protected override string GameObjectPath => "Prefabs/Visual/GameEvents/WaveIndicatorUI";
    }
}