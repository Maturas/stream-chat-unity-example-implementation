﻿using System;
using System.Collections.Generic;
using StreamChat.Core.Models;

namespace StreamChat.Unity
{
    /// <summary>
    /// Keep chat state
    /// </summary>
    public interface IChatState : IDisposable
    {
        event Action<ChannelState> ActiveChanelChanged;
        event Action ChannelsUpdated;

        ChannelState ActiveChannelDeprecated { get; }
        IReadOnlyList<ChannelState> Channels { get; }


        void OpenChannel(ChannelState channel);

        void EditMessage(Message message);

        event Action<Message> MessageEditRequested;
    }
}