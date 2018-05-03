using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{

    public class RandomPlacementAI : IAIBehaviour
    {
        private bool m_AIInitialized;

        private LevelCreator m_levelCreator;
        private List<int> listOfBoxesOfScoreLessThan2 = new List<int>();
        private Dictionary<int, GameStructs.NodeData> relevantNodeData = new Dictionary<int, GameStructs.NodeData>();

        public void OnEnter()
        {
            m_AIInitialized = true;
            GetRequiredData();
            GatherAndGenerateRequiredData();
        }

        private void GetRequiredData()
        {
            m_levelCreator = ReferenceRegistry.instance.GetLevelCreator();
        }

        //this should only be called once when this AI is initialized
        private void GatherAndGenerateRequiredData()
        {
            listOfBoxesOfScoreLessThan2.Clear();

            List<Box> boxesList = m_levelCreator.GetListOfBoxes();
            foreach (Box item in boxesList)
            {
                if(item.GetScore() < 2)
                {
                    listOfBoxesOfScoreLessThan2.Add(item.GetBoxID());
                }
            }

            Dictionary<int, GameStructs.NodeData> nodeData = m_levelCreator.GetLevelNodeData();

            foreach (var item in listOfBoxesOfScoreLessThan2)
            {
                relevantNodeData.Add(item, nodeData[item]);
            }
        }

        public void OnTrigger()
        {


        }

        public void OnExit()
        {
            m_AIInitialized = false;
            InitializeNextTierAI();
        }


        public bool IsAIInitialized()
        {
            return m_AIInitialized;
        }

        private void InitializeNextTierAI()
        {
            throw new NotImplementedException();
        }

    }
}