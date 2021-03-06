﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger;
using ObjectFactory;

namespace DnBGame
{

    public class Player
    {

        private unsafe string m_Name;
        private int m_Score;
        private GameEnums.EPlayerType m_PlayerType;
        private bool m_IsPlayerTurn;
        private Color m_PlayerColor;

        public Player(string name, Color color, GameEnums.EPlayerType playerType)
        {
            m_Name = name;
            m_PlayerType = playerType;
            m_Score = 0;
            m_PlayerColor = color;

			GameLogger.LogMessage(name + " Created!!");
        }

        public string GetName()
        {
            return m_Name;
        }

        public void SetScore(int score)
        {
            m_Score = score;
        }

        public int GetScore()
        {
            return m_Score;
        }

        public GameEnums.EPlayerType GetPlayerType()
        {
            return m_PlayerType;
        }

        public bool IsPlayerTurn()
        {
            return m_IsPlayerTurn;
        }

        public virtual void SetPlayerTurn(bool status)
        {
            m_IsPlayerTurn = status;
			GameLogger.LogMessage(m_Name + " Turn: " + status);
        }

        public Color GetPlayerColor()
        {
            return m_PlayerColor;
        }
    }  

}