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

        private void GetReferences()
        {
            m_gameManager = ReferenceRegistry.instance.GetGameManager();
        }

        void CheckIfAIShouldBeSetUp()
        {
            if(m_gameManager.player1Type == GameEnums.EPlayerType.PLAYER_AI || m_gameManager.player2Type == GameEnums.EPlayerType.PLAYER_AI)
            {
                SetUpAI(m_gameManager.difficultyMode);
            }
        }

        private void SetUpAI(GameEnums.EGameDifficultyMode difficultyMode)
        {
            AIFactory.CreateInstance(m_DifficultyToAlgorithmMap[difficultyMode].tier1Algorithm);
            AIFactory.CreateInstance(m_DifficultyToAlgorithmMap[difficultyMode].tier2Algorithm);
        }


        private void CheckIfAICanPlaceLine()
        {
            
        }

    }
}
