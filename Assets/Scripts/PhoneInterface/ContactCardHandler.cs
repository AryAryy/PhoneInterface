using UnityEngine;
using UnityEngine.UI;

namespace PhoneInterfaceCode.PhoneInterface
{
    /// <summary>
    /// This class manages each card that is used by phone interface contacts app
    /// </summary>
    public class ContactCardHandler : MonoBehaviour
    {
        [SerializeField] private Text contactName;
        [SerializeField] private Text contactNumber;
        [SerializeField] private SpriteRenderer backgroundRenderer;
        [SerializeField] private SpriteRenderer contactPictureRenderer;

        /// <summary>
        /// Sets up the card initial data
        /// </summary>
        /// <param name="cName"> Contact's name </param>
        /// <param name="cNumber"> Contact's number </param>
        /// <param name="cPicture"> Contact's picture </param>
        /// <param name="backgroundTransparency"> Transparency level of card's background </param>
        public void SetupCard(string cName, string cNumber, Sprite cPicture, float backgroundTransparency)
        {
            contactName.text = cName;
            contactNumber.text = cNumber;
            SetBackgroundTransparency(backgroundTransparency);
            contactPictureRenderer.sprite = cPicture;
        }

        /// <summary>
        /// Activated card
        /// </summary>
        /// <param name="backgroundTransparency"> Background transparency while card is active </param>
        public void ActivateCard(float backgroundTransparency) => SetBackgroundTransparency(backgroundTransparency);

        /// <summary>
        /// Deactivates card
        /// </summary>
        /// <param name="backgroundTransparency"> Background transparency while card is inactive </param>
        public void DeactivateCard(float backgroundTransparency) => SetBackgroundTransparency(backgroundTransparency);

        /// <summary>
        /// Sets card's background transparency
        /// </summary>
        /// <param name="transparency"> Card's background transparency </param>
        private void SetBackgroundTransparency(float transparency)
        {
            var color = backgroundRenderer.color;
            color.a = transparency;
            backgroundRenderer.color = color;
        }
    }
}