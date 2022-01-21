﻿using StreamChat.Core;
using StreamChat.Core.Utils;
using StreamChat.Unity.Scripts;

namespace StreamChat.Unity
{
    /// <summary>
    /// Context for view with state and common services
    /// </summary>
    public interface IChatViewContext
    {
        IStreamChatClient Client { get; }
        IImageLoader ImageLoader { get; }
        ViewFactory Factory { get; }
        IChatState State { get; }
    }
}