using System;

namespace AOPConsoleApp.DAL
{
    public interface IDummyService
    {
        DateTime GenerateDate(DateTime date, int age);
    }
}
