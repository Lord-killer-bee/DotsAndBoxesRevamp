using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnBGame
{
    public interface IAIBehaviour
    {

        void OnEnter();
        void OnTrigger();
        void OnExit();
        bool IsAIInitialized();

    }
}