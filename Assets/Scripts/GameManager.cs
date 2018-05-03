﻿using ObjectFactory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger;

namespace DnBGame
{
	public class GameManager : MonoBehaviour
	{
		private GameEnums.EPlayerType m_Player1Type, m_Player2Type;

		public GameEnums.EGameMode gameMode;

        public GameEnums.EGameDifficultyMode difficultyMode;

		private Player m_Player_1, m_Player_2;

		public Player activePlayer { get;  private set;}
		public Player inactivePlayer { get; private set; }

		protected bool isPlayer1Turn;

		private void Awake()
		{
			GameLogger.SetLogStatus(GameLogger.ELoggingStatus.ENABLE_LOGGING);

			GameEventManager.LevelCreated += CreatePlayers;
			GameEventManager.SwitchPlayers += SwitchPlayers;
		}

		void SetInitiatingPlayer()
		{
			int temp = Random.Range(0, 1);

			if(temp == 0)
			{
				activePlayer = m_Player_1;
				inactivePlayer = m_Player_2;

				isPlayer1Turn = true;
			}
			else
			{
				activePlayer = m_Player_2;
				inactivePlayer = m_Player_1;

				isPlayer1Turn = false;
			}

			UpdatePlayerTurns();
		}

		void SwitchPlayers()
		{
			Player temp = activePlayer;
			activePlayer = inactivePlayer;
			inactivePlayer = temp;

			isPlayer1Turn = !isPlayer1Turn;

			UpdatePlayerTurns();
		}

		void UpdatePlayerTurns()
		{
			activePlayer.SetPlayerTurn(true);
			inactivePlayer.SetPlayerTurn(false);

            GameEventManager.TriggerPlayerTurnsUpdated();
		}

		void CreatePlayers()
		{
			switch (gameMode)
			{
				case GameEnums.EGameMode.PvP:

					m_Player1Type = GameEnums.EPlayerType.PLAYER_1;
					m_Player2Type = GameEnums.EPlayerType.PLAYER_2;

					m_Player_1 = NonMonoObjectFactory<Player>.CreateInstance(() => new Player("Player 1", new Color(1, 0, 0, 1), m_Player1Type));
					m_Player_2 = NonMonoObjectFactory<Player>.CreateInstance(() => new Player("Player 2", new Color(0, 0, 1, 1), m_Player2Type));
					break;
				case GameEnums.EGameMode.PvAI:

					m_Player1Type = GameEnums.EPlayerType.PLAYER_1;
					m_Player2Type = GameEnums.EPlayerType.PLAYER_AI;
					m_Player_1 = NonMonoObjectFactory<Player>.CreateInstance(() => new Player("Player 1", new Color(1, 0, 0, 1), m_Player1Type));
					m_Player_2 = NonMonoObjectFactory<AIPlayer>.CreateInstance(() => new AIPlayer("Player 2", new Color(0, 0, 1, 1), m_Player2Type));

					break;
				case GameEnums.EGameMode.AIvAI:

					m_Player1Type = GameEnums.EPlayerType.PLAYER_AI;
					m_Player2Type = GameEnums.EPlayerType.PLAYER_AI;

					break;
			}

			

			SetInitiatingPlayer();

            GameEventManager.TriggerPlayersCreated();
		}
		
	}
}