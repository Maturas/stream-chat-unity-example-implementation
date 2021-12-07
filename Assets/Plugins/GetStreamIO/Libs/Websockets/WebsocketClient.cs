﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugins.GetStreamIO.Libs.Logs;
using Plugins.GetStreamIO.Libs.Utils;

namespace Plugins.GetStreamIO.Libs.Websockets
{
    /// <summary>
    /// Implementation of <see cref="IWebsocketClient"/>
    /// </summary>
    public class WebsocketClient : IWebsocketClient
    {
        public event Action Connected;
        public event Action ConnectionFailed;
        public event Action Disconnected;

        public bool IsRunning { get; private set; }

        public WebSocketState State => _client?.State ?? WebSocketState.None;

        public WebsocketClient(ILogs logs, Encoding encoding = default)
        {
            _logs = logs ?? throw new ArgumentNullException(nameof(logs));
            _encoding = encoding ?? DefaultEncoding;
        }

        public bool TryDequeueMessage(out string message) => _receiveQueue.TryDequeue(out message);

        public async Task ConnectAsync(Uri serverUri)
        {
            _uri = serverUri ?? throw new ArgumentNullException(nameof(serverUri));

            _connectionCts?.Dispose();
            _connectionCts = new CancellationTokenSource();

            try
            {
                _client = new ClientWebSocket();
                await _client.ConnectAsync(_uri, _connectionCts.Token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logs.Exception(e);
                OnConnectionFailed();
                return;
            }

            IsRunning = true;

            Task.Factory.StartNew(SendMessages, _connectionCts.Token, TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            Task.Factory.StartNew(ReceiveMessages, _connectionCts.Token, TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            Connected?.Invoke();
        }

        private void OnConnectionFailed()
        {
            ConnectionFailed?.Invoke();
        }

        public async Task Send(string message)
        {
            var buffer = _encoding.GetBytes(message);
            var messageSegment = new ArraySegment<byte>(buffer);

            await _client
                .SendAsync(messageSegment, WebSocketMessageType.Text, true, _connectionCts.Token)
                .ConfigureAwait(false);
        }

        public void Disconnect()
        {
            _logs.Info("Disconnect");
            _connectionCts?.Dispose();
            IsRunning = false;
        }

        public void Dispose()
        {
            Disconnect();
        }

        private static Encoding DefaultEncoding { get; } = Encoding.UTF8;
        private static float ReconnectTimeout = 5;

        private readonly ConcurrentQueue<string> _receiveQueue = new ConcurrentQueue<string>();

        private readonly BlockingCollection<ArraySegment<byte>> _sendQueue =
            new BlockingCollection<ArraySegment<byte>>();

        private readonly ILogs _logs;
        private readonly Encoding _encoding;

        private Uri _uri;
        private ClientWebSocket _client;
        private CancellationTokenSource _connectionCts;

        private async Task SendMessages()
        {
            while (IsRunning)
            {
                while (!_sendQueue.IsCompleted)
                {
                    var msg = _sendQueue.Take();
                    await _client.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private async Task ReceiveMessages()
        {
            while (IsRunning)
            {
                var result = await Receive();
                if (!string.IsNullOrEmpty(result))
                {
                    _receiveQueue.Enqueue(result);
                }
                else
                {
                    Task.Delay(50).Wait();
                }
            }
        }

        private void OnDisconnected()
        {
            _logs.Info("Disconnected. Attempt to reconnect");
            ConnectAsync(_uri).ContinueWith(_ => _logs.Exception(_.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        private async Task<string> Receive()
        {
            var readBuffer = new byte[4 * 1024];
            var ms = new MemoryStream();
            var bufferSegment = new ArraySegment<byte>(readBuffer);

            if (_client.State == WebSocketState.Open)
            {
                WebSocketReceiveResult chunkResult;
                do
                {
                    chunkResult = await _client.ReceiveAsync(bufferSegment, CancellationToken.None);

                    if (chunkResult.MessageType == WebSocketMessageType.Close)
                    {
                        OnDisconnected();
                        return "";
                    }

                    ms.Write(bufferSegment.Array, bufferSegment.Offset, chunkResult.Count);
                } while (!chunkResult.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                if (chunkResult.MessageType == WebSocketMessageType.Text)
                {
                    return CommunicationUtils.StreamToString(ms, Encoding.UTF8);
                }
            }

            return "";
        }
    }
}