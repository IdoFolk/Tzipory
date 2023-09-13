using GameplayeLogic.Managers;

namespace Tzipory.GameplayLogic.AbilitySystem.AbilityEntity.Managers
{
    public class PlayerManager
    {
        private PlayerSerializeData  _playerSerializeData;

        public PlayerSerializeData PlayerSerializeData => _playerSerializeData;

        public PlayerManager(PlayerSerializeData playerSerializeData)
        {
            _playerSerializeData = playerSerializeData;
        }
    }
}