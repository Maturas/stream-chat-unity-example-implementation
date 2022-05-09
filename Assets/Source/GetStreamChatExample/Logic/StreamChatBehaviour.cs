using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetStreamChatExample.UI;
using StreamChat.Core;
using StreamChat.Core.Models;
using StreamChat.Core.Requests;
using StreamChat.Libs.Auth;
using UnityEngine;

namespace GetStreamChatExample.Logic
{
    public class StreamChatBehaviour : MonoBehaviour
    {
        public AuthCredentialsAsset Credentials;
        public UIClanBrowser ClanBrowser;

        private IStreamChatClient _client;

        private void Awake()
        {
            try
            {
                _client = StreamChatClient.CreateDefaultClient(Credentials.Credentials);
                _client.Connect();
                _client.Connected += OnClientConnected;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async Task<IEnumerable<Channel>> GetChannels(SortingMode sortingMode, int currentPage, int pageSize)
        {
            var queryChannelsRequest = new QueryChannelsRequest
            {
                Sort = new List<SortParamRequest>
                {
                    sortingMode.ToSortParamRequest()
                },
                Limit = pageSize,
                Offset = currentPage * pageSize,
                Watch = true,
                State = true
            };

            var channelResponse = await _client.ChannelApi.QueryChannelsAsync(queryChannelsRequest);
            return channelResponse?.Channels?.Select(cs => cs.Channel);
        }

        public async Task<int> GetChannelsCount()
        {
            // TODO Can it be done more efficiently?
            var channelResponse = await _client.ChannelApi.QueryChannelsAsync(new QueryChannelsRequest());
            return channelResponse?.Channels?.Count ?? 0;
        }

        private void Update()
        {
            _client.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _client.Dispose();
        }

        private void OnClientConnected()
        {
            ClanBrowser.Init(this);
        }
    }
}
