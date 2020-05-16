using System;

namespace AOPConsoleApp.DAL
{
    public interface IDummyRepository
    {
        Guid SaveData(string name, int age, DateTime date);
    }
}
