﻿using StreamChat.Core;
using StreamChat.Core.DTO.Responses;
using StreamChat.Core.Helpers;
using StreamChat.Core.Models;
using StreamChat.Core.Responses;

namespace StreamChat.Core.Responses
{
    public partial class UsersResponse : ResponseObjectBase, ILoadableFrom<UsersResponseDTO, UsersResponse>
    {
        /// <summary>
        /// Duration of the request in human-readable format
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// List of found users
        /// </summary>
        public System.Collections.Generic.ICollection<User> Users { get; set; }

        public UsersResponse LoadFromDto(UsersResponseDTO dto)
        {
            Duration = dto.Duration;
            Users = Users.TryLoadFromDtoCollection<UserResponseDTO, User>(dto.Users);
            AdditionalProperties = dto.AdditionalProperties;

            return this;
        }
    }
}