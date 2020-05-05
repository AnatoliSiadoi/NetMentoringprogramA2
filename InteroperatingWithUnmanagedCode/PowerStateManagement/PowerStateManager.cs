using PowerStateManagement.Enums;
using PowerStateManagement.Models;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PowerStateManagement
{
    public class PowerStateManager
    {
        public DateTime GetLastSleepTime()
        {
            long lastSleepTimeTicks = GetPowerInformation<long>(PowerInformationLevel.LastSleepTime);

            return new DateTime(lastSleepTimeTicks);
        }

        public DateTime GetLastWakeTime()
        {
            long lastWakeTimeTicks = GetPowerInformation<long>(PowerInformationLevel.LastWakeTime);

            return new DateTime(lastWakeTimeTicks);
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            var systemBatteryState = GetPowerInformation<SystemBatteryState>(PowerInformationLevel.SystemBatteryState);

            return systemBatteryState;
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            var systemPowerInformation = GetPowerInformation<SystemPowerInformation>(PowerInformationLevel.SystemPowerInformation);

            return systemPowerInformation;
        }

        public void SetSuspendState(bool hibernate, bool force, bool wakeupEventsDisabled)
        {
            var result = PowerManagementInteroperation.SetSuspendState(hibernate, force, wakeupEventsDisabled);

            if (!result)
            {
                throw new Win32Exception();
            }
        }

        private T GetPowerInformation<T>(PowerInformationLevel informaitonLevel)
        {
            IntPtr inputBuffer = IntPtr.Zero;
            int inputBufferLength = 0;
            int outputBufferLength = Marshal.SizeOf<T>();
            IntPtr outputBuffer = Marshal.AllocCoTaskMem(outputBufferLength);

            var retval = PowerManagementInteroperation.CallNtPowerInformation(
                (int)informaitonLevel,
                inputBuffer,
                inputBufferLength,
                outputBuffer,
                outputBufferLength);

            Marshal.FreeHGlobal(inputBuffer);
            if (retval == PowerManagementInteroperation.STATUS_SUCCESS)
            {
                var obj = Marshal.PtrToStructure<T>(outputBuffer);
                Marshal.FreeHGlobal(outputBuffer);
                return obj;
            }
            else
            {
                throw new Win32Exception();
            }
        }
    }
}
