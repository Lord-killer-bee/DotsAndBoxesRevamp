﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DnBGame
{
    public class Line : MonoBehaviour
    {
        public Sprite activeSprite, defaultSprite;

        private Button m_ClickableButton;
        private Image m_PressedImage;


        public void Initialize(Vector3 position, Vector2 size, Vector2 anchorMin, Vector2 anchorMax, GameEnums.E_LineRotationCode rotationCode, GameObject panel)
        {
            m_ClickableButton = gameObject.GetComponent<Button>();
            m_ClickableButton.onClick.AddListener(() => OnClick());
            m_ClickableButton.gameObject.transform.SetParent(panel.transform);

            m_PressedImage = m_ClickableButton.GetComponent<Image>();

            m_ClickableButton.GetComponent<RectTransform>().anchorMin = anchorMin;
            m_ClickableButton.GetComponent<RectTransform>().anchorMax = anchorMax;

            m_ClickableButton.GetComponent<RectTransform>().anchoredPosition = position;
            m_ClickableButton.GetComponent<RectTransform>().sizeDelta = size;
            m_ClickableButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (rotationCode == GameEnums.E_LineRotationCode.HORIZONTAL_ROTATION_CODE)
            {
                m_ClickableButton.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                m_ClickableButton.tag = GameConstants.TAG_HORIZONTAL;
            }
            else if (rotationCode == GameEnums.E_LineRotationCode.VERTICAL_ROTATION_CODE)
            {
                m_ClickableButton.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 90);
                m_ClickableButton.tag = GameConstants.TAG_VERTICAL;
            }
        }

        private void OnClick()
        {
            throw new NotImplementedException();
        }

        private void SetLineJustActiveBehaviour()
        {
            m_PressedImage.sprite = activeSprite;
        }

        private void SetLineInactiveBehaviour()
        {
            m_PressedImage.sprite = defaultSprite;
        }

        private void AssignColorToLine(Color color)
        {
            m_PressedImage.color = color;
        }
    }
}