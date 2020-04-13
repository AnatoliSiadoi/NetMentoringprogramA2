using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement
{
    internal class PowerManagementInteroperation
    {
        internal const uint STATUS_SUCCESS = 0;

        [DllImport("PowrProf.dll", SetLastError = true,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern uint CallNtPowerInformation(
            int informaitonLevel,
            IntPtr inputBuffer,
            int inputBufferLength,
            IntPtr outputBuffer,
            int outputBufferLength);

        [DllImport("PowrProf.dll", SetLastError = true,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetSuspendState(
            bool Hibernate,
            bool Force,
            bool WakeupEventsDisabled);
    }
}
