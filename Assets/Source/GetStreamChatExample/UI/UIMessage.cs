using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetStreamChatExample.UI
{
    public class UIMessage : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public RectTransform AdministrationPanel;
        public Button BackButton;
        public Button EditButton;
        public Button DeleteButton;

        /// <summary>
        ///     Fills-in text component with the provided message information
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userRole"></param>
        /// <param name="minsAgo"></param>
        /// <param name="message"></param>
        public void Init(string userName, string userRole, int minsAgo, string message)
        {
            // TODO Get message reference for administration purposes

            Text.text = $"{userName} [{userRole}], {minsAgo} mins ago\n{message}";
        }

        /// <summary>
        ///     Invoked after clicking on the message panel, displays the administration buttons panel (depending on the user's permissions)
        /// </summary>
        public void OnPanelClick()
        {
            // TODO Check user's permissions
            AdministrationPanel.gameObject.SetActive(true);
            BackButton.gameObject.SetActive(true);
            EditButton.gameObject.SetActive(true);
            DeleteButton.gameObject.SetActive(true);
        }

        /// <summary>
        ///     Invoked after clicking the "Back" button in the administration panel
        /// </summary>
        public void OnButtonBackClick()
        {
            AdministrationPanel.gameObject.SetActive(false);
        }

        /// <summary>
        ///     Invoked after clicking the "Edit" button in the administration panel
        /// </summary>
        public void OnEditButtonClick()
        {
            // TODO
        }

        /// <summary>
        ///     Invoked after clicking "Delete" button in the administration panel
        /// </summary>
        public void OnDeleteButtonClick()
        {
            // TODO
        }
    }
}
