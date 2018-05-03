using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame {

    public class AIFactory {

        public static IAIBehaviour CreateInstance(GameEnums.EAIBehaviour behaviour)
        {
            if (behaviour == GameEnums.EAIBehaviour.BASIC_CHAINING)
                return new BasicChainingAI();
            else if (behaviour == GameEnums.EAIBehaviour.PRIORITY_CHAINING)
                return new PriorityChainingAI();
            else if (behaviour == GameEnums.EAIBehaviour.RANDOM_PLACEMENT)
                return new RandomPlacementAI();
            else if (behaviour == GameEnums.EAIBehaviour.SACRIFICIAL_CHAINING)
                return new SacrificialChainingAI();
            else
                return null;
        }
    }
}