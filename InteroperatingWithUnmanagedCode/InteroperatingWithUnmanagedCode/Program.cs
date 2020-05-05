using PowerStateManagement;
using System;

namespace InteroperatingWithUnmanagedCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var powerStateManager = new PowerStateManager();
            var hibernateFileManager = new HibernateFileManager();

            var lastSleepTime = powerStateManager.GetLastSleepTime();
            Console.WriteLine($"Last sleep time : {lastSleepTime}\n");

            DateTime lastWakeTime = powerStateManager.GetLastWakeTime();
            Console.WriteLine($"Last wake time : {lastWakeTime}\n");

            var systemBatteryState = powerStateManager.GetSystemBatteryState();
            Console.WriteLine($"System battery state :\n" +
                $"AcOnLine : {systemBatteryState.AcOnLine}\n" +
                $"BatteryPresent : {systemBatteryState.BatteryPresent}\n" +
                $"Charging : {systemBatteryState.Charging}\n" +
                $"Discharging : {systemBatteryState.Discharging}\n" +
                $"MaxCapacity : {systemBatteryState.MaxCapacity}\n" +
                $"RemainingCapacity : {systemBatteryState.RemainingCapacity}\n" +
                $"Rate : {systemBatteryState.Rate}\n" +
                $"EstimatedTime {systemBatteryState.EstimatedTime}\n");

            var systemPowerInformation = powerStateManager.GetSystemPowerInformation();
            Console.WriteLine($"System power information :\n" +
                $"MaxIdlenessAllowed : {systemPowerInformation.MaxIdlenessAllowed}\n" +
                $"Idleness : {systemPowerInformation.Idleness}\n" +
                $"TimeRemaining : {systemPowerInformation.TimeRemaining}\n" +
                $"CoolingMode : {systemPowerInformation.CoolingMode}\n");

            //hibernateFileManager.Reserve();
            //hibernateFileManager.Delete();

            Console.Read();
        }
    }
}
