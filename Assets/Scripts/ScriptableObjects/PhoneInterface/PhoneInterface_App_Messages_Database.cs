using System.Collections.Generic;
using PhoneInterfaceCode.Database;
using UnityEngine;

namespace PhoneInterfaceCode.ScriptableObjects.PhoneInterface
{
    /// <summary>
    /// This class is the database for messages
    /// </summary>
    [CreateAssetMenu(fileName = "Phone Interface App Messages Database", 
        menuName = "Database/Phone Interface/Apps/Messages Database", order = 0)]
    public class PhoneInterface_App_Messages_Database : StateMachine_Database
    {
        public List<PhoneInterface_Messages_BaseDta> database;
    }
}