using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MargarineFramework.AI.StateMachine
{
    public abstract class AiStateBehaviour : MonoBehaviour
    {
        public UnityEvent OnStateEnterEvent =  new UnityEvent();
        public UnityEvent OnStateExitEvent =  new UnityEvent();
 
        protected virtual void OnEnable()
        {
            OnStateEnter();
            OnStateEnterEvent?.Invoke();
        }

        protected virtual void OnDisable()
        {
            OnStateExit();
            OnStateExitEvent?.Invoke();
        }

        protected abstract void OnStateEnter();
        protected abstract void OnStateExit();
    }
}