using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour {

    #region Public parameters

    public GameObject boardPanel;
    public int NoOfRowsOrColumns;
    public float BoardPercentage;
    public float dotDimension;

    #endregion

    #region Private parameters

    private float m_dotWidth, m_dotHeight;
    private float m_lineWidth, m_lineHeight;
    private float m_boxWidth, m_boxHeight;

    #endregion

    #region Constant parameters

    private const int HORIZONTAL_ROTATION_CODE = 1;
    private const int VERTICAL_ROTATION_CODE = -1;

    #endregion

    private List<Box> listOfBoxes = new List<Box>();
    private List<Line> listOfLines = new List<Line>();

    // Use this for initialization
    void Start ()
    {
        InitializeDimensions();
        ArrangeLines();
        ArrangeBoxes();
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
            //This doesnt instantiate the object in scene.
            //Create an object factory to instantiate objects
            Box box = new Box(new Vector3((m_lineWidth + m_dotWidth) * (j + 0.5f), (-m_lineHeight - m_dotHeight) * (k + 0.5f), 0));
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
                Line lineHorizontal = new Line(new Vector3((j + 0.5f) * (m_lineWidth + m_dotWidth),
                                    -k * (m_lineHeight + m_dotHeight), 1),
                                    new Vector2(m_lineWidth, m_dotHeight),
                                    HORIZONTAL_ROTATION_CODE);

                Line lineVertical = new Line(new Vector3(k * (m_lineWidth + m_dotWidth),
                                    -(j + 0.5f) * (m_lineHeight + m_dotHeight), 1),
                                    new Vector2(m_lineHeight, m_dotWidth),
                                    VERTICAL_ROTATION_CODE);

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
        return (((boardPanel.GetComponent<RectTransform>().rect.width) * BoardPercentage) - (NoOfRowsOrColumns * m_dotWidth)) / (NoOfRowsOrColumns - 1);
    }

    private float GetLineWidth()
    {
        return  (((boardPanel.GetComponent<RectTransform>().rect.height) * BoardPercentage) - (NoOfRowsOrColumns * m_dotHeight)) / (NoOfRowsOrColumns - 1);
    }

    #endregion
}
