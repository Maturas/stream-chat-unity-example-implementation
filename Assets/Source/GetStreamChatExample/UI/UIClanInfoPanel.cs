using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GetStreamChatExample.UI
{
    public class UIClanInfoPanel : MonoBehaviour
    {
        public Image ClanIcon;
        public TextMeshProUGUI ClanName;
        public TextMeshProUGUI ClanDescription;
        public TextMeshProUGUI MembersCount;
        public TextMeshProUGUI OnlineMembersCount;

        // TODO Check if the user is the member of the clan and change the label of "Join/Leave" button accordingly

        /// <summary>
        ///     Fills in clan's information to the UI components
        /// </summary>
        /// <param name="clanIcon"></param>
        /// <param name="clanName"></param>
        /// <param name="description"></param>
        /// <param name="currentMembers"></param>
        /// <param name="maxMembers"></param>
        /// <param name="onlineMembers"></param>
        public void Init(Sprite clanIcon, string clanName, string description,  int currentMembers, string maxMembers, int onlineMembers)
        {
            // TODO Clan info reference for joining

            if (ClanIcon != null)
                ClanIcon.sprite = clanIcon;
            
            if (ClanName != null)
                ClanName.text = clanName;

            if (ClanDescription != null)
                ClanDescription.text = description;

            if (MembersCount != null)
                MembersCount.text = $"{currentMembers}/{maxMembers}";
           
            if (OnlineMembersCount != null)
                OnlineMembersCount.text = onlineMembers.ToString();

            gameObject.SetActive(true);
        }

        /// <summary>
        ///     Invoked when a Clan Info panel in the Clan Browser list is clicked, to display the Clan Info panel
        /// </summary>
        public void OnPanelClick()
        {
            // TODO
        }

        /// <summary>
        ///     Invoked when the "Join/Leave" button is clicked in the Clan Info Panel
        /// </summary>
        public void OnJoinLeaveButtonClick()
        {
            // TODO
        }
    }
}