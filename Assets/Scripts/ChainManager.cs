using Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
    public class ChainManager : MonoBehaviour
    {
		public Dictionary<int, Chain> chainsList = new Dictionary<int, Chain>();

		public List<Chain> inspector_Chain_List = new List<Chain>();

		private LevelCreator levelCreator;

		private int m_LastCreatedChainID = 0;

		private void Awake()
		{
			GameEventManager.BoxScoreValidForChain += CheckTheAffectedBoxesForChaining;
			GameEventManager.ReferencesRegistered += GetReferences;
		}

		private void GetReferences()
		{
			levelCreator = ReferenceRegistry.instance.GetLevelCreator();
		}

		private void CheckTheAffectedBoxesForChaining(int boxID)
		{
			if(levelCreator.GetListOfBoxes()[boxID].GetScore() == 4)
			{
				RemoveBoxFromChain(boxID);
			}
			else if(levelCreator.GetListOfBoxes()[boxID].GetScore() >= 2)
			{
				CalculateChainAroundPivotBox(boxID);
			}
		}

		private void CalculateChainAroundPivotBox(int boxID)
		{
			Chain fillChain = new Chain();

			List<int> hitChainIDs = new List<int>();

			if (levelCreator.GetListOfBoxes()[boxID].HasChainID())
			{
				int chainID = levelCreator.GetListOfBoxes()[boxID].GetChainID();
				AddBoxToChainAndTraverseForMore(boxID, ref fillChain, ref hitChainIDs);

				if(hitChainIDs.Count > 1)
				{
					foreach (int ID in hitChainIDs)
					{
						ClearChainIds(chainsList[ID]);
						chainsList.Remove(ID);
					}
					AssignChainID(fillChain, chainID);
					chainsList.Add(chainID, fillChain);
				}

			}
			else
			{
				GenerateChainID();
				AddBoxToChainAndTraverseForMore(boxID, ref fillChain, ref hitChainIDs);

				if(hitChainIDs.Count <= 0)
				{
					AssignChainID(fillChain, m_LastCreatedChainID);
					chainsList.Add(m_LastCreatedChainID, fillChain);
				}
				else
				{
					foreach (int chainID in hitChainIDs)
					{
						ClearChainIds(chainsList[chainID]);
						chainsList.Remove(chainID);
					}

					AssignChainID(fillChain, m_LastCreatedChainID);
					chainsList.Add(m_LastCreatedChainID, fillChain);
				}

			}

			Test_List_For_Dictionary();

		}

		private void RemoveBoxFromChain(int boxID)
		{
			int chainID = levelCreator.GetListOfBoxes()[boxID].GetChainID();

			chainsList[chainID].chainData.Remove(boxID);

			if (chainsList[chainID].chainData.Count == 0)
				chainsList.Remove(chainID);
		}

		private void Test_List_For_Dictionary()
		{
			inspector_Chain_List.Clear();
			foreach (var item in chainsList)
			{
				inspector_Chain_List.Add(item.Value);
			}
		}

		private void AddBoxToChainAndTraverseForMore(int boxID, ref Chain fillChain, ref List<int> chainIDTracker)
		{
			if (boxID != GameConstants.INVALID_ID)
			{
				Box box = levelCreator.GetListOfBoxes()[boxID];

				if (box.GetScore() < 2 || box.GetScore() == 4 || fillChain.chainData.Contains(boxID))
					return;

				GameStructs.NodeData nodeData = levelCreator.GetLevelNodeData()[boxID];
				GameStructs.NeighbouringBoxes neighbourData = levelCreator.GetNeighbouringBoxesList()[boxID];

				if (!fillChain.chainData.Contains(boxID))
				{
					fillChain.chainData.Add(boxID);

					if (box.HasChainID() && !chainIDTracker.Contains(box.GetChainID()))
					{
						chainIDTracker.Add(box.GetChainID());
					}
				}

				if (levelCreator.GetHorizontalListOfLines()[nodeData.topLineID].IsLineOpen())
				{
					AddBoxToChainAndTraverseForMore(neighbourData.topBoxID, ref fillChain, ref chainIDTracker);
				}

				if (levelCreator.GetVerticalListOfLines()[nodeData.rightLineID].IsLineOpen())
				{
					AddBoxToChainAndTraverseForMore(neighbourData.rightBoxID, ref fillChain, ref chainIDTracker);
				}

				if (levelCreator.GetHorizontalListOfLines()[nodeData.bottomLineID].IsLineOpen())
				{
					AddBoxToChainAndTraverseForMore(neighbourData.bottomBoxID, ref fillChain, ref chainIDTracker);
				}

				if (levelCreator.GetVerticalListOfLines()[nodeData.leftLineID].IsLineOpen())
				{
					AddBoxToChainAndTraverseForMore(neighbourData.leftBoxID, ref fillChain, ref chainIDTracker);
				}
			}
		}

		void AssignChainID(Chain chain, int chainID)
		{
			chain.chainID = chainID;// assign chainid to chain

			foreach (int boxID in chain.chainData)// assign chainid to all the boxes
			{
				levelCreator.GetListOfBoxes()[boxID].SetChainID(chainID);
			}
		}

		void ClearChainIds(Chain chain)
		{
			chain.chainID = GameConstants.INVALID_ID;

			foreach (int boxID in chain.chainData)// assign chainid to all the boxes
			{
				levelCreator.GetListOfBoxes()[boxID].SetChainID(GameConstants.INVALID_ID);
			}
		}

		int GenerateChainID()
		{
			return m_LastCreatedChainID++;
		}

	}

	[System.Serializable]
	public class Chain
	{
		public int chainID;
		public List<int> chainData = new List<int>();
	}

}