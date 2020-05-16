using System;
using System.Globalization;

namespace AOPConsoleApp.DAL
{
    public class DummyRepository : IDummyRepository
    {
        public Guid SaveData(string name, int age, DateTime date)
        {
            name = name.ToUpper(new CultureInfo("tr-TR", false));
            date = DateTime.UtcNow;

            return Guid.NewGuid();
        }
    }
}
