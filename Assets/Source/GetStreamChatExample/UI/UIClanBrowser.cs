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

            RefreshChannels();
        }

        public void OnDevUpdateChannelsButton()
        {
            _chatBehaviour.DevUpdateChannels();
        }

        private async void RefreshChannels(string searchString = null)
        {
            var channelsCount = await _chatBehaviour.GetChannelsCount();
            _maxPage = Mathf.CeilToInt((float) channelsCount / PageSize);

            var channelStates = await _chatBehaviour.GetChannels(_lastUsedSortingMode, _currentPage, PageSize, searchString);

            // TODO Object pool
            foreach (var clanInfoPanel in _clanInfoPanels)
            {
                Destroy(clanInfoPanel.gameObject);
            }
            _clanInfoPanels.Clear();

            foreach (var channelState in channelStates)
            {
                // TODO Icon
                var clanInfoPanel = Instantiate(ClanInfoPanelTemplate.gameObject, ClanListPanel).GetComponent<UIClanInfoPanel>();

                var channelName = channelState.Channel.Name;
                var description = channelState.Channel.GetAdditionalProperty<string>(AdditionalPropertyKeys.ChannelDescription);
                var memberCount = channelState.Channel.MemberCount ?? 0;
                var maxMemberCount = channelState.Channel.GetAdditionalProperty<object>(AdditionalPropertyKeys.ChannelMaxMemberCount).ToString(); // Workaround TODO It should be an int
                var onlineMemberCount = channelState.WatcherCount ?? 0;

                clanInfoPanel.Init(null, channelName, description, memberCount, maxMemberCount, onlineMemberCount);

                _clanInfoPanels.Add(clanInfoPanel);
            }
        }

        /// <summary>
        ///     Invoked when the "Search by name" input field's value is changed
        /// </summary>
        /// <param name="value"></param>
        public void OnSearchFieldChange(string value)
        {
            RefreshChannels(value);
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
