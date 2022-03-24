﻿using System;
using System.Collections;
using System.Threading.Tasks;
using StreamChat.Core;
using UnityEditor;
using UnityEngine;

namespace StreamChat.Tests
{
    /// <summary>
    /// Utils for testing purposes
    /// </summary>
    public static class UnityTestUtils
    {
        public static IEnumerator RunAsIEnumerator<TResponse>(this Task<TResponse> task, Action<TResponse> action)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw task.Exception;
            }

            action(task.Result);
        }

        public static IEnumerator WaitForClientToConnect(this IStreamChatClient client)
        {
            const float MaxTimeToConnect = 3;
            var timeStarted = EditorApplication.timeSinceStartup;

            while (true)
            {
                var elapsed = EditorApplication.timeSinceStartup - timeStarted;

                if (elapsed > MaxTimeToConnect)
                {
                    Debug.LogError("Waiting for connection exceeded max time. Terminating");
                    break;
                }

                client.Update(0.1f);

                if (client.ConnectionState == ConnectionState.Connecting)
                {
                    yield return null;
                }

                if (client.ConnectionState == ConnectionState.Connected)
                {
                    break;
                }

                if (client.ConnectionState == ConnectionState.Disconnected)
                {
                    Debug.LogError("Client disconnected when waiting for connection. Terminating");
                    break;
                }
            }
        }
    }
}