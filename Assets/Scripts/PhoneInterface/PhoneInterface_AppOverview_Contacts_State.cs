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
    /// This class handles phone interface in contacts app overview state
    /// </summary>
    public class PhoneInterface_AppOverview_Contacts_State : PhoneInterface_CardAppOverviewBase_State
    {
        private struct ContactCard
        {
            public ContactCardHandler cardHandler;
            public PhoneInterface_Contacts_BaseData cardData;
        }
        
        private PhoneInterface_App_Contacts_Database database_;

        private List<ContactCard> cards_ = new List<ContactCard>();
        private ContactCard activeCard_;

        public PhoneInterface_AppOverview_Contacts_State(PhoneInterface_CardAppOverview_InitializerData initializerData,
            PhoneInterface_App_Contacts_Database database) : base(initializerData, database)
        {
            initializerDatabase = initializerData;
            database_ = database;
            InitializeState();
        }

        public override void SetDatabase(StateMachine_Database database)
        {
            if (!database) throw new Exception("State machine database is null, please pass a valid database");
            database_ = database as PhoneInterface_App_Contacts_Database;
        }

        protected override void InitializeCards()
        {
            for (int i = 0; i < database_.database.Count; i++)
            {
                var data = database_.database[i];
                if (cards_.Count > i)
                {
                    cards_[i].cardHandler.SetupCard(data.ContactName, data.ContactNumber, data.ContactPicture,
                        initializerDatabase.InactiveCardTransparency);
                    continue;
                }
                
                Vector3 pos = initializerDatabase.AppCardParent.position;
                pos.y -= initializerDatabase.DistanceBetweenCards * i;
                var obj = Object.Instantiate(initializerDatabase.CardPrefab, pos, Quaternion.identity,
                    initializerDatabase.AppCardParent);
                obj.TryGetComponent(out ContactCardHandler handler);
                cards_.Add(new ContactCard() {cardHandler = handler, cardData = data});
                handler.SetupCard(data.ContactName, data.ContactNumber, data.ContactPicture,
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
            var data = ScriptableObject.CreateInstance<PhoneInterface_App_Contacts_Database>();
            data.database = new List<PhoneInterface_Contacts_BaseData>() {activeCard_.cardData};
            var newStateData = new StateMachine_StateSwitchData()
                {nextState = typeof(PhoneInterface_CardOverview_Contacts_State), nextStateDatabase = data};
            stateMachine.SwitchToState(newStateData, true);
            Object.Destroy(data);
        }
    }
}