using MarsRoverCaseStudy.Business.Common.Entities;
using MarsRoverCaseStudy.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverCaseStudy.Business.Services
{
    public interface IBusinessService
    {
        void Run(int roverCount);
    }

    public class BusinessService : IBusinessService
    {
        private readonly IDataHelper _dataHelper;
        private readonly IConsoleHelper _consoleHelper;
        private readonly IMovementHelper _roverHelper;

        public BusinessService(IDataHelper dataHelper, IConsoleHelper consoleHelper, IMovementHelper roverHelper)
        {
            _dataHelper = dataHelper;
            _consoleHelper = consoleHelper;
            _roverHelper = roverHelper;
        }

        public void Run(int roverCount)
        {
            try
            {
                string input = _consoleHelper.ReadLine();
                Plateau plateau = _dataHelper.GetPlateau(input);

                List<Rover> roverList = new List<Rover>();

                for (int i = 0; i < roverCount; i++)
                {
                    int roverId = i + 1;
                    input = _consoleHelper.ReadLine();
                    Position position = _dataHelper.GetInitialPosition(input);

                    _dataHelper.ValidatePosition(position, plateau, roverId);

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
                        Position finalPosition = _roverHelper.RunMoveCode(rover, plateau);
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
                throw ex;
            }
        }
    }
}
