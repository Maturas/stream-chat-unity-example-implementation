using System;
using StreamChat.Core.Requests;

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
            SortingMode.MembersCountAscending => new SortParamRequest { Field = "members_count", Direction = 1 },
            SortingMode.MembersCountDescending => new SortParamRequest { Field = "members_count", Direction = -1 },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
