using AOPConsoleApp.CodeRewriting;
using System;

namespace AOPConsoleApp.DAL
{
    public class DummyService : IDummyService
    {
        [CodeRewritingPostSharp]
        public DateTime GenerateDate(DateTime date, int age)
        {
            var gen = new Random();
            var start = new DateTime(date.Year - age, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(gen.Next(range));
        }
    }
}
