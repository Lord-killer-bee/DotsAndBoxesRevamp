using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectFactory;

namespace DnBGame {
    public class AIManager : MonoBehaviour {

        private Dictionary<GameEnums.EGameDifficultyMode, GameStructs.AIAlgorithmPackage> m_DifficultyToAlgorithmMap = new Dictionary<GameEnums.EGameDifficultyMode, GameStructs.AIAlgorithmPackage>();

		private GameManager m_gameManager;
       
        private Player m_AIPlayer;

        void Awake()
        {
            GameEventManager.PlayersCreated += CheckIfAIShouldBeSetUp;
            GameEventManager.ReferencesRegistered += GetReferences;
            GameEventManager.PlayerTurnsUpdated += CheckIfAICanPlaceLine;
        }

        private void Start()
        {
            GetAIPlayer();
            SetUpDifficultyToAlgorithmsMap();
        }

		private void GetReferences()
		{
			m_gameManager = ReferenceRegistry.instance.GetGameManager();
		}


		private void GetAIPlayer()
        {
            
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

        void CheckIfAIShouldBeSetUp()
        {

        }

        private void SetUpAI(GameEnums.EGameDifficultyMode difficultyMode)
        {

        }


        private void CheckIfAICanPlaceLine()
        {
            
        }

    }
}
