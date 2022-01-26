﻿using StreamChat.Core;
using StreamChat.Core.DTO.Requests;
using StreamChat.Core.Helpers;
using StreamChat.Core.Requests;

namespace Plugins.StreamChat.Core.Requests
{
    public partial class GuestRequest : RequestObjectBase, ISavableTo<GuestRequestDTO>
    {
        public UserObjectRequest User { get; set; } = new UserObjectRequest();

        public GuestRequestDTO SaveToDto() =>
            new GuestRequestDTO
            {
                User = User.TrySaveToDto(),
                AdditionalProperties = AdditionalProperties,
            };
    }
}