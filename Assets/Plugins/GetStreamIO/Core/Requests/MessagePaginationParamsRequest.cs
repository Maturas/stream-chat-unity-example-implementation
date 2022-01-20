﻿using GetStreamIO.Core.DTO.Requests;

namespace Plugins.GetStreamIO.Core.Requests
{
    public partial class MessagePaginationParamsRequest : RequestObjectBase, ISavableTo<MessagePaginationParamsRequestDTO>
    {
        public System.DateTimeOffset? CreatedAtAfter { get; set; }

        public System.DateTimeOffset? CreatedAtAfterOrEqual { get; set; }

        public System.DateTimeOffset? CreatedAtAround { get; set; }

        public System.DateTimeOffset? CreatedAtBefore { get; set; }

        public System.DateTimeOffset? CreatedAtBeforeOrEqual { get; set; }

        public string IdAround { get; set; }

        public string IdGt { get; set; }

        public string IdGte { get; set; }

        public string IdLt { get; set; }

        public string IdLte { get; set; }

        public double? Limit { get; set; }

        public double? Offset { get; set; }

        public MessagePaginationParamsRequestDTO SaveToDto() =>
            new MessagePaginationParamsRequestDTO
            {
                AdditionalProperties = AdditionalProperties,
                CreatedAtAfter = CreatedAtAfter,
                CreatedAtAfterOrEqual = CreatedAtAfterOrEqual,
                CreatedAtAround = CreatedAtAround,
                CreatedAtBefore = CreatedAtBefore,
                CreatedAtBeforeOrEqual = CreatedAtBeforeOrEqual,
                IdAround = IdAround,
                IdGt = IdGt,
                IdGte = IdGte,
                IdLt = IdLt,
                IdLte = IdLte,
                Limit = Limit,
                Offset = Offset,
            };
    }
}