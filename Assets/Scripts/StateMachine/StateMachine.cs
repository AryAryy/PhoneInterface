using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeneathTheNight.Data.Database;
using UnityEngine;

namespace BeneathTheNight.StateMachine
{
    /// <summary>
    /// This class provides a state machine functionality, switching between states is handled by each state individually,
    /// this scrip should be attached to any object that needs to use it
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<Type, State_Base> states_ = new Dictionary<Type, State_Base>();
        private State_Base currentState_;
        private Coroutine stateSwitchCo_;

        [Header("DEBUG TOOLS")] 
        [SerializeField] private string currentState;

        /// <summary>
        /// Creates a dictionary of all necessary states, is called by an external manager
        /// </summary>
        /// <param name="states"> A dictionary of states that will be used by this state machine </param>
        public void InitializerStateMachine(Dictionary<Type, State_Base> states)
        {
            states_ = states;
            foreach (var state in states_) state.Value.StateMachine = this;
        }

        /// <summary>
        /// Switching to a new state, is called by an external handler or states
        /// </summary>
        /// <param name="newStateData"> A data class containing next state and a database related to that state if
        /// necessary </param>
        /// <param name="setupDatabase"> True if database for the next state needs to be changed </param>
        public void SwitchToState(StateMachine_StateSwitchData newStateData, bool setupDatabase)
        {
            var state = states_.Values.Single(s => s.GetType() == newStateData.nextState);
            currentState_?.OnExitState();
            if (setupDatabase) state.SetDatabase(newStateData.nextStateDatabase);

            stateSwitchCo_ = StartCoroutine(StateSwitchRoutine());
            IEnumerator StateSwitchRoutine()
            {
                if (currentState_ != null) yield return new WaitUntil(() => currentState_.CanSwitchState);
                
                state.OnEnterState();
                currentState_ = state;
                DEBUG_SetStateName();
            }
        }

        /// <summary>
        /// DEBUG FUNCTION, shows current state name in the inspector
        /// </summary>
        private void DEBUG_SetStateName()
        {
            var stateName = currentState_.GetType().ToString();
            var strings = stateName.Split('.');
            currentState = strings.Last();
        }
    }
}