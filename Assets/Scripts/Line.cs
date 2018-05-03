using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DnBGame
{
    public class Line : MonoBehaviour
    {
		public GameObject highlightImage;
        public Text debugText;

        private GameStructs.LineID m_lineID;// Set this variable in the level setter

        private Button m_ClickableButton;
		private Image m_PressedImage;

		private bool m_JustActive, m_IsOpen = true;

		public void Initialize(Vector3 position, Vector2 size, Vector2 anchorMin, Vector2 anchorMax, GameStructs.LineID lineID, GameObject panel)
        {
            m_ClickableButton = gameObject.GetComponent<Button>();
            m_ClickableButton.onClick.AddListener(() => OnClick());
            m_ClickableButton.gameObject.transform.SetParent(panel.transform);

			m_PressedImage = gameObject.GetComponent<Image>();

			m_ClickableButton.GetComponent<RectTransform>().anchorMin = anchorMin;
            m_ClickableButton.GetComponent<RectTransform>().anchorMax = anchorMax;

            m_ClickableButton.GetComponent<RectTransform>().anchoredPosition = position;
            m_ClickableButton.GetComponent<RectTransform>().sizeDelta = size;
            m_ClickableButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            m_lineID = lineID;

            if (m_lineID.rotation == GameEnums.ELineRotationCode.HORIZONTAL_ROTATION_CODE)
            {
                m_ClickableButton.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                m_ClickableButton.tag = GameConstants.TAG_HORIZONTAL;
            }
            else if (m_lineID.rotation == GameEnums.ELineRotationCode.VERTICAL_ROTATION_CODE)
            {
                m_ClickableButton.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 90);
                m_ClickableButton.tag = GameConstants.TAG_VERTICAL;
            }


            //debugText.text = m_lineID.ToString();
		}

        private void OnClick()
        {
			m_ClickableButton.enabled = false;
            m_IsOpen = false;
			AssignColorToLine(ReferenceRegistry.instance.GetGameManager().activePlayer.GetPlayerColor());
			GameEventManager.TriggerLinePlaced(m_lineID);

			SetLineToActive();
		}

        #region Public Getters

        public GameStructs.LineID GetLineID()
        {
            return m_lineID;
        }

        public bool IsLineActive()
        {
            return m_JustActive;
        }

        //Open - Line is still placeable, not Open - Line is already placed
        public bool IsLineOpen()
        {
            return m_IsOpen;
        }

#endregion

		private void SetLineToActive()
        {
			highlightImage.SetActive(true);
			m_JustActive = true;
		}

        public void SetLineInactive()
        {
			if (m_JustActive)
			{
				highlightImage.SetActive(false);
				m_JustActive = false;
			}
		}

        private void AssignColorToLine(Color color)
        {
            m_PressedImage.color = color;
        }
        
    }
}