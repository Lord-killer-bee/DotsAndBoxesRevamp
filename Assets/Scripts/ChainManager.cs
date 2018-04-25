using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
    public class ChainManager : MonoBehaviour
    {
        public Dictionary<int, Chain> chainsList = new Dictionary<int, Chain>();

        private LevelCreator levelCreator;

        private void Awake()
        {
            GameEventManager.BoxScoreValidForChain += CalculateChainAroundPivotBox;
        }

        private void Start()
        {
            levelCreator = ReferenceRegistry.instance.GetLevelCreator();
        }

        private void CalculateChainAroundPivotBox(int boxID)
        {
            GameStructs.NodeData data = levelCreator.GetLevelNodeData()[boxID];

            if (levelCreator.GetHorizontalListOfLines()[levelCreator.GetAffectedBoxesHorizontal()[data.topLineID][0]].IsLineOpen())
            {

            }
        }

    }

    public struct Chain
    {
        public List<Box> chain;
    }

}