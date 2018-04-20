using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
	public class GameEventManager
	{

		public delegate void GameplayEvents();

		public static event GameplayEvents LevelCreated, LinePlaced;


		public static void TriggerLevelCreated()
		{
			if (LevelCreated != null)
				LevelCreated();
		}

		public static void TriggerLinePlaced()
		{
			if (LinePlaced != null)
				LinePlaced();
		}
	}
}