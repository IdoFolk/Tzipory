using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.FactorySystem;

public class HeroFactory : BaseGameObjectFactory<Shaman>
{
    protected override string GameObjectPath => "Prefabs/Entities/Units/NewShaman";
}