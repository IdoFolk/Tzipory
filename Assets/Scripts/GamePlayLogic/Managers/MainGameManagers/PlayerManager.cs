using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.SerializeData;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
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