using System;
using System.Collections.Generic;
using PhoneInterfaceCode.Database;
using PhoneInterfaceCode.InputSystem;
using PhoneInterfaceCode.ScriptableObjects.PhoneInterface;
using PhoneInterfaceCode.StateMachine;
using UnityEngine;

namespace PhoneInterfaceCode.PhoneInterface
{
    /// <summary>
    /// This class is the main handler for phone interface
    /// </summary>
    public class PhoneManager : MonoBehaviour
    {
        public enum PhoneInterfaceAppNames
        {
            Calls,
            Contacts,
            Messages,
        }
 
        private StateMachine.StateMachine stateMachine_;
        [SerializeField] private GameObject interfaceObject;

        [Header("Phone Interface Initializer Data")] 
        [SerializeField] private PhoneInterface_MainMenu_InitializerData mainMenuInitializerData;
        [SerializeField] private PhoneInterface_CardAppOverview_InitializerData appCallsInitializerData;
        [SerializeField] private PhoneInterface_CardAppOverview_InitializerData appContactsInitializerData;
        [SerializeField] private PhoneInterface_CardAppOverview_InitializerData appMessagesInitializerData;
        
        [SerializeField] private PhoneInterface_CardOverview_Calls_InitializerData cardCallsInitializerData;
        [SerializeField] private PhoneInterface_CardOverview_Contacts_InitializerData cardContactsInitializerData;
        [SerializeField] private PhoneInterface_CardOverview_Messages_InitializerData cardMessagesInitializerData;
        
        [Header("Phone Interface Database")] [Space] [Space]
        [SerializeField] private PhoneInterface_App_Contacts_Database appContactsDatabase;
        [SerializeField] private PhoneInterface_App_Messages_Database appMessagesDatabase;

        private void Awake()
        {
            stateMachine_ = GetComponent<StateMachine.StateMachine>();
            InitializeStateMachine();
            InputManager.Instance.OnPhoneInterfaceInput += ControlPhone;
        }

        /// <summary>
        /// Initializes phone interface state machine
        /// </summary>
        private void InitializeStateMachine()
        {
            var states = new Dictionary<Type, State_Base>()
            {
                {
                    typeof(PhoneInterface_MainMenu_State),
                    new PhoneInterface_MainMenu_State(mainMenuInitializerData, null)
                },
                {
                    typeof(PhoneInterface_AppOverview_Calls_State),
                    new PhoneInterface_AppOverview_Calls_State(appCallsInitializerData, appContactsDatabase)
                },
                {
                    typeof(PhoneInterface_AppOverview_Contacts_State),
                    new PhoneInterface_AppOverview_Contacts_State(appContactsInitializerData, appContactsDatabase)
                },
                {
                    typeof(PhoneInterface_AppOverview_Messages_State),
                    new PhoneInterface_AppOverview_Messages_State(appMessagesInitializerData, appMessagesDatabase)
                },

                {
                    typeof(PhoneInterface_CardOverview_Calls_State),
                    new PhoneInterface_CardOverview_Calls_State(cardCallsInitializerData, null)
                },
                {
                    typeof(PhoneInterface_CardOverview_Contacts_State),
                    new PhoneInterface_CardOverview_Contacts_State(cardContactsInitializerData, null)
                },
                {
                    typeof(PhoneInterface_CardOverview_Messages_State),
                    new PhoneInterface_CardOverview_Messages_State(cardMessagesInitializerData, null)
                }
            };
            stateMachine_.InitializerStateMachine(states);
        }

        /// <summary>
        /// Opens/Closes phone interface
        /// </summary>
        /// <param name="actionType"> Type of input action </param>
        private void ControlPhone(InputManager.PhoneInterfaceActionType actionType)
        {
            if (actionType == InputManager.PhoneInterfaceActionType.OpenPhone) OpenPhone();
            if (actionType == InputManager.PhoneInterfaceActionType.ClosePhone) ClosePhone();
        }

        /// <summary>
        /// Opening phone interface
        /// </summary>
        private void OpenPhone()
        {
            interfaceObject.SetActive(true);
            var nextStateData = new StateMachine_StateSwitchData() {nextState = typeof(PhoneInterface_MainMenu_State)};
            stateMachine_.SwitchToState(nextStateData, true);
        }

        /// <summary>
        /// Closing phone interface
        /// </summary>
        private void ClosePhone() => interfaceObject.SetActive(false);
    }
}