
namespace PowerStateManagement.Models
{
    public struct SystemPowerInformation
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public byte CoolingMode;
    }
}
