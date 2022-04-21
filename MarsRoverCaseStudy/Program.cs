using MarsRoverCaseStudy.Business.Services;
using MarsRoverCaseStudy.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarsRoverCaseStudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IBusinessService, BusinessService>()
                .AddScoped<IDataHelper, DataHelper>()
                .AddScoped<IConsoleHelper, ConsoleHelper>()
                .AddScoped<IStringHelper, StringHelper>()
                .BuildServiceProvider();

            var businessService = serviceProvider.GetService<IBusinessService>();

            while (true)
            {
                try
                {
                    businessService.Run();
                    break;
                }
                catch (Exception)
                {
                    string input;

                    while (true)
                    {
                        Console.WriteLine("Please type 'Y' to restart or 'N' to stop the program.");
                        input = Console.ReadLine().ToUpper();
                        if (input == "Y" || input == "N")
                            break;
                    }

                    if (input == "N")
                        break;
                }
            }


            //Run:
            //try
            //{
            //    businessService.Run();
            //}
            //catch (Exception)
            //{
            //    string input;
            //    while (true)
            //    {
            //        Console.WriteLine("Please type 'Y' to restart or 'N' to stop the program.");
            //        input = Console.ReadLine().ToUpper();

            //        if (input == "Y" || input == "N")
            //            break;
            //    }

            //    if (input == "Y")
            //    {
            //        goto Run;
            //    }
            //}
        }
    }
}
