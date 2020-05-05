using PowerStateManagement.Interfaces;
using PowerStateManagement.Models;
using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement.COM
{
    [ComVisible(true)]
    [Guid("8CDDE3FE-955D-4A16-9C34-478543E89DFE")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PowerStateManagerCOM : IPowerStateManager
    {
        private readonly PowerStateManager _powerStateManager; 

        public PowerStateManagerCOM()
        {
            _powerStateManager = new PowerStateManager();
        }

        public DateTime GetLastSleepTime()
        {
            return _powerStateManager.GetLastSleepTime();
        }

        public DateTime GetLastWakeTime()
        {
            return _powerStateManager.GetLastWakeTime();
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            return _powerStateManager.GetSystemBatteryState();
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            return _powerStateManager.GetSystemPowerInformation();
        }

        public void SetSuspendState(bool hibernate, bool force, bool wakeupEventsDisabled)
        {
            _powerStateManager.SetSuspendState(hibernate, force, wakeupEventsDisabled);
        }
    }
}
