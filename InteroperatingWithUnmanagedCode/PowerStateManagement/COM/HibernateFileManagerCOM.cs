using PowerStateManagement.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement.COM
{
    [ComVisible(true)]
    [Guid("F7CC6A7E-A9AD-417A-B585-2C3D6CE4CDBF")]
    [ClassInterface(ClassInterfaceType.None)]
    public class HibernateFileManagerCOM : IHibernateFileManager
    {
        private readonly HibernateFileManager _hibernateFileManager;

        public HibernateFileManagerCOM()
        {
            _hibernateFileManager = new HibernateFileManager();
        }

        public void Delete()
        {
            _hibernateFileManager.Delete();
        }

        public void Reserve()
        {
            _hibernateFileManager.Reserve();
        }
    }
}
