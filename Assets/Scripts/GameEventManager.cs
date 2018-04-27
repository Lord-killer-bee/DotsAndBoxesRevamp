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

        public static event NoParamGameplayEvents ReferencesRegistered, LevelCreated;
        public static event ButtonPressGameplayEvents LinePlaced;
        public static event BoolParamGameplayEvents BoxScoredToFour;
        public static event IntParamGameplayEvents BoxScoreValidForChain;


		public static void TriggerReferencesRegistered()
		{
			if (ReferencesRegistered != null)
				ReferencesRegistered();
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

        public static void TriggerBoxScoredToFour(bool status)
        {
            if(BoxScoredToFour != null)
                BoxScoredToFour(status);
        }

        public static void TriggerBoxScoreValidForChain(int param)
        {
            if (BoxScoreValidForChain != null)
                BoxScoreValidForChain(param);
        }

    }
}