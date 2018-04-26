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
			Chain fillChain = new Chain();

			//if(levelCreator.GetListOfBoxes()[boxID].HasChainID())

			AddBoxToChainAndTraverseForMore(boxID, ref fillChain);
		}

		private void AddBoxToChainAndTraverseForMore(int boxID, ref Chain fillChain)
		{
			GameStructs.NodeData nodeData = levelCreator.GetLevelNodeData()[boxID];
			GameStructs.NeighbouringBoxes neighbourData = levelCreator.GetNeighbouringBoxesList()[boxID];

			if(!fillChain.chainData.Contains(boxID))
				fillChain.chainData.Add(boxID);

			if (levelCreator.GetHorizontalListOfLines()[nodeData.topLineID].IsLineOpen())
			{
				AddBoxToChainAndTraverseForMore(neighbourData.topBoxID, ref fillChain);
			}

			if (levelCreator.GetVerticalListOfLines()[nodeData.rightLineID].IsLineOpen())
			{
				AddBoxToChainAndTraverseForMore(neighbourData.rightBoxID, ref fillChain);
			}

			if (levelCreator.GetHorizontalListOfLines()[nodeData.bottomLineID].IsLineOpen())
			{
				AddBoxToChainAndTraverseForMore(neighbourData.bottomBoxID, ref fillChain);
			}

			if (levelCreator.GetVerticalListOfLines()[nodeData.leftLineID].IsLineOpen())
			{
				AddBoxToChainAndTraverseForMore(neighbourData.leftBoxID, ref fillChain);
			}
		}

		void AssignChainID(Chain chain, int chainID)
		{
			foreach (int boxID in chain.chainData)
			{
				levelCreator.GetListOfBoxes()[boxID].SetChainID(chainID);
			}
		}

    }

	[System.Serializable]
    public struct Chain
    {
        public List<int> chainData;
    }

}