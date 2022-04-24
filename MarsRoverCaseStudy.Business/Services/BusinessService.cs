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
        private readonly IMovementHelper _movementHelper;

        public BusinessService(IDataHelper dataHelper, IConsoleHelper consoleHelper, IMovementHelper movementHelper)
        {
            _dataHelper = dataHelper;
            _consoleHelper = consoleHelper;
            _movementHelper = movementHelper;
        }

        public void Run()
        {
            try
            {
                string input = _consoleHelper.ReadLine();
                Plateau plateau = _dataHelper.GetPlateau(input);

                int count = 0;
                int roverId = 1;
                Position position = null;
                List<Rover> roverList = new List<Rover>();

                while (!string.IsNullOrEmpty(input = _consoleHelper.ReadLine()))
                {
                    count++;
                    if (count % 2 == 1)
                    {
                        position = _dataHelper.GetInitialPosition(input);

                        _dataHelper.ValidatePosition(position, plateau, roverId);
                    }
                    else
                    {
                        List<string> moveCodeList = _dataHelper.GetMoveCodeList(input);

                        Rover rover = _dataHelper.GetRover(roverId, position, moveCodeList);
                        roverList.Add(rover);
                        roverId++;
                    }
                }

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Empty);

                foreach (var rover in roverList)
                {
                    try
                    {
                        Position finalPosition = _movementHelper.RunMoveCode(rover, plateau);
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
