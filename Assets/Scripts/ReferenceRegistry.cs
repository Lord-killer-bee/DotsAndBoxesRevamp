using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame {

    public class ReferenceRegistry : MonoBehaviour {

        public static ReferenceRegistry instance;

        [SerializeField]
        private LevelCreator levelCreator;
        [SerializeField]
        private ChainManager chainManager;
        [SerializeField]
        private GameManager gameManager;
        [SerializeField]
        private AIManager aiManager;

        private void Start()
        {
            if(instance == null)
            {
                instance = this;
            }

			GameEventManager.TriggerReferencesRegistered();
        }

        public LevelCreator GetLevelCreator()
        {
            return levelCreator;
        }

        public ChainManager GetChainManager()
        {
            return chainManager;
        }

        public GameManager GetGameManager()
        {
            return gameManager;
        }

        public AIManager GetAIManager()
        {
            return aiManager;
        }
    }
}