using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DnBGame
{
    public class Box : MonoBehaviour
    {

		private int m_Score, m_ChainID;
        private GameObject m_Box;
		private Image m_BoxImage;

        public void Initialize(Vector3 position, Vector2 size, Vector2 anchorMin, Vector2 anchorMax, GameObject panel)
        {
            m_Box = this.gameObject;
            m_Box.transform.SetParent(panel.transform);

            m_Box.GetComponent<RectTransform>().anchorMin = anchorMin;
            m_Box.GetComponent<RectTransform>().anchorMax = anchorMax;

			m_BoxImage = gameObject.GetComponent<Image>();

            m_Box.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            m_Box.GetComponent<RectTransform>().anchoredPosition = position;
            m_Box.GetComponent<RectTransform>().sizeDelta = size;

            m_Box.SetActive(false);

			m_Score = 0;
        }

		public void ActivateBox(Color color)
		{
			AssignColorToBox(color);
			m_Box.SetActive(true);
		}

		private void AssignColorToBox(Color color)
		{
			m_BoxImage.color = color;
		}

    }
}