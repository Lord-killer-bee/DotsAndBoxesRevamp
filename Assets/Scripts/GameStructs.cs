using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
    public static class GameStructs
    {
        public struct NodeData
        {
            public LineID topLineID, rightLineID, bottomLineID, leftLineID;
        }

        public struct LineID
        {
            public int ID;
            public GameEnums.E_LineRotationCode rotation;
        }

		public struct NeighbouringBoxes
		{
			public int topBoxID, rightBoxID, bottomBoxID, leftBoxID;
		}

        public struct AIAlgorithmPackage
        {
            public GameEnums.EAIBehaviour tier1Algorithm, tier2Algorithm;
        }

    }
}