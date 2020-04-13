using System.Runtime.InteropServices;

namespace PowerStateManagement.Models
{
    [ComVisible(true)]
    [Guid("9560E6D2-1DF0-47E0-B9F9-516A989E2238")]
    public struct SystemPowerInformation
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public byte CoolingMode;
    }
}
