using System;
using System.Collections.Generic;
using System.Linq;
using PhoneInterfaceCode.Database;
using PhoneInterfaceCode.Extensions;
using PhoneInterfaceCode.InputSystem;
using PhoneInterfaceCode.StateMachine;
using UnityEngine;

namespace PhoneInterfaceCode.PhoneInterface
{
    /// <summary>
    /// This class handles phone interface while it's in main menu state
    /// </summary>
    public class PhoneInterface_MainMenu_State : State_Base
    {
        private struct AppData
        {
            public Transform app;
            public AppIdentifier appIdentifier;
        }
        
        private PhoneInterface_MainMenu_InitializerData initializerData_;

        private readonly List<AppData> apps_ = new List<AppData>();
        private AppData activeApp_;
        private int activeAppIndex_;

        public PhoneInterface_MainMenu_State(PhoneInterface_MainMenu_InitializerData initializerData,
            StateMachine_Database database) : base(initializerData, database)
        {
            initializerData_ = initializerData;
            InitializeState();
        }

        protected override void InitializeState()
        {
            SubscribeToUserInput();
            var orderedApps = initializerData_.Apps.OrderByDescending(a => a.position.y).ToList();
            foreach (var app in orderedApps)
            {
                app.TryGetComponent(out AppIdentifier identifier);
                if (!identifier) throw new Exception("App object doesn't have 'AppIdentifier' script attached");
                var data = new AppData() {app = app, appIdentifier = identifier};
                apps_.Add(data);
                if (identifier.AppType == initializerData_.DefaultApp) activeApp_ = data;
            }
        }
        
        public override void OnEnterState()
        {
            CoroutineManager.Instance.RunNextUpdate(() => canReceivePlayerInput = true);
            ActivateCard(activeAppIndex_);
            initializerData_.UnlockAnimator.SetTrigger(initializerData_.UnlockAnimationParameter);
            CoroutineManager.Instance.RunWithDelay(() => initializerData_.MainMenuPanel.SetActive(true),
                initializerData_.UnlockAnimationDuration);
        }

        public override void OnExitState()
        {
            canReceivePlayerInput = false;
            initializerData_.MainMenuPanel.SetActive(false);
            canSwitchState = true;
        }
        
        public override void SetDatabase(StateMachine_Database database) { }

        protected override void SubscribeToUserInput()
        {
            InputManager.Instance.OnPhoneInterfaceInput += HandleMenuNavigation;
            InputManager.Instance.OnPhoneInterfaceInput += HandleMenuSelection;
        }

        /// <summary>
        /// This function handles menu navigation
        /// </summary>
        /// <param name="actionType"> Type of input action </param>
        private void HandleMenuNavigation(InputManager.PhoneInterfaceActionType actionType)
        {
            bool irrelevantAction = actionType != InputManager.PhoneInterfaceActionType.NavigationUp &&
                                    actionType != InputManager.PhoneInterfaceActionType.NavigationDown;
            if (!canReceivePlayerInput || irrelevantAction) return;

            bool goUp = actionType == InputManager.PhoneInterfaceActionType.NavigationUp;
            bool goDown = actionType == InputManager.PhoneInterfaceActionType.NavigationDown;
            bool canGoUp = activeAppIndex_ - 1 >= 0;
            bool canGoDown = activeAppIndex_ + 1 <= apps_.Count - 1;

            if (goUp && canGoUp) ActivateCard(activeAppIndex_ - 1);
            if (goDown && canGoDown) ActivateCard(activeAppIndex_ + 1);
        }
        
        /// <summary>
        /// This function activated selected card
        /// </summary>
        /// <param name="selectedCard"> Selected card to activated </param>
        private void ActivateCard(int selectedCard)
        {
            activeApp_ = apps_[selectedCard];
            initializerData_.SelectedAppBackground.position = activeApp_.app.position;
            activeAppIndex_ = selectedCard;
        }

        /// <summary>
        /// This function handles selecting in menu
        /// </summary>
        private void HandleMenuSelection(InputManager.PhoneInterfaceActionType actionType)
        {
            if (!canReceivePlayerInput || actionType != InputManager.PhoneInterfaceActionType.Select) return;
            var newStateData = new StateMachine_StateSwitchData() {nextState = GetNextState()};
            stateMachine.SwitchToState(newStateData, false);

            Type GetNextState()
            {
                switch (activeApp_.appIdentifier.AppType)
                {
                    case (PhoneManager.PhoneInterfaceAppNames.Calls):
                        return typeof(PhoneInterface_AppOverview_Calls_State);

                    case (PhoneManager.PhoneInterfaceAppNames.Contacts):
                        return typeof(PhoneInterface_AppOverview_Contacts_State);

                    case (PhoneManager.PhoneInterfaceAppNames.Messages):
                        return typeof(PhoneInterface_AppOverview_Messages_State);

                    default: return typeof(PhoneInterface_MainMenu_State);
                }
            }
        }
    }
}