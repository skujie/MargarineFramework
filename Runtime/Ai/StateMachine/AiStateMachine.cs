using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MargarineFramework.AI.StateMachine
{
    public class AiStateMachine : MonoBehaviour
    {
        public AiStateBehaviour[] StateBehaviours => _stateBehaviours;

        [SerializeField] AiStateBehaviour[] _stateBehaviours = System.Array.Empty<AiStateBehaviour>();
    }
}