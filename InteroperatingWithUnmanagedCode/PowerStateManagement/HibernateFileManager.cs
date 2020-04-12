using PowerStateManagement.Enums;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PowerStateManagement
{
    public class HibernateFileManager
    {
        public void Reserve()
        {
            ExecuteHibernateFileAction(HibernateFileAction.Reserve);
        }

        public void Delete()
        {
            ExecuteHibernateFileAction(HibernateFileAction.Delete);
        }

        private void ExecuteHibernateFileAction(HibernateFileAction fileAction)
        {
            int inputBufferLength = Marshal.SizeOf<bool>();
            IntPtr inputBuffer = Marshal.AllocCoTaskMem(inputBufferLength);
            Marshal.WriteByte(inputBuffer, (byte)fileAction);

            var retval = PowerManagementInteroperation.CallNtPowerInformation(
                (int)PowerInformationLevel.SystemReserveHiberFile,
                inputBuffer,
                inputBufferLength,
                IntPtr.Zero,
                0);
            Marshal.FreeHGlobal(inputBuffer);
            if (retval == PowerManagementInteroperation.STATUS_SUCCESS)
            {
            }
            else
            {
                throw new Win32Exception();
            }
        }
    }
}
