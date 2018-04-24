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
        private List<Line> m_ListOfLinesHorizontal = new List<Line>();
        private List<Line> m_ListOfLinesVertical = new List<Line>();

        public Dictionary<int, GameStructs.NodeData> levelNodeData = new Dictionary<int, GameStructs.NodeData>();
        public Dictionary<GameStructs.LineID, int[]> affectedBoxesHorizontal = new Dictionary<GameStructs.LineID, int[]>();
        public Dictionary<GameStructs.LineID, int[]> affectedBoxesVertical = new Dictionary<GameStructs.LineID, int[]>();

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
                    Line lineHorizontal = MonoObjectFactory<Line>.CreateInstance( new ObjectConstructMaterials()
                    {
                        prefab = linePrefab,
                        parameters = new object[] {  new Vector3((j + 0.5f) * (m_lineWidth + m_dotWidth), -k * (m_lineHeight + m_dotHeight), 1),
                                                 new Vector2(m_lineWidth, m_dotHeight),
                                                 new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                 new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                 new GameStructs.LineID {ID = i, rotation =  GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE},
                                                 boardPanel}
                    });

                    Line lineVertical = MonoObjectFactory<Line>.CreateInstance( new ObjectConstructMaterials()
                    {
                        prefab = linePrefab,
                        parameters = new object[] { new Vector3(k * (m_lineWidth + m_dotWidth), -(j + 0.5f) * (m_lineHeight + m_dotHeight), 1),
                                                new Vector2(m_lineHeight, m_dotWidth),
                                                new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                new GameStructs.LineID {ID = i, rotation =  GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE},
                                                boardPanel}
                    });

                    m_ListOfLinesHorizontal.Add(lineHorizontal);
                    m_ListOfLinesVertical.Add(lineVertical);


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

        public List<Line> GetHorizontalListOfLines()
        {
            return m_ListOfLinesHorizontal;
        }

        public List<Line> GetVerticalListOfLines()
        {
            return m_ListOfLinesVertical;
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

                levelNodeData.Add(nodeIndex, new GameStructs.NodeData()
                {
                    topLineID = new GameStructs.LineID { ID = nodeIndex, rotation = GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE },
                    rightLineID = new GameStructs.LineID { ID = (row + (NoOfRowsOrColumns - 1) * column) + (NoOfRowsOrColumns - 1), rotation = GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE },
                    bottomLineID = new GameStructs.LineID { ID = nodeIndex + (NoOfRowsOrColumns - 1), rotation = GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE },
                    leftLineID = new GameStructs.LineID { ID = row + (NoOfRowsOrColumns - 1) * column, rotation = GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE }
                });

                GameLogger.LogMessage(i.ToString() + " : " + "[" + levelNodeData[i].topLineID.ToString() + "," + levelNodeData[i].rightLineID.ToString() + "," + levelNodeData[i].bottomLineID.ToString() + "," + levelNodeData[i].leftLineID.ToString() + "]");

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
                    affectedBoxesHorizontal.Add(horizontalID, new int[]{ i });
                    affectedBoxesVertical.Add(verticalID, new int[] { row + (NoOfRowsOrColumns - 1) * column });
                }
                else if(row > 0 && row < NoOfRowsOrColumns - 1)
                {
                    affectedBoxesHorizontal.Add(horizontalID, new int[] { i, i - (NoOfRowsOrColumns - 1) });
                    affectedBoxesVertical.Add(verticalID, new int[] { row + ((NoOfRowsOrColumns - 1) * column) - 1, row + ((NoOfRowsOrColumns - 1) * column) });
                }
                else if(row == NoOfRowsOrColumns - 1)
                {
                    affectedBoxesHorizontal.Add(horizontalID, new int[] { i - (NoOfRowsOrColumns - 1) });
                    affectedBoxesVertical.Add(verticalID, new int[] { row + ((NoOfRowsOrColumns - 1) * column) - 1 });
                }

                column++;

                if (column >= NoOfRowsOrColumns - 1)
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

                    for (int i = 0; i < affectedBoxesHorizontal[lineID].Length; i++)
                    {
                        Box temp = m_ListOfBoxes[affectedBoxesHorizontal[lineID][i]];
                        temp.AddScore();

                        if (temp.GetScore() == 4)
                            boxActivatedCounter++;
                    }

                    break;
                case GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE:

                    for (int i = 0; i < affectedBoxesVertical[lineID].Length; i++)
                    {
                        Box temp = m_ListOfBoxes[affectedBoxesVertical[lineID][i]];
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
            foreach (Line line in m_ListOfLinesHorizontal)
            {
                if (line.IsLineActive())
                    line.SetLineInactive();                  
            }

            foreach (Line line in m_ListOfLinesVertical)
            {
                if (line.IsLineActive())
                    line.SetLineInactive();
            }
        }
    }

}