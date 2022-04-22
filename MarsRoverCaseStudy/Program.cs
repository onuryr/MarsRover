using MarsRoverCaseStudy.Business.Services;
using MarsRoverCaseStudy.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using System;
using System.IO;

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
                    .AddScoped<IMovementHelper, MovementHelper>()
                    .BuildServiceProvider();

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
                    .Build();

                int roverCount = 2;

                var roverCountSection = configuration.GetSection("RoverCount");
                if (roverCountSection.Exists() && int.TryParse(roverCountSection.Value, out int roverCountTemp))
                {
                    roverCount = roverCountTemp;
                }

                var businessService = serviceProvider.GetService<IBusinessService>();
                businessService.Run(roverCount);

                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}Please restart the program.");
                logger.Error(ex.Message);
            } 
        }
    }
}
