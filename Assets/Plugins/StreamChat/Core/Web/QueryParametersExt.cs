﻿namespace StreamChat.Core.Web
{
    public static class QueryParametersExt
    {
        public static QueryParameters Append(this QueryParameters queryParameters, string key, bool value)
            => Append(queryParameters, key, value.ToString());

        public static QueryParameters Append(this QueryParameters queryParameters, string key, string value)
        {
            queryParameters[key] = value;
            return queryParameters;
        }
    }
}