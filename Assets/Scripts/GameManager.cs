using ObjectFactory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger;

namespace DnBGame
{
	public class GameManager : MonoBehaviour
	{
		public GameEnums.EPlayerType player1Type, player2Type;

		private Player m_Player_1, m_Player_2;

		public static Player activePlayer { get;  private set;}
		public static Player inactivePlayer { get; private set; }

		protected bool isPlayer1Turn;

		private void Awake()
		{
			GameLogger.SetLogStatus(GameLogger.ELoggingStatus.ENABLE_LOGGING);

			GameEventManager.LevelCreated += CreatePlayers;
			GameEventManager.LinePlaced += PlayerLinePlaced;
		}

		void Start()
		{

		}

		void Update()
		{

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
		}

		void CreatePlayers()
		{
			m_Player_1 = NonMonoObjectFactory<Player>.CreateInstance(() => new Player("Player 1", new Color(1, 0, 0, 1), player1Type));
			m_Player_2 = NonMonoObjectFactory<Player>.CreateInstance(() => new Player("Player 2", new Color(0, 0, 1, 1), player2Type));

			SetInitiatingPlayer();
		}

		void PlayerLinePlaced()
		{
			SwitchPlayers();
		}
	}
}