using System.Collections.Generic;
using BeneathTheNight.Data.Database;
using UnityEngine;

namespace BeneathTheNight.ScriptableObjects.PhoneInterface
{
    /// <summary>
    /// This class is the database for contacts
    /// </summary>
    [CreateAssetMenu(fileName = "Phone Interface App Contacts Database", 
        menuName = "Database/Phone Interface/Apps/Contact Database", order = 0)]
    public class PhoneInterface_App_Contacts_Database : StateMachine_Database
    {
        public List<PhoneInterface_Contacts_BaseData> database;
    }
}
