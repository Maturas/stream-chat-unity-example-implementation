﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetStreamChatExample.UI;
using StreamChat.Core;
using StreamChat.Core.Models;
using StreamChat.Core.Requests;
using StreamChat.Libs.Auth;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public async Task<IEnumerable<ChannelState>> GetAllChannels()
        {
            var channelResponse = await _client.ChannelApi.QueryChannelsAsync(new QueryChannelsRequest
            {
                Limit = int.MaxValue
            });
            return channelResponse?.Channels;
        }

        public async Task<IEnumerable<ChannelState>> GetChannels(SortingMode sortingMode, int currentPage, int pageSize, string searchString)
        {
            var queryChannelsRequest = new QueryChannelsRequest
            {
                Sort = new List<SortParamRequest>
                {
                    sortingMode.ToSortParamRequest()
                },
                FilterConditions = string.IsNullOrWhiteSpace(searchString) ? null : new Dictionary<string, object>
                {
                    { "name", new Dictionary<string, string> { { "$autocomplete", searchString } } }
                },
                Limit = pageSize,
                Offset = currentPage * pageSize,
                Watch = true,
                State = true
            };

            var channelResponse = await _client.ChannelApi.QueryChannelsAsync(queryChannelsRequest);
            return channelResponse?.Channels;
        }

        public async Task<int> GetChannelsCount()
        {
            // TODO Can it be done more efficiently?
            var channels = await GetAllChannels();
            return channels?.Count() ?? 0;
        }

        public async void CreateChannel(string channelName)
        {
            await _client.ChannelApi.GetOrCreateChannelAsync("livestream", channelName.Replace(" ", ""), new ChannelGetOrCreateRequest
            {
                Data = new ChannelRequest
                {
                    AdditionalProperties = new Dictionary<string, object>
                    {
                        { "name", channelName },
                        { AdditionalPropertyKeys.ChannelDescription, $"Created channel description #{Random.Range(100, 1000)}" },
                        { AdditionalPropertyKeys.ChannelMaxMemberCount, Random.Range(100, 1000) }
                    }
                }
            });
        }

        public async void DevUpdateChannels()
        {
            var channels = await GetAllChannels();

            foreach (var channelState in channels)
            {
                await _client.ChannelApi.UpdateChannelPartialAsync(channelState.Channel.Type, channelState.Channel.Id,
                    new UpdateChannelPartialRequest
                    {
                        Set = new Dictionary<string, object>
                        {
                            { AdditionalPropertyKeys.ChannelMaxMemberCount, Random.Range(100, 1000) },
                            { AdditionalPropertyKeys.ChannelDescription, $"Test description #{Random.Range(100, 1000)}" },
                        }
                    });
            }
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
