﻿using GetStreamIO.Core.DTO.Models;

namespace Plugins.GetStreamIO.Core.Models
{
    public partial class Device : ModelBase, ILoadableFrom<DeviceDTO, Device>
    {
        /// <summary>
        /// Date/time of creation
        /// </summary>
        public System.DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Whether device is disabled or not
        /// </summary>
        public bool? Disabled { get; set; }

        /// <summary>
        /// Reason explaining why device had been disabled
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Device ID
        /// </summary>
        public string Id { get; set; }

        public PushProviderType? PushProvider { get; set; }

        public string UserId { get; set; }


        public Device LoadFromDto(DeviceDTO dto)
        {
            CreatedAt = dto.CreatedAt;
            Disabled = dto.Disabled;
            DisabledReason = dto.DisabledReason;
            Id = dto.Id;
            PushProvider = dto.PushProvider;
            UserId = dto.UserId;
            AdditionalProperties = dto.AdditionalProperties;

            return this;
        }
    }
}