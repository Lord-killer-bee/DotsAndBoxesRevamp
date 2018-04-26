using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectFactory;
using Logger;

namespace DnBGame
{

    public class LevelCreator : MonoBehaviour
    {

        #region Public parameters

        public GameObject boardPanel;
        public GameObject linePrefab, boxPrefab;
        public int NoOfRowsOrColumns;
        public float BoardPercentage;
        public float dotDimension;

        #endregion

        #region Private parameters

        private float m_dotWidth, m_dotHeight;
        private float m_lineWidth, m_lineHeight;
        private float m_boxWidth, m_boxHeight;

        #endregion

        private List<Box> m_ListOfBoxes = new List<Box>();
        private Dictionary<GameStructs.LineID, Line> m_ListOfLinesHorizontal = new Dictionary<GameStructs.LineID, Line>();
        private Dictionary<GameStructs.LineID, Line> m_ListOfLinesVertical = new Dictionary<GameStructs.LineID, Line>();

        private Dictionary<int, GameStructs.NodeData> m_LevelNodeData = new Dictionary<int, GameStructs.NodeData>();
        private Dictionary<GameStructs.LineID, int[]> m_AffectedBoxesHorizontal = new Dictionary<GameStructs.LineID, int[]>();
        private Dictionary<GameStructs.LineID, int[]> m_AffectedBoxesVertical = new Dictionary<GameStructs.LineID, int[]>();
		private Dictionary<int, GameStructs.NeighbouringBoxes> m_NeighbouringBoxesList = new Dictionary<int, GameStructs.NeighbouringBoxes>();

        private void Awake()
        {
            GameEventManager.LinePlaced += LinePlacedDoStuff;
        }

        void Start()
        {
            InitializeDimensions();
            ArrangeLines();
            ArrangeBoxes();
            GenerateNodeData();
            GenerateAffectedBoxesData();
			GenerateNeighbouringBoxesList();

			GameEventManager.TriggerLevelCreated();
			GameLogger.LogMessage("Level Created!!");
        }

		#region Level setup

		private void InitializeDimensions()
        {
            m_dotWidth = m_dotHeight = dotDimension;
            m_lineWidth = GetLineWidth();
            m_lineHeight = GetLineHeight();
            m_boxWidth = GetBoxWidth();
            m_boxHeight = GetBoxHeight();
        }

        private void ArrangeBoxes()
        {
            int j = 0;
            int k = 0;
            for (int i = 0; i < (NoOfRowsOrColumns - 1) * (NoOfRowsOrColumns - 1); i++)
            {
                Box box = MonoObjectFactory<Box>.CreateInstance(new ObjectConstructMaterials()
                {
                    prefab = boxPrefab,
                    parameters = new object[]{ new Vector3((m_lineWidth + m_dotWidth) * (j + 0.5f), (-m_lineHeight - m_dotHeight) * (k + 0.5f), 0),
                                           new Vector2(m_boxWidth, m_boxHeight),
                                           new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                           new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                           i,
                                           boardPanel }
                });

                j++;

                m_ListOfBoxes.Add(box);

                if (j > NoOfRowsOrColumns - 2)
                {
                    j = 0;
                    k++;
                }
            }

        }

        private void ArrangeLines()
        {
            int j = 0, k = 0;

            for (int i = 0; i < GetNoOfLines(NoOfRowsOrColumns) / 2; i++)
            {
                if (j < NoOfRowsOrColumns - 1)
                {
					GameStructs.LineID lineID = new GameStructs.LineID { ID = i, rotation = GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE };

					Line lineHorizontal = MonoObjectFactory<Line>.CreateInstance(new ObjectConstructMaterials()
					{ 					
						prefab = linePrefab,
                        parameters = new object[] {  new Vector3((j + 0.5f) * (m_lineWidth + m_dotWidth), -k * (m_lineHeight + m_dotHeight), 1),
                                                 new Vector2(m_lineWidth, m_dotHeight),
                                                 new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                 new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                 lineID,
                                                 boardPanel}
                    });

					m_ListOfLinesHorizontal.Add(lineID, lineHorizontal);

					lineID.rotation = GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE;

					Line lineVertical = MonoObjectFactory<Line>.CreateInstance( new ObjectConstructMaterials()
                    {
                        prefab = linePrefab,
                        parameters = new object[] { new Vector3(k * (m_lineWidth + m_dotWidth), -(j + 0.5f) * (m_lineHeight + m_dotHeight), 1),
                                                new Vector2(m_lineHeight, m_dotWidth),
                                                new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                lineID,
                                                boardPanel}
                    });
              
                    m_ListOfLinesVertical.Add(lineID, lineVertical);


                    j++;
                    if (j == NoOfRowsOrColumns - 1)
                    {
                        j = 0;
                        k++;
                    }
                }
            }
        }

        #endregion

        #region Public Getters
        public List<Box> GetListOfBoxes()
        {
            return m_ListOfBoxes;
        }

        public Dictionary<GameStructs.LineID, Line> GetHorizontalListOfLines()
        {
            return m_ListOfLinesHorizontal;
        }

        public Dictionary<GameStructs.LineID, Line> GetVerticalListOfLines()
        {
            return m_ListOfLinesVertical;
        }

        public Dictionary<int, GameStructs.NodeData> GetLevelNodeData()
        {
            return m_LevelNodeData;
        }

        public Dictionary<GameStructs.LineID, int[]> GetAffectedBoxesHorizontal()
        {
            return m_AffectedBoxesHorizontal;
        }

        public Dictionary<GameStructs.LineID, int[]> GetAffectedBoxesVertical()
        {
            return m_AffectedBoxesVertical;
        }

		public Dictionary<int, GameStructs.NeighbouringBoxes> GetNeighbouringBoxesList()
		{
			return m_NeighbouringBoxesList;
		}

		#endregion

		#region Private Getters
		private int GetNoOfLines(int NoOfRowsOrColumns)
        {
            return 2 * NoOfRowsOrColumns * (NoOfRowsOrColumns - 1);
        }

        private float GetBoxHeight()
        {
            return GetLineHeight() + 2 * m_dotHeight;
        }

        private float GetBoxWidth()
        {
            return GetLineWidth() + 2 * m_dotWidth;
        }

        private float GetLineHeight()
        {
            return (((boardPanel.GetComponent<RectTransform>().rect.height) * BoardPercentage) - (NoOfRowsOrColumns * m_dotHeight)) / (NoOfRowsOrColumns - 1);
        }

        private float GetLineWidth()
        {
            return (((boardPanel.GetComponent<RectTransform>().rect.width) * BoardPercentage) - (NoOfRowsOrColumns * m_dotWidth)) / (NoOfRowsOrColumns - 1);
        }

        #endregion


        #region Data Generation

        private void GenerateNodeData()
        {
            int nodeIndex, row = 0, column = 0;

            for (int i = 0; i < m_ListOfBoxes.Count; i++)
            {
                nodeIndex = m_ListOfBoxes[i].GetBoxID();

                m_LevelNodeData.Add(nodeIndex, new GameStructs.NodeData()
                {
                    topLineID = new GameStructs.LineID { ID = nodeIndex, rotation = GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE },
                    rightLineID = new GameStructs.LineID { ID = (row + (NoOfRowsOrColumns - 1) * column) + (NoOfRowsOrColumns - 1), rotation = GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE },
                    bottomLineID = new GameStructs.LineID { ID = nodeIndex + (NoOfRowsOrColumns - 1), rotation = GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE },
                    leftLineID = new GameStructs.LineID { ID = row + (NoOfRowsOrColumns - 1) * column, rotation = GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE }
                });

                GameLogger.LogMessage(i.ToString() + " : " + "[" + m_LevelNodeData[i].topLineID.ToString() + "," + m_LevelNodeData[i].rightLineID.ToString() + "," + m_LevelNodeData[i].bottomLineID.ToString() + "," + m_LevelNodeData[i].leftLineID.ToString() + "]");

                column++;

                if(column >= NoOfRowsOrColumns - 1)
                {
                    column = 0;
                    row++;
                }
            }
        }

        private void GenerateAffectedBoxesData()
        {
            int row = 0, column = 0;

            for (int i = 0; i < m_ListOfLinesHorizontal.Count; i++)
            {
                GameStructs.LineID horizontalID = new GameStructs.LineID { ID = i, rotation = GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE };
                GameStructs.LineID verticalID = new GameStructs.LineID { ID = i, rotation = GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE };

                if (row == 0)
                {
                    m_AffectedBoxesHorizontal.Add(horizontalID, new int[]{ i });
                    m_AffectedBoxesVertical.Add(verticalID, new int[] { row + (NoOfRowsOrColumns - 1) * column });
                }
                else if(row > 0 && row < NoOfRowsOrColumns - 1)
                {
                    m_AffectedBoxesHorizontal.Add(horizontalID, new int[] { i, i - (NoOfRowsOrColumns - 1) });
                    m_AffectedBoxesVertical.Add(verticalID, new int[] { row + ((NoOfRowsOrColumns - 1) * column) - 1, row + ((NoOfRowsOrColumns - 1) * column) });
                }
                else if(row == NoOfRowsOrColumns - 1)
                {
                    m_AffectedBoxesHorizontal.Add(horizontalID, new int[] { i - (NoOfRowsOrColumns - 1) });
                    m_AffectedBoxesVertical.Add(verticalID, new int[] { row + ((NoOfRowsOrColumns - 1) * column) - 1 });
                }

                column++;

                if (column >= NoOfRowsOrColumns - 1)
                {
                    column = 0;
                    row++;
                }
            }          
        }

		private void GenerateNeighbouringBoxesList()
		{
			int nodeIndex, row = 0, column = 0;

			for (int i = 0; i < m_ListOfBoxes.Count; i++)
			{
				nodeIndex = m_ListOfBoxes[i].GetBoxID();

				if (row == 0 && column == 0)
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = GameConstants.INVALID_ID,
						rightBoxID = nodeIndex + 1,
						bottomBoxID = nodeIndex + (NoOfRowsOrColumns - 1),
						leftBoxID = GameConstants.INVALID_ID				
					});
				}
				else if (row == 0 && column == NoOfRowsOrColumns - 2)
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = GameConstants.INVALID_ID,
						rightBoxID = GameConstants.INVALID_ID,
						bottomBoxID = nodeIndex + (NoOfRowsOrColumns - 1),
						leftBoxID = nodeIndex - 1
					});
				}
				else if (row == NoOfRowsOrColumns - 2 && column == NoOfRowsOrColumns - 2)
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = nodeIndex - (NoOfRowsOrColumns - 1),
						rightBoxID = GameConstants.INVALID_ID,
						bottomBoxID = GameConstants.INVALID_ID,
						leftBoxID = nodeIndex - 1
					});
				}
				else if (row == NoOfRowsOrColumns - 2 && column == 0)
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = nodeIndex - (NoOfRowsOrColumns - 1),
						rightBoxID = nodeIndex + 1,
						bottomBoxID = GameConstants.INVALID_ID,
						leftBoxID = GameConstants.INVALID_ID
					});
				}
				else if(row == 0 && (column > 0 && column < NoOfRowsOrColumns - 2))
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = GameConstants.INVALID_ID,
						rightBoxID = nodeIndex + 1,
						bottomBoxID = nodeIndex + (NoOfRowsOrColumns - 1),
						leftBoxID = nodeIndex - 1
					});
				}
				else if (row == NoOfRowsOrColumns - 2 && (column > 0 && column < NoOfRowsOrColumns - 2))
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = nodeIndex - (NoOfRowsOrColumns - 1),
						rightBoxID = nodeIndex + 1,
						bottomBoxID = GameConstants.INVALID_ID,
						leftBoxID = nodeIndex - 1
					});
				}
				else if (column == 0 && (row > 0 && row < NoOfRowsOrColumns - 2))
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = nodeIndex - (NoOfRowsOrColumns - 1),
						rightBoxID = nodeIndex + 1,
						bottomBoxID = nodeIndex + (NoOfRowsOrColumns - 1),
						leftBoxID = GameConstants.INVALID_ID
					});
				}
				else if (column == NoOfRowsOrColumns - 2 && (row > 0 && row < NoOfRowsOrColumns - 2))
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = nodeIndex - (NoOfRowsOrColumns - 1),
						rightBoxID = GameConstants.INVALID_ID,
						bottomBoxID = nodeIndex + (NoOfRowsOrColumns - 1),
						leftBoxID = nodeIndex - 1
					});
				}
				else
				{
					m_NeighbouringBoxesList.Add(nodeIndex, new GameStructs.NeighbouringBoxes
					{
						topBoxID = nodeIndex - (NoOfRowsOrColumns - 1),
						rightBoxID = nodeIndex + 1,
						bottomBoxID = nodeIndex + (NoOfRowsOrColumns - 1),
						leftBoxID = nodeIndex - 1
					});
				}

				column++;

				if (column >= NoOfRowsOrColumns - 2)
				{
					column = 0;
					row++;
				}
			}
		}


		#endregion

		private void LinePlacedDoStuff(GameStructs.LineID lineID)
        {
            LinePlacedDeactivateOldLine();
            IncrementAffectedBoxScores(lineID);
        }

        private void IncrementAffectedBoxScores(GameStructs.LineID lineID)
        {
            int boxActivatedCounter = 0;

            switch (lineID.rotation)
            {
                case GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE:

                    for (int i = 0; i < m_AffectedBoxesHorizontal[lineID].Length; i++)
                    {
                        Box temp = m_ListOfBoxes[m_AffectedBoxesHorizontal[lineID][i]];
                        temp.AddScore();

                        if (temp.GetScore() == 4)
                            boxActivatedCounter++;
                    }

                    break;
                case GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE:

                    for (int i = 0; i < m_AffectedBoxesVertical[lineID].Length; i++)
                    {
                        Box temp = m_ListOfBoxes[m_AffectedBoxesVertical[lineID][i]];
                        temp.AddScore();

                        if (temp.GetScore() == 4)
                            boxActivatedCounter++;
                    }

                    break;
            }

            if(boxActivatedCounter >= 1)
                GameEventManager.TriggerBoxScoredToFour(true);
            else
                GameEventManager.TriggerBoxScoredToFour(false);
        }

        private void LinePlacedDeactivateOldLine()
        {
            foreach (KeyValuePair<GameStructs.LineID, Line> pair in m_ListOfLinesHorizontal)
            {
                if (pair.Value.IsLineActive())
					pair.Value.SetLineInactive();                  
            }

            foreach (KeyValuePair<GameStructs.LineID, Line> pair in m_ListOfLinesVertical)
            {
                if (pair.Value.IsLineActive())
					pair.Value.SetLineInactive();
            }
        }
    }

}