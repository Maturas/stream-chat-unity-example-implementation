using System.Threading.Tasks;
using UnityEngine;

namespace Plugins.GetStreamIO.Libs.Utils
{
    public static class ThreadUtils
    {
        public static void LogIfFailed(this Task t) => t.ContinueWith(_ => Debug.LogException(_.Exception.Flatten()),
            TaskContinuationOptions.OnlyOnFaulted);
    }
}