using MarsRoverCaseStudy.Business.Common.Entities;
using MarsRoverCaseStudy.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverCaseStudy.Business.Services
{
    public interface IBusinessService
    {
        void Run();
    }

    public class BusinessService : IBusinessService
    {
        private readonly IDataHelper _dataHelper;
        private readonly IConsoleHelper _consoleHelper;

        public BusinessService(IDataHelper dataHelper, IConsoleHelper consoleHelper)
        {
            _dataHelper = dataHelper;
            _consoleHelper = consoleHelper;
        }

        public void Run()
        {
            try
            {
                _consoleHelper.WriteLine("Please enter two integer values to determine the size of the plateau:");
                string input = _consoleHelper.ReadLine();
                Plateau plateau = _dataHelper.GetPlateau(input);

                List<Rover> roverList = new List<Rover>();

                for (int i = 0; i < 2; i++)
                {
                    int roverId = i + 1;
                    _consoleHelper.WriteLine($"Please enter the coordinates for rover {roverId}:");
                    input = _consoleHelper.ReadLine();
                    Position position = _dataHelper.GetInitialPosition(input);

                    _dataHelper.ValidatePosition(position, plateau, roverId);

                    _consoleHelper.WriteLine($"In order to move rover {roverId}, please enter the code:");
                    input = _consoleHelper.ReadLine();
                    List<string> moveCodeList = _dataHelper.GetMoveCodeList(input);

                    Rover rover = _dataHelper.GetRover(roverId, position, moveCodeList);
                    roverList.Add(rover);
                }

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var rover in roverList)
                {
                    try
                    {
                        Position finalPosition = _dataHelper.RunMoveCode(rover, plateau);
                        stringBuilder.AppendLine($"{finalPosition.XCoordinate} {finalPosition.YCoordinate} {finalPosition.Direction}");
                    }
                    catch (Exception ex)
                    {
                        stringBuilder.AppendLine(ex.Message);
                    }
                }

                _consoleHelper.WriteLine(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                _consoleHelper.WriteLine($"{ex.Message}");
                throw;
            }
        }
    }
}
