﻿using StreamChat.Core;
using StreamChat.Core.DTO.Responses;
using StreamChat.Core.Responses;

namespace StreamChat.Core.Responses
{
    public partial class DeleteUsersResponse : ResponseObjectBase, ILoadableFrom<DeleteUsersResponseDTO, DeleteUsersResponse>
    {
        /// <summary>
        /// Duration of the request in human-readable format
        /// </summary>
        public string Duration { get; set; }

        public string TaskId { get; set; }

        public DeleteUsersResponse LoadFromDto(DeleteUsersResponseDTO dto)
        {
            Duration = dto.Duration;
            TaskId = dto.TaskId;
            AdditionalProperties = dto.AdditionalProperties;

            return this;
        }
    }
}