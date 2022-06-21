using UnityEngine;

namespace PhoneInterfaceCode.PhoneInterface
{
    /// <summary>
    /// This class holds app type for each app icon, should be attached to all app icons
    /// </summary>
    public class AppIdentifier : MonoBehaviour
    {
        [SerializeField] private PhoneManager.PhoneInterfaceAppNames appType;
        public PhoneManager.PhoneInterfaceAppNames AppType => appType;
    }
}