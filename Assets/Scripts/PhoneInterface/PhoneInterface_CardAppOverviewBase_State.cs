using BeneathTheNight.Data.Database;
using BeneathTheNight.Extensions;
using BeneathTheNight.InputSystem;
using BeneathTheNight.StateMachine;
using DG.Tweening;
using UnityEngine;

namespace BeneathTheNight.PhoneInterface
{
    /// <summary>
    /// This class is the base class for all app overview states that use a card system
    /// </summary>
    public abstract class PhoneInterface_CardAppOverviewBase_State : State_Base
    {
        protected PhoneInterface_CardAppOverview_InitializerData initializerDatabase;
        protected int activeCardIndex;

        private int upperCardIndex_, lowerCardIndex_;
        private bool animatorActive_;
        private Tweener cardMoverTweener_;
        
        protected PhoneInterface_CardAppOverviewBase_State(StateMachine_InitializerData initializerData,
            StateMachine_Database database) : base(initializerData, database) { }

        protected override void InitializeState()
        {
            SubscribeToUserInput();
            InitializeCards();
            lowerCardIndex_ = initializerDatabase.MaxCardPerPanel;
            delayEnterState = true;
        }

        public override void OnEnterState()
        {
            InitializeCards();
            ActivateCard(activeCardIndex);
            canSwitchState = false;
            initializerDatabase.AppOverviewPanel.SetActive(true);

            if (!animatorActive_)
            {
                initializerDatabase.PanelAnimator.gameObject.SetActive(true);
                initializerDatabase.PanelAnimator.SetTrigger(initializerDatabase.PanelOpenAnimationParameter);
                animatorActive_ = true;
            }

            CoroutineManager.Instance.RunWithDelay(DelayedEnter,
                delayEnterState ? initializerDatabase.PanelAnimationDuration : 0);

            void DelayedEnter()
            {
                canReceivePlayerInput = true;
                initializerDatabase.AppCardParent.gameObject.SetActive(true);
            }
        }

        public override void OnExitState()
        {
            canReceivePlayerInput = false;
            initializerDatabase.AppCardParent.gameObject.SetActive(false);
            stateSwitchCo = CoroutineManager.Instance.RunWithDelay(DelayedExit,
                canSwitchState ? 0 : initializerDatabase.PanelAnimationDuration);

            void DelayedExit()
            {
                canSwitchState = true;
                initializerDatabase.AppOverviewPanel.SetActive(false);
            }
        }

        protected override void SubscribeToUserInput()
        {
            InputManager.Instance.OnPhoneInterfaceInput += actionType => HandleAppNavigation(actionType, 0);
            InputManager.Instance.OnPhoneInterfaceInput += HandleCardSelectionCaller;
            InputManager.Instance.OnPhoneInterfaceInput += HandleReturning;
        }

        /// <summary>
        /// This function handles navigation in app
        /// </summary>
        /// <param name="actionType"> Type of input action </param>
        /// <param name="cardCount"> Max number of cards in the app database </param>
        protected virtual void HandleAppNavigation(InputManager.PhoneInterfaceActionType actionType, int cardCount)
        {
            bool irrelevantAction = actionType != InputManager.PhoneInterfaceActionType.NavigationUp &&
                                    actionType != InputManager.PhoneInterfaceActionType.NavigationDown;
            if (!canReceivePlayerInput || cardMoverTweener_ != null || irrelevantAction) return;

            bool goUp = actionType == InputManager.PhoneInterfaceActionType.NavigationUp;
            bool goDown = actionType == InputManager.PhoneInterfaceActionType.NavigationDown;
            bool canGoUp = activeCardIndex - 1 >= 0;
            bool canGoDown = activeCardIndex + 1 <= cardCount - 1;

            if (goUp && canGoUp)
            {
                ActivateCard(activeCardIndex - 1);
                if (activeCardIndex < upperCardIndex_)
                {
                    lowerCardIndex_--;
                    upperCardIndex_ = activeCardIndex;
                    ScrollCards(-1);
                }
            }

            if (goDown && canGoDown)
            {
                ActivateCard(activeCardIndex + 1);
                if (activeCardIndex + 1 > lowerCardIndex_)
                {
                    upperCardIndex_++;
                    lowerCardIndex_ = activeCardIndex + 1;
                    ScrollCards(1);
                }
            }

            void ScrollCards(int moveDir)
            {
                float pos = initializerDatabase.DistanceBetweenCards * moveDir;
                cardMoverTweener_ = initializerDatabase.AppCardParent
                    .DOMoveY(pos, initializerDatabase.CardScrollingSpeed).SetRelative(true)
                    .OnComplete(() => cardMoverTweener_ = null);
            }
        }
        
        /// <summary>
        /// Receives input and calls card selection function
        /// </summary>
        /// <param name="actionType"> Type of input action </param>
        private void HandleCardSelectionCaller(InputManager.PhoneInterfaceActionType actionType)
        {
            if (!canReceivePlayerInput || actionType != InputManager.PhoneInterfaceActionType.Select) return;
            HandleCardSelection();
        }

        /// <summary>
        /// Handles card selection 
        /// </summary>
        protected virtual void HandleCardSelection()
        {
            canSwitchState = true;
            delayEnterState = false;
        }

        /// <summary>
        /// Handles returning to previous state
        /// </summary>
        /// <param name="actionType"> Type of input action </param>
        private void HandleReturning(InputManager.PhoneInterfaceActionType actionType)
        {
            if (!canReceivePlayerInput || actionType != InputManager.PhoneInterfaceActionType.Return) return;
            
            initializerDatabase.PanelAnimator.SetTrigger(initializerDatabase.PanelCloseAnimationParameter);
            CoroutineManager.Instance.RunWithDelay(
                () => initializerDatabase.PanelAnimator.gameObject.SetActive(false),
                initializerDatabase.PanelAnimationDuration + 0.1f);
            animatorActive_ = false;
            delayEnterState = true;

            var newStateData = new StateMachine_StateSwitchData() {nextState = typeof(PhoneInterface_MainMenu_State)};
            stateMachine.SwitchToState(newStateData, false);
        }

        /// <summary>
        /// Initializes cards, based on state's database, called on initialize state and on enter state
        /// </summary>
        protected abstract void InitializeCards();
        
        /// <summary>
        /// Activates selected card
        /// </summary>
        /// <param name="selectedCard"> Selected card to activate </param>
        protected abstract void ActivateCard(int selectedCard);
    }
}