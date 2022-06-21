using BeneathTheNight.Data.Database;
using UnityEngine;

namespace BeneathTheNight.StateMachine
{
    /// <summary>
    /// This class is the base class for all state machine states 
    /// </summary>
    public abstract class State_Base
    {
        protected StateMachine stateMachine;
        protected Coroutine stateSwitchCo;
        protected bool canReceivePlayerInput, canSwitchState, delayEnterState;

        public StateMachine StateMachine { set => stateMachine = value; }
        public bool CanSwitchState => canSwitchState;

        protected State_Base(StateMachine_InitializerData initializerData, StateMachine_Database database) { }
        
        /// <summary>
        /// Main initializer for each state, only called once by state machine initializer
        /// </summary>
        protected abstract void InitializeState();
        
        /// <summary>
        /// Called each time state machine enters this state
        /// </summary>
        public abstract void OnEnterState();
        
        /// <summary>
        /// Called each time state machine exits this state
        /// </summary>
        public abstract void OnExitState();
        
        /// <summary>
        /// Called by each state if necessary, if the state needs to receive player input
        /// </summary>
        protected abstract void SubscribeToUserInput();
        
        /// <summary>
        /// Called by state machine if necessary, changes the database for this state
        /// </summary>
        /// <param name="database"> New database for this state </param>
        public virtual void SetDatabase(StateMachine_Database database) { }
    }
}