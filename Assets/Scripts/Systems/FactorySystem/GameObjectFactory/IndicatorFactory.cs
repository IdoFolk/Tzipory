using Tzipory.Systems.UISystem.Indicators;

namespace Tzipory.Systems.FactorySystem.GameObjectFactory
{
    public class IndicatorFactory : BaseGameObjectFactory<UIIndicator>
    {
        protected override string GameObjectPath => "Prefabs/Visual/GameEvents/UIIndicator";
    }
}