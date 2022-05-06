using System.Collections.Generic;
using GetStreamChatExample.Logic;
using UnityEngine;

namespace GetStreamChatExample.UI
{
    public class UIClanBrowser : MonoBehaviour
    {
        private const int PageSize = 10;

        public UIClanInfoPanel ClanInfoPanelTemplate;
        public RectTransform ClanListPanel;

        private StreamChatBehaviour _chatBehaviour;

        private int _currentPage;
        private int _maxPage;

        private SortingMode _lastUsedSortingMode;

        private List<UIClanInfoPanel> _clanInfoPanels;

        public void Init(StreamChatBehaviour chatBehaviour)
        {
            _chatBehaviour = chatBehaviour;
            _currentPage = 0;
            _lastUsedSortingMode = SortingMode.NameAscending;
            _clanInfoPanels = new List<UIClanInfoPanel>();

            // TODO Get max amount of pages
            // TODO Query channels
            RefreshChannels();
        }

        private async void RefreshChannels()
        {
            var channels = await _chatBehaviour.GetChannels(_lastUsedSortingMode, _currentPage, PageSize);

            // TODO Object pool
            foreach (var clanInfoPanel in _clanInfoPanels)
            {
                Destroy(clanInfoPanel.gameObject);
            }
            _clanInfoPanels.Clear();

            foreach (var channel in channels)
            {
                var clanInfoPanel = Instantiate(ClanInfoPanelTemplate.gameObject, ClanListPanel).GetComponent<UIClanInfoPanel>();
                clanInfoPanel.Init(null, channel.Name, null, channel.MemberCount ?? 0, 0, 0);

                _clanInfoPanels.Add(clanInfoPanel);
            }
        }

        /// <summary>
        ///     Invoked when the "Search by name" input field's value is changed
        /// </summary>
        /// <param name="value"></param>
        public void OnSearchFieldChange(string value)
        {
            // TODO

            RefreshChannels();
        }

        /// <summary>
        ///     Invoked when "Sort by Names" button is clicked
        /// </summary>
        public void OnClickSortByNames()
        {
            _lastUsedSortingMode =
                _lastUsedSortingMode == SortingMode.NameAscending 
                    ? SortingMode.NameDescending : SortingMode.NameAscending;

            RefreshChannels();
        }

        /// <summary>
        ///     Invoked when "Sort by Creation Date" button is clicked
        /// </summary>
        public void OnClickSortByCreationDate()
        {
            _lastUsedSortingMode =
                _lastUsedSortingMode == SortingMode.CreationDateAscending
                    ? SortingMode.CreationDateDescending : SortingMode.CreationDateAscending;

            RefreshChannels();
        }

        /// <summary>
        ///     Invoked when "Sort by Members Count" button is clicked
        /// </summary>
        public void OnClickSortByMembersCount()
        {
            _lastUsedSortingMode =
                _lastUsedSortingMode == SortingMode.MembersCountAscending
                    ? SortingMode.MembersCountDescending : SortingMode.MembersCountAscending;

            RefreshChannels();
        }

        /// <summary>
        ///     Invoked when "Previous Page" button is clicked
        /// </summary>
        public void OnClickPreviousPage()
        {
            if (_currentPage == 0)
                return;

            _currentPage--;

            RefreshChannels();
        }

        /// <summary>
        ///     Invoked when "Next Page" button is clicked
        /// </summary>
        public void OnClickNextPage()
        {
            if (_currentPage == _maxPage)
                return;

            _currentPage++;

            RefreshChannels();
        }

        /// <summary>
        ///     Invoked when "Create a Clan" button is clicked
        /// </summary>
        public void OnClickCreateClan()
        {
            // TODO
        }
    }
}
