using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
    public static class GameEnums
    {

		public enum EGameMode
		{
			PvP,
			PvAI,
			AIvAI
		}

        public enum ELineRotationCode
        {
            HORIZONTAL_ROTATION_CODE,
            VERTICAL_ROTATION_CODE
        }

        public enum EPlayerType
        {
            PLAYER_1,
            PLAYER_2,
            PLAYER_AI
        }		

        public enum EGameDifficultyMode
        {
            EASY,
            MEDIUM,
            HARD
        }

        public enum EAIBehaviour
        {
            RANDOM_PLACEMENT,
            BASIC_CHAINING,
            PRIORITY_CHAINING,
            SACRIFICIAL_CHAINING
        }

    }
}