using PowerStateManagement.Models;
using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement.Interfaces
{
    [ComVisible(true)]
    [Guid("1965833D-D1EE-405A-92CF-338589FA6269")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerStateManager
    {
        DateTime GetLastSleepTime();

        DateTime GetLastWakeTime();

        SystemBatteryState GetSystemBatteryState();

        SystemPowerInformation GetSystemPowerInformation();

        void SetSuspendState(bool hibernate, bool force, bool wakeupEventsDisabled);
    }
}
