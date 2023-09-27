﻿using Tzipory.SerializeData.PlayerData.PartySerializeData.EntitySerializeData;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
    public class GameData
    {
        public LevelConfig LevelConfig;
        
        //Temp
        public bool[] NodeLockStatState { get; }
        public bool[] NodeCompletedState { get; }

        public GameData()
        {
            //temp
            NodeLockStatState = new bool[4];
            NodeCompletedState = new bool[4];
            
            for (int i = 0; i < 4; i++)
            {
                NodeCompletedState[i] = false;
                
                if (i == 0)
                {
                    NodeLockStatState[i] = true;
                    continue;
                }
                
                NodeLockStatState[i] = false;
            }
        }

        public void SetLockNodeStat(int nodeId, bool status)
        {
            NodeLockStatState[nodeId] = status;
        }
        
        public void SetCompletedNodeStat(int nodeId, bool status)
        {
            NodeCompletedState[nodeId] = status;
            NodeLockStatState[nodeId] = status;
        }
    }
}