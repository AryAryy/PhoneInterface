using System;
using System.Linq;
using BeneathTheNight.Data.Database;
using BeneathTheNight.Extensions;
using BeneathTheNight.InputSystem;
using BeneathTheNight.ScriptableObjects.PhoneInterface;
using BeneathTheNight.StateMachine;

namespace BeneathTheNight.PhoneInterface
{
    /// <summary>
    /// This class handles phone interface in contacts card overview state
    /// </summary>
    public class PhoneInterface_CardOverview_Contacts_State : State_Base
    {
        private PhoneInterface_App_Contacts_Database database_;
        private PhoneInterface_CardOverview_Contacts_InitializerData initializerData_;

        public PhoneInterface_CardOverview_Contacts_State(
            PhoneInterface_CardOverview_Contacts_InitializerData initializerData,
            PhoneInterface_App_Contacts_Database database) : base(initializerData, database)
        {
            initializerData_ = initializerData;
            InitializeState();
        }

        protected override void InitializeState()
        {
            SubscribeToUserInput();
        }

        public override void OnEnterState()
        {
            CoroutineManager.Instance.RunNextUpdate(() => canReceivePlayerInput = true);
            initializerData_.ContactCardOverviewPanel.SetActive(true);
            InitializeCard();
        }

        public override void OnExitState()
        {
            canReceivePlayerInput = false;
            initializerData_.ContactCardOverviewPanel.SetActive(false);
        }

        public override void SetDatabase(StateMachine_Database database)
        {
            if (!database) throw new Exception("State machine database is null, please pass a valid database");
            database_ = database as PhoneInterface_App_Contacts_Database;
        }

        protected override void SubscribeToUserInput()
            => InputManager.Instance.OnPhoneInterfaceInput += HandleReturning;

        /// <summary>
        /// Initializes card once state machine enters this state, sets the data to graphical interface 
        /// </summary>
        private void InitializeCard()
        {
            initializerData_.ContactCardPictureRenderer.sprite = database_.database.First().ContactPicture;
            initializerData_.ContactCardNameText.text = database_.database.First().ContactName;
            initializerData_.ContactCardNumberText.text = database_.database.First().ContactNumber;
            initializerData_.ContactCardContactDescriptionText.text = database_.database.First().ContactDescription;
        }

        /// <summary>
        /// Handles returning to contacts app overview state
        /// </summary>
        private void HandleReturning(InputManager.PhoneInterfaceActionType actionType)
        {
            if (!canReceivePlayerInput || actionType != InputManager.PhoneInterfaceActionType.Return) return;

            canSwitchState = true;
            var newStateData = new StateMachine_StateSwitchData()
                {nextState = typeof(PhoneInterface_AppOverview_Contacts_State)};
            stateMachine.SwitchToState(newStateData, false);
        }
    }
}