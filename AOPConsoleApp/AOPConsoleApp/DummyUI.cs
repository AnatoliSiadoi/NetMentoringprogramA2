using AOPConsoleApp.DAL;
using System;

namespace AOPConsoleApp
{
    public class DummyUI
    {
        private readonly IDummyRepository _dynamicProxyRepository;
        private readonly IDummyService _dummyService;

        public DummyUI(IDummyRepository dynamicProxyRepository, IDummyService dummyService)
        {
            _dynamicProxyRepository = dynamicProxyRepository;
            _dummyService = dummyService;
        }

        public void Start()
        {
            string name = string.Empty;
            int age = 0;

            Console.WriteLine("********************");
            Console.Write("Enter your name : ");
            name = Console.ReadLine();
            Console.Write("How old are you? : ");
            Int32.TryParse(Console.ReadLine(), out age);
            var date = _dummyService.GenerateDate(DateTime.Now, age);
            Console.WriteLine("*****************************************************");
            Console.WriteLine($"You had a very happy day in your life : {date.ToString("d/M/yyyy")} ");
            Console.WriteLine("*****************************************************");
            var guid = _dynamicProxyRepository.SaveData(name, age, date);
            Console.WriteLine("********************");
            Console.WriteLine("Press 'q' to quit.");
            while (Console.Read() != 'q') ;
            Console.ReadKey();
        }
    }
}
