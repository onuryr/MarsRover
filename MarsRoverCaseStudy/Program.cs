using MarsRoverCaseStudy.Business.Services;
using MarsRoverCaseStudy.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using System;

namespace MarsRoverCaseStudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddScoped<IBusinessService, BusinessService>()
                    .AddScoped<IDataHelper, DataHelper>()
                    .AddScoped<IConsoleHelper, ConsoleHelper>()
                    .AddScoped<IStringHelper, StringHelper>()
                    .BuildServiceProvider();

                var businessService = serviceProvider.GetService<IBusinessService>();

                businessService.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}Please restart the program.");
                logger.Error(ex.Message);
            } 
        }
    }
}
