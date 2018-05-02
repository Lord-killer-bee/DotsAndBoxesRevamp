using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
	public class GameEventManager
	{

		public delegate void NoParamGameplayEvents();
        public delegate void IntParamGameplayEvents(int param);
        public delegate void BoolParamGameplayEvents(bool status);
        public delegate void ButtonPressGameplayEvents(GameStructs.LineID lineID);

        public static event NoParamGameplayEvents ReferencesRegistered, SwitchPlayers, BoxScoredToFour, LevelCreated, PlayersCreated, PlayerTurnsUpdated;
        public static event ButtonPressGameplayEvents LinePlaced;
        public static event IntParamGameplayEvents BoxScoreValidForChain;


		public static void TriggerReferencesRegistered()
		{
			if (ReferencesRegistered != null)
				ReferencesRegistered();
		}

        public static void TriggerSwitchPlayers()
        {
            if (SwitchPlayers != null)
                SwitchPlayers();
        }

        public static void TriggerPlayersCreated()
        {
            if (PlayersCreated != null)
                PlayersCreated();
        }

        public static void TriggerLevelCreated()
		{
			if (LevelCreated != null)
				LevelCreated();
		}

		public static void TriggerLinePlaced(GameStructs.LineID lineID)
		{
			if (LinePlaced != null)
				LinePlaced(lineID);
		}

        public static void TriggerBoxScoredToFour()
        {
            if(BoxScoredToFour != null)
                BoxScoredToFour();
        }

        public static void TriggerBoxScoreValidForChain(int param)
        {
            if (BoxScoreValidForChain != null)
                BoxScoreValidForChain(param);
        }

        public static void TriggerPlayerTurnsUpdated()
        {
            if (PlayerTurnsUpdated != null)
                PlayerTurnsUpdated();
        }

    }
}