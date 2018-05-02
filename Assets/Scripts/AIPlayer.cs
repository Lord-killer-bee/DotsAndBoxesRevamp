using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
    public class AIPlayer : Player
    {

        private IAIBehaviour m_Tier1AI, m_Tier2AI;

        public AIPlayer(string name, Color color, GameEnums.EPlayerType playerType, IAIBehaviour tier1AI, IAIBehaviour tier2AI) : 
            base(name, color, playerType)
        {
            m_Tier1AI = tier1AI;
            m_Tier2AI = tier2AI;
        }
    }
}