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
            Console.WriteLine(lastSleepTime);

            DateTime lastWakeTime = powerStateManager.GetLastWakeTime();
            Console.WriteLine(lastWakeTime);

            var systemBatteryState = powerStateManager.GetSystemBatteryState();
            //Console.WriteLine(systemBatteryState);

            var systemPowerInformation = powerStateManager.GetSystemPowerInformation();
            //Console.WriteLine(systemPowerInformation);

            //hibernateFileManager.Reserve();
            //hibernateFileManager.Delete();

            Console.Read();
        }
    }
}
