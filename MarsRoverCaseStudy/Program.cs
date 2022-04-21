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
            
            Run:
            try
            {
                businessService.Run();
            }
            catch (Exception)
            {
                string input;
                while (true)
                {
                    Console.WriteLine("Please type 'Y' to restart or 'N' to stop the program.");
                    input = Console.ReadLine();

                    if ((input.ToUpper() == "Y" || input.ToUpper() == "N")) 
                        break;
                }

                if (input.ToUpper() == "Y")
                {
                    goto Run;
                }
            }
        }
    }
}
