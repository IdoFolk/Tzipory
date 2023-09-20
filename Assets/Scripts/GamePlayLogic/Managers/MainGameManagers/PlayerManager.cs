using GameplayeLogic.Managers;

namespace GamePlayLogic.Managers
{
    public class PlayerManager
    {
        private PlayerSerializeData  _playerSerializeData;

        public PlayerSerializeData PlayerSerializeData => _playerSerializeData;
        
        //TODO : add Inventory manager 

        public PlayerManager(PlayerSerializeData playerSerializeData)
        {
            _playerSerializeData = playerSerializeData;
        }
    }
}