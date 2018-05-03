using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectFactory;

namespace DnBGame {
    public class AIManager : MonoBehaviour {

        private Dictionary<GameEnums.EGameDifficultyMode, GameStructs.AIAlgorithmPackage> m_DifficultyToAlgorithmMap = new Dictionary<GameEnums.EGameDifficultyMode, GameStructs.AIAlgorithmPackage>();

		private GameManager m_gameManager;

        void Awake()
        {
            GameEventManager.ReferencesRegistered += GetReferences;
            //GameEventManager.PlayerTurnsUpdated += CheckIfAICanPlaceLine;
        }

        private void Start()
        {
            SetUpDifficultyToAlgorithmsMap();
        }

		private void GetReferences()
		{
			m_gameManager = ReferenceRegistry.instance.GetGameManager();
		}

        private void SetUpDifficultyToAlgorithmsMap()
        {
            m_DifficultyToAlgorithmMap.Add(GameEnums.EGameDifficultyMode.EASY, new GameStructs.AIAlgorithmPackage
            {
                tier1Algorithm = GameEnums.EAIBehaviour.RANDOM_PLACEMENT,
                tier2Algorithm = GameEnums.EAIBehaviour.BASIC_CHAINING
            });

            m_DifficultyToAlgorithmMap.Add(GameEnums.EGameDifficultyMode.MEDIUM, new GameStructs.AIAlgorithmPackage
            {
                tier1Algorithm = GameEnums.EAIBehaviour.RANDOM_PLACEMENT,
                tier2Algorithm = GameEnums.EAIBehaviour.PRIORITY_CHAINING
            });

            m_DifficultyToAlgorithmMap.Add(GameEnums.EGameDifficultyMode.HARD, new GameStructs.AIAlgorithmPackage
            {
                tier1Algorithm = GameEnums.EAIBehaviour.RANDOM_PLACEMENT,
                tier2Algorithm = GameEnums.EAIBehaviour.SACRIFICIAL_CHAINING
            });
        }     

        public AIPlayer CreateAIPlayer(GameEnums.EGameDifficultyMode difficultyMode, string name, Color color)
        {
            IAIBehaviour tier1AI = AIFactory.CreateInstance(m_DifficultyToAlgorithmMap[difficultyMode].tier1Algorithm);
            IAIBehaviour tier2AI = AIFactory.CreateInstance(m_DifficultyToAlgorithmMap[difficultyMode].tier2Algorithm);

           return NonMonoObjectFactory<AIPlayer>.CreateInstance(() => new AIPlayer(name, color, GameEnums.EPlayerType.PLAYER_AI, tier1AI, tier2AI));
        }

    }
}
