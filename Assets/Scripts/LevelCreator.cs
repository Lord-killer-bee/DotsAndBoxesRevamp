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

        private List<Box> listOfBoxes = new List<Box>();
        private List<Line> listOfLines = new List<Line>();

        void Start()
        {
            InitializeDimensions();
            ArrangeLines();
            ArrangeBoxes();

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
                                           boardPanel }
                });

                j++;

                listOfBoxes.Add(box);

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
                                                 GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE,
                                                 boardPanel}
                    });

                    Line lineVertical = MonoObjectFactory<Line>.CreateInstance( new ObjectConstructMaterials()
                    {
                        prefab = linePrefab,
                        parameters = new object[] { new Vector3(k * (m_lineWidth + m_dotWidth), -(j + 0.5f) * (m_lineHeight + m_dotHeight), 1),
                                                new Vector2(m_lineHeight, m_dotWidth),
                                                new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                new Vector2(((1 - BoardPercentage) / 2), ((1 + BoardPercentage) / 2)),
                                                GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE,
                                                boardPanel}
                    });

                    listOfLines.Add(lineHorizontal);
                    listOfLines.Add(lineVertical);


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
            return listOfBoxes;
        }

        public List<Line> GetListOfLines()
        {
            return listOfLines;
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
    }

}