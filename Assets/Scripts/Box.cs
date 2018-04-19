using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
    public class Box : MonoBehaviour
    {

        private GameObject m_Box;

        public void Initialize(Vector3 position, Vector2 size, Vector2 anchorMin, Vector2 anchorMax, GameObject panel)
        {
            m_Box = this.gameObject;
            m_Box.transform.SetParent(panel.transform);

            m_Box.GetComponent<RectTransform>().anchorMin = anchorMin;
            m_Box.GetComponent<RectTransform>().anchorMax = anchorMax;

            m_Box.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            m_Box.GetComponent<RectTransform>().anchoredPosition = position;
            m_Box.GetComponent<RectTransform>().sizeDelta = size;

            m_Box.SetActive(false);

            //Set score to zero
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}