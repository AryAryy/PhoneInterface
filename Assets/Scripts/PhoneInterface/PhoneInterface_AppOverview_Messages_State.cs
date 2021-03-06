using System;
using System.Collections.Generic;
using PhoneInterfaceCode.Database;
using PhoneInterfaceCode.InputSystem;
using PhoneInterfaceCode.ScriptableObjects.PhoneInterface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PhoneInterfaceCode.PhoneInterface
{
    /// <summary>
    /// This class handles phone interface in messages app overview state
    /// </summary>
    public class PhoneInterface_AppOverview_Messages_State : PhoneInterface_CardAppOverviewBase_State
    {
        private struct MessageCard
        {
            public MessageCardHandler cardHandler;
            public PhoneInterface_Messages_BaseDta cardData;
        }
        
        private PhoneInterface_App_Messages_Database database_;

        private List<MessageCard> cards_ = new List<MessageCard>();
        private MessageCard activeCard_;

        public PhoneInterface_AppOverview_Messages_State(PhoneInterface_CardAppOverview_InitializerData initializerData,
            PhoneInterface_App_Messages_Database database) : base(initializerData, database)
        {
            initializerDatabase = initializerData;
            database_ = database;
            InitializeState();
        }

        public override void SetDatabase(StateMachine_Database database)
        {
            if (!database) throw new Exception("State machine database is null, please pass a valid database");
            database_ = database as PhoneInterface_App_Messages_Database;
        }
        
        protected override void InitializeCards()
        {
            for (int i = 0; i < database_.database.Count; i++)
            {
                var data = database_.database[i];
                if (cards_.Count > i)
                {
                    cards_[i].cardHandler.SetupCard(data.ContactName, data.MessagePreview, data.ContactPicture,
                        initializerDatabase.InactiveCardTransparency);
                    continue;
                }
                
                Vector3 pos = initializerDatabase.AppCardParent.position;
                pos.y -= initializerDatabase.DistanceBetweenCards * i;
                var obj = Object.Instantiate(initializerDatabase.CardPrefab, pos, Quaternion.identity,
                    initializerDatabase.AppCardParent);
                obj.TryGetComponent(out MessageCardHandler handler);
                cards_.Add(new MessageCard() {cardHandler = handler, cardData = data});
                handler.SetupCard(data.ContactName, data.MessagePreview, data.ContactPicture,
                    initializerDatabase.InactiveCardTransparency);
            }
        }

        protected override void ActivateCard(int selectedCard)
        {
            if (selectedCard == activeCardIndex)
            {
                cards_[selectedCard].cardHandler.ActivateCard(initializerDatabase.ActiveCardTransparency);
                activeCard_ = cards_[selectedCard];
                return;
            }
            
            cards_[activeCardIndex].cardHandler.DeactivateCard(initializerDatabase.InactiveCardTransparency);
            cards_[selectedCard].cardHandler.ActivateCard(initializerDatabase.ActiveCardTransparency);
            activeCard_ = cards_[selectedCard];
            activeCardIndex = selectedCard;
        }

        protected override void HandleAppNavigation(InputManager.PhoneInterfaceActionType actionType, int cardCount) =>
            base.HandleAppNavigation(actionType, cards_.Count);

        protected override void HandleCardSelection()
        {
            base.HandleCardSelection();
            var data = ScriptableObject.CreateInstance<PhoneInterface_App_Messages_Database>();
            data.database = new List<PhoneInterface_Messages_BaseDta>() {activeCard_.cardData};
            var newStateData = new StateMachine_StateSwitchData()
                {nextState = typeof(PhoneInterface_CardOverview_Messages_State), nextStateDatabase = data};
            stateMachine.SwitchToState(newStateData, true);
            Object.Destroy(data);
        }
    }
}