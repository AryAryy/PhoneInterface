using System;
using UnityEngine;

// This script is a class library for all data classes that are used by state machine system
namespace PhoneInterfaceCode.Database
{
    /// <summary>
    /// This class is the base class for all state machine initializer data classes. Data inside this class is used in
    /// the first initialization of the state
    /// </summary>
    public class StateMachine_InitializerData { }
    
    /// <summary>
    /// This class is the base class for all state machine state switching classes. Switch state calls pass data that
    /// includes the next state and any database related to next state, the data base will be passed from previous to
    /// next state by the state machine
    /// </summary>
    public class StateMachine_StateSwitchData
    {
        public Type nextState;
        public StateMachine_Database nextStateDatabase;
    }
    
    /// <summary>
    /// This class is the base class for all databases that need to be used by any state machine
    /// </summary>
    public class StateMachine_Database : ScriptableObject { }
}