using System;
using StreamChat.Core.Models;
using StreamChat.Core.Requests;
using UnityEngine;

namespace GetStreamChatExample
{
    public static class Utilities
    {
        public static SortParamRequest ToSortParamRequest(this SortingMode mode) => mode switch
        {
            SortingMode.NameAscending => new SortParamRequest { Field = "name", Direction = 1 },
            SortingMode.NameDescending => new SortParamRequest { Field = "name", Direction = -1 },
            SortingMode.CreationDateAscending => new SortParamRequest { Field = "created_at", Direction = 1 },
            SortingMode.CreationDateDescending => new SortParamRequest { Field = "created_at", Direction = -1 },
            SortingMode.MembersCountAscending => new SortParamRequest { Field = "member_count", Direction = 1 },
            SortingMode.MembersCountDescending => new SortParamRequest { Field = "member_count", Direction = -1 },
            _ => throw new ArgumentOutOfRangeException()
        };

        public static T GetAdditionalProperty<T>(this Channel channel, string key)
        {
            try
            {
                if (channel.AdditionalProperties.TryGetValue(key, out var obj))
                {
                    return (T) obj;
                }
                else
                {
                    return default;
                }
            }
            catch (InvalidCastException e)
            {
                Debug.LogError(e.Message);
                throw;
            }
        }
    }
}
