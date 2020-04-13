using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement.Interfaces
{
    [ComVisible(true)]
    [Guid("04269623-5D63-4260-A302-CCC95211A62F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IHibernateFileManager
    {
        void Reserve();

        void Delete();
    }
}
