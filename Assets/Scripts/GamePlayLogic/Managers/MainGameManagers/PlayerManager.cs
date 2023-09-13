﻿using Tzipory.GameplayLogic.Managers.CoreGameManagers;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
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