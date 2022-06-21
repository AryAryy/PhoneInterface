using System;
using System.Linq;
using PhoneInterfaceCode.Database;
using PhoneInterfaceCode.Extensions;
using PhoneInterfaceCode.InputSystem;
using PhoneInterfaceCode.ScriptableObjects.PhoneInterface;
using PhoneInterfaceCode.StateMachine;

namespace PhoneInterfaceCode.PhoneInterface
{
    /// <summary>
    /// This class handles phone interface in messages card overview state
    /// </summary>
    public class PhoneInterface_CardOverview_Messages_State : State_Base
    {
        private PhoneInterface_App_Messages_Database database_;
        private PhoneInterface_CardOverview_Messages_InitializerData initializerData_;

        public PhoneInterface_CardOverview_Messages_State(
            PhoneInterface_CardOverview_Messages_InitializerData initializerData,
            PhoneInterface_App_Messages_Database database) : base(initializerData, database)
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
            initializerData_.MessagesCardOverviewPanel.SetActive(true);
            InitializeCard();
        }

        public override void OnExitState()
        {
            canReceivePlayerInput = false;
            initializerData_.MessagesCardOverviewPanel.SetActive(false);
        }

        public override void SetDatabase(StateMachine_Database database)
        {
            if (!database) throw new Exception("State machine database is null, please pass a valid database");
            database_ = database as PhoneInterface_App_Messages_Database;
        }

        protected override void SubscribeToUserInput() =>
            InputManager.Instance.OnPhoneInterfaceInput += HandleReturning;
        
        /// <summary>
        /// Initializes card once state machine enters this state, sets the data to graphical interface 
        /// </summary>
        private void InitializeCard()
        {
            initializerData_.MessagesCardPictureRenderer.sprite = database_.database.First().ContactPicture;
            initializerData_.MessagesCardContactNameText.text = database_.database.First().ContactName;
            initializerData_.MessagesCardFullMessageText.text = database_.database.First().FullMessage;
        }

        /// <summary>
        /// Handles returning to messages app overview state
        /// </summary>
        private void HandleReturning(InputManager.PhoneInterfaceActionType actionType)
        {
            if (!canReceivePlayerInput || actionType != InputManager.PhoneInterfaceActionType.Return) return;

            canSwitchState = true;
            var newStateData = new StateMachine_StateSwitchData()
                {nextState = typeof(PhoneInterface_AppOverview_Messages_State)};
            stateMachine.SwitchToState(newStateData, false);
        }
    }
}