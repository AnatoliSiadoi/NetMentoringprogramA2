using System.Runtime.InteropServices;

namespace PowerStateManagement.Models
{
    [ComVisible(true)]
    [Guid("D5B742E7-E296-47E4-87A7-E7B03FDDFC2A")]
    public struct SystemBatteryState
    {
        public bool AcOnLine;
        public bool BatteryPresent;
        public bool Charging;
        public bool Discharging;
        public byte spare1;
        public byte spare2;
        public byte spare3;
        public uint MaxCapacity;
        public uint RemainingCapacity;
        public uint Rate;
        public uint EstimatedTime;
        public uint DefaultAlert1;
        public uint DefaultAlert2;
    }
}
