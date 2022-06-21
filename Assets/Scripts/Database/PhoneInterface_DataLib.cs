using System;
using BeneathTheNight.PhoneInterface;
using TMPro;
using UnityEngine;

// This script is a class library for all data classes that are used by phone interface system
namespace BeneathTheNight.Data.Database
{
    /// <summary>
    /// This class holds data that is used to initialize main menu panel 
    /// </summary>
    [Serializable]
    public class PhoneInterface_MainMenu_InitializerData : StateMachine_InitializerData
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Animator unlockAnimator;
        [SerializeField, Tooltip("Duration of unlock animation")]
        private float unlockAnimationDuration;
        [SerializeField, Tooltip("Animation parameter used for unlock animation")]
        private string unlockAnimationParameter;
        [SerializeField] private Transform selectedAppBackground;
        [SerializeField] private PhoneManager.PhoneInterfaceAppNames defaultApp;
        [SerializeField] private Transform[] apps;

        public GameObject MainMenuPanel => mainMenuPanel;
        public Animator UnlockAnimator => unlockAnimator;
        public float UnlockAnimationDuration => unlockAnimationDuration;
        public string UnlockAnimationParameter => unlockAnimationParameter;
        public Transform SelectedAppBackground => selectedAppBackground;
        public PhoneManager.PhoneInterfaceAppNames DefaultApp => defaultApp;
        public Transform[] Apps => apps;
    }
    
    /// <summary>
    /// This class hold data that is used to initialize all apps that use a card system
    /// </summary>
    [Serializable]
    public class PhoneInterface_CardAppOverview_InitializerData : StateMachine_InitializerData
    {
        [SerializeField] private GameObject appOverviewPanel;
        [SerializeField] private Transform appCardParent;
        [SerializeField] private Animator panelAnimator;
        [SerializeField, Tooltip("Duration of open and close animation")]
        private float panelAnimationDuration;
        [SerializeField, Tooltip("Animation parameter used for open panel animation")]
        private string panelOpenAnimationParameter;
        [SerializeField, Tooltip("Animation parameter used for close panel animation")]
        private string panelCloseAnimationParameter;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField, Tooltip("Vertical distance between cards in app overview state")]
        private float distanceBetweenCards;
        [SerializeField, Tooltip("Max number of cards in app overview state")]
        private int maxCardPerPanel;
        [SerializeField, Tooltip("Scrolling time duration, the larger this number the slower the scrolling")]
        private float cardScrollingDuration;
        [SerializeField, Range(0, 1)] private float activeCardTransparency;
        [SerializeField, Range(0, 1)] private float inactiveCardTransparency;

        public GameObject AppOverviewPanel => appOverviewPanel;
        public Transform AppCardParent => appCardParent;
        public Animator PanelAnimator => panelAnimator;
        public float PanelAnimationDuration => panelAnimationDuration;
        public string PanelOpenAnimationParameter => panelOpenAnimationParameter;
        public string PanelCloseAnimationParameter => panelCloseAnimationParameter;
        public GameObject CardPrefab => cardPrefab;
        public float DistanceBetweenCards => distanceBetweenCards;
        public int MaxCardPerPanel => maxCardPerPanel;
        public float CardScrollingSpeed => cardScrollingDuration;
        public float ActiveCardTransparency => activeCardTransparency;
        public float InactiveCardTransparency => inactiveCardTransparency;
    }
    
    
    
    
    /// <summary>
    /// This class holds data that is used to initialize call card overview data
    /// </summary>
    [Serializable]
    public class PhoneInterface_CardOverview_Calls_InitializerData : StateMachine_InitializerData
    {
        [SerializeField] private GameObject callsCardOverviewPanel;
        [SerializeField] private SpriteRenderer callsCardPictureRenderer;
        [SerializeField] private TextMeshPro callsCardContactNameText;

        public GameObject CallsCardOverviewPanel => callsCardOverviewPanel;
        public SpriteRenderer CallsCardPictureRenderer => callsCardPictureRenderer;
        public TextMeshPro CallsCardContactNameText => callsCardContactNameText;
    }
    
    /// This class holds data that is used to initialize contacts card overview data
    [Serializable]
    public class PhoneInterface_CardOverview_Contacts_InitializerData : StateMachine_InitializerData
    {
        [SerializeField] private GameObject contactCardOverviewPanel;
        [SerializeField] private SpriteRenderer contactCardPictureRenderer;
        [SerializeField] private TextMeshPro contactCardNameText;
        [SerializeField] private TextMeshPro contactCardNumberText;
        [SerializeField] private TextMeshPro contactCardContactDescriptionText;

        public GameObject ContactCardOverviewPanel => contactCardOverviewPanel;
        public SpriteRenderer ContactCardPictureRenderer => contactCardPictureRenderer;
        public TextMeshPro ContactCardNameText => contactCardNameText;
        public TextMeshPro ContactCardNumberText => contactCardNumberText;
        public TextMeshPro ContactCardContactDescriptionText => contactCardContactDescriptionText;
    }
    
    /// This class holds data that is used to initialize messages card overview data
    [Serializable]
    public class PhoneInterface_CardOverview_Messages_InitializerData : StateMachine_InitializerData
    {
        [SerializeField] private GameObject messagesCardOverviewPanel;
        [SerializeField] private SpriteRenderer messagesCardPictureRenderer;
        [SerializeField] private TextMeshPro messagesCardContactNameText;
        [SerializeField] private TextMeshPro messagesCardFullMessageText;

        public GameObject MessagesCardOverviewPanel => messagesCardOverviewPanel;
        public SpriteRenderer MessagesCardPictureRenderer => messagesCardPictureRenderer;
        public TextMeshPro MessagesCardContactNameText => messagesCardContactNameText;
        public TextMeshPro MessagesCardFullMessageText => messagesCardFullMessageText;
    }
    
    
    
    
    /// <summary>
    /// This class is used as the base data by contacts database
    /// </summary>
    [Serializable]
    public class PhoneInterface_Contacts_BaseData
    {
        [SerializeField] private string contactName;
        [SerializeField] private string contactNumber;
        [SerializeField] private Sprite contactPicture;
        [SerializeField, TextArea] private string contactDescription;

        public string ContactName => contactName;
        public string ContactNumber => contactNumber;
        public Sprite ContactPicture => contactPicture;
        public string ContactDescription => contactDescription;
    }
    
    /// <summary>
    /// This class us used as the base data by messages database
    /// </summary>
    [Serializable]
    public class PhoneInterface_Messages_BaseDta
    {
        [SerializeField] private string contactName;
        [SerializeField] private Sprite contactPicture;
        [SerializeField] private string messagePreview;
        [SerializeField, TextArea] private string fullMessage;

        public string ContactName => contactName;
        public Sprite ContactPicture => contactPicture;
        public string MessagePreview => messagePreview;
        public string FullMessage => fullMessage;
    }
}