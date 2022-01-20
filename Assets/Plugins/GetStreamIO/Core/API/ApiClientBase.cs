﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetStreamIO.Core.DTO.Models;
using Plugins.GetStreamIO.Core.Exceptions;
using Plugins.GetStreamIO.Core.Web;
using Plugins.GetStreamIO.Libs.Http;
using Plugins.GetStreamIO.Libs.Logs;
using Plugins.GetStreamIO.Libs.Serialization;

namespace Plugins.GetStreamIO.Core.API
{
    /// <summary>
    /// Base Api client
    /// </summary>
    public abstract class ApiClientBase
    {
        protected ApiClientBase(IHttpClient httpClient, ISerializer serializer, ILogs logs,
            IRequestUriFactory requestUriFactory)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logs = logs ?? throw new ArgumentNullException(nameof(logs));
            _requestUriFactory = requestUriFactory ?? throw new ArgumentNullException(nameof(requestUriFactory));
        }

        protected async Task<TResponse> Get<TResponse, TResponseDto>(string url)
            where TResponse : ILoadableFrom<TResponseDto, TResponse>, new()
        {
            var uri = _requestUriFactory.CreateEndpointUri(url);

            var httpResponse = await _httpClient.GetAsync(uri);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var apiError = _serializer.Deserialize<APIErrorDTO>(responseContent);
                throw new StreamApiException(apiError);
            }

            var responseDto = _serializer.Deserialize<TResponseDto>(responseContent);
            var response = new TResponse();
            response.LoadFromDto(responseDto);

            return response;
        }

        protected async Task<TResponse> Post<TRequest, TRequestDto, TResponse, TResponseDto>(string url,
            TRequest request)
            where TRequest : ISavableTo<TRequestDto>
            where TResponse : ILoadableFrom<TResponseDto, TResponse>, new()
        {
            var uri = _requestUriFactory.CreateEndpointUri(url);
            var requestContent = _serializer.Serialize(request.SaveToDto());

            var httpResponse = await _httpClient.PostAsync(uri, requestContent);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var apiError = _serializer.Deserialize<APIErrorDTO>(responseContent);
                throw new StreamApiException(apiError);
            }

            TResponseDto responseDto;

            try
            {
                responseDto = _serializer.Deserialize<TResponseDto>(responseContent);
            }
            catch (Exception e)
            {
                throw new StreamDeserializationException(requestContent, typeof(TResponseDto), e);
            }

            LogRestCall(uri, requestContent, responseContent);

            var response = new TResponse();
            response.LoadFromDto(responseDto);

            return response;
        }

        //Todo: refactor methods to remove duplication
        //Probably best to use HttpClient.SendAsync only with optional content instead specialized methods that share common logic
        protected async Task<TResponse> Patch<TRequest, TRequestDto, TResponse, TResponseDto>(string url,
            TRequest request)
            where TRequest : ISavableTo<TRequestDto>
            where TResponse : ILoadableFrom<TResponseDto, TResponse>, new()
        {
            var uri = _requestUriFactory.CreateEndpointUri(url);
            var requestContent = _serializer.Serialize(request.SaveToDto());

            var httpResponse = await _httpClient.PatchAsync(uri, requestContent);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var apiError = _serializer.Deserialize<APIErrorDTO>(responseContent);
                throw new StreamApiException(apiError);
            }

            TResponseDto responseDto;

            try
            {
                responseDto = _serializer.Deserialize<TResponseDto>(responseContent);
            }
            catch (Exception e)
            {
                throw new StreamDeserializationException(requestContent, typeof(TResponseDto), e);
            }

            LogRestCall(uri, requestContent, responseContent);

            var response = new TResponse();
            response.LoadFromDto(responseDto);

            return response;
        }

        protected async Task<TResponse> Delete<TResponse, TResponseDto>(string endpoint,
            Dictionary<string, string> parameters = null)
            where TResponse : ILoadableFrom<TResponseDto, TResponse>, new()
        {
            var uri = _requestUriFactory.CreateEndpointUri(endpoint, parameters);

            var httpResponse = await _httpClient.DeleteAsync(uri);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                var apiError = _serializer.Deserialize<APIErrorDTO>(responseContent);
                throw new StreamApiException(apiError);
            }

            var responseDto = _serializer.Deserialize<TResponseDto>(responseContent);
            var response = new TResponse();
            response.LoadFromDto(responseDto);

            return response;
        }

        protected void LogRestCall(Uri uri, string request, string response)
            => _logs.Info($"REST API Call: {uri}\n\nRequest:\n{request}\n\nResponse:\n{response}\n\n\n");

        protected void LogRestCall(Uri uri, string response)
            => _logs.Info($"REST API Call: {uri}\n\nResponse:\n{response}\n\n\n");

        private readonly IHttpClient _httpClient;
        private readonly ISerializer _serializer;
        private readonly ILogs _logs;
        private readonly IRequestUriFactory _requestUriFactory;
    }
}