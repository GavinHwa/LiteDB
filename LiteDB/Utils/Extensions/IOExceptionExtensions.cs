﻿using System.IO;
using System.Runtime.InteropServices;

namespace LiteDB
{
    internal static class IOExceptionExtensions
    {
        private const int ERROR_SHARING_VIOLATION = 32;
        private const int ERROR_LOCK_VIOLATION = 33;

        public static void WaitIfLocked(this IOException ex, int timer)
        {
            var errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
            if (errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION)
            {
                if (timer > 0)
                {
                    WaitFor(250);
                }
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// WaitFor function used in NETFULL + PCL
        /// </summary>
        public static void WaitFor(int ms)
        {
            // http://stackoverflow.com/questions/12641223/thread-sleep-replacement-in-net-for-windows-store
#if NET35
            System.Threading.Thread.Sleep(250);
#else
            System.Threading.Tasks.Task.Delay(250).Wait();
#endif
        }
    }
}