using MarsRoverCaseStudy.Business.Common.Entities;
using MarsRoverCaseStudy.Business.Common.Entities.Enums;
using MarsRoverCaseStudy.Business.Services;
using MarsRoverCaseStudy.Common.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MarsRoverCaseStudy.Tests.BusinessLayerTests
{
    public class BusinessServiceTests
    {
        private readonly BusinessService _sut;
        private readonly Mock<IDataHelper> _dataHelper;
        private readonly Mock<IConsoleHelper> _consoleHelper;

        public BusinessServiceTests()
        {
            _dataHelper = new Mock<IDataHelper>();
            _consoleHelper = new Mock<IConsoleHelper>();
            _sut = new BusinessService(_dataHelper.Object, _consoleHelper.Object);
        }

        [Fact]
        public void GetPlateau_InvalidValue_ThrowsException()
        {
            string plateauInput = "5 D";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => plateauInput);

            Plateau plateau = new Plateau
            {
                XLength = 5,
                YLength = 5
            };

            _dataHelper.Setup(e => e.GetPlateau(plateauInput)).Throws<Exception>();

            Assert.Throws<Exception>(() => _sut.Run());
        }

        [Fact]
        public void GetInitialPosition_CannotParseThirdValue_ThrowsException()
        {
            string plateauInput = "5 5";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => plateauInput);

            Plateau plateau = new Plateau
            {
                XLength = 5,
                YLength = 5
            };

            _dataHelper.Setup(e => e.GetPlateau(plateauInput)).Returns(() => plateau);

            string initialPositionInput = "2 2 Q";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => initialPositionInput);

            _dataHelper.Setup(e => e.GetInitialPosition(initialPositionInput)).Throws<Exception>();

            Assert.Throws<Exception>(() => _sut.Run());
        }

        [Fact]
        public void GetInitialPosition_InvalidPosition_ThrowsException()
        {
            int roverId = 1;
            string plateauInput = "5 5";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => plateauInput);

            Plateau plateau = new Plateau
            {
                XLength = 5,
                YLength = 5
            };

            _dataHelper.Setup(e => e.GetPlateau(plateauInput)).Returns(() => plateau);

            string initialPositionInput = "3 6 W";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => initialPositionInput);

            Position position = new Position()
            {
                XCoordinate = 3,
                YCoordinate = 6,
                Direction = Direction.W
            };

            _dataHelper.Setup(e => e.GetInitialPosition(initialPositionInput)).Returns(() => position);

            _dataHelper.Setup(e => e.ValidatePosition(position, plateau, roverId)).Throws<Exception>();
        }

        [Fact]
        public void GetMoveCodeList_InvalidInput_ThrowsException()
        {
            string plateauInput = "5 5";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => plateauInput);

            Plateau plateau = new Plateau
            {
                XLength = 5,
                YLength = 5
            };

            _dataHelper.Setup(e => e.GetPlateau(plateauInput)).Returns(() => plateau);

            string initialPositionInput = "3 4 W";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => initialPositionInput);

            Position position = new Position()
            {
                XCoordinate = 3,
                YCoordinate = 4,
                Direction = Direction.W
            };

            _dataHelper.Setup(e => e.GetInitialPosition(initialPositionInput)).Returns(() => position);

            _dataHelper.Setup(e => e.ValidatePosition(position, plateau, 1));

            string moveCodeInput = "MMQRF LR";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => moveCodeInput);

            _dataHelper.Setup(e => e.GetMoveCodeList(moveCodeInput)).Throws<Exception>();

            Assert.Throws<Exception>(() => _sut.Run());
        }

        [Fact]
        public void RunMoveCode_OutOfBoundaries_ThrowsException()
        {
            string plateauInput = "5 5";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => plateauInput);

            Plateau plateau = new Plateau
            {
                XLength = 5,
                YLength = 5
            };

            _dataHelper.Setup(e => e.GetPlateau(plateauInput)).Returns(() => plateau);

            string initialPositionInput = "3 4 W";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => initialPositionInput);

            Position position = new Position()
            {
                XCoordinate = 3,
                YCoordinate = 4,
                Direction = Direction.W
            };

            _dataHelper.Setup(e => e.GetInitialPosition(initialPositionInput)).Returns(() => position);

            _dataHelper.Setup(e => e.ValidatePosition(position, plateau, 1));

            string moveCodeInput = "MMMMM";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => moveCodeInput);

            List<string> moveCodeList = new List<string> { "M", "M", "M", "M", "M" };

            _dataHelper.Setup(e => e.GetMoveCodeList(moveCodeInput)).Returns(() => moveCodeList);

            int roverId = 1;
            Rover rover = new Rover
            {
                Id = roverId,
                Position = position,
                MoveCode = moveCodeList
            };

            _dataHelper.Setup(e => e.GetRover(roverId, position, moveCodeList)).Returns(() => rover);

            List<Rover> roverList = new List<Rover>();
            roverList.Add(rover);

            _dataHelper.Setup(e => e.RunMoveCode(rover, plateau)).Throws<Exception>();
        }

        [Fact]
        public void Run_ValidInputs_Successful()
        {
            string plateauInput = "5 5";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => plateauInput);

            Plateau plateau = new Plateau
            {
                XLength = 5,
                YLength = 5
            };

            _dataHelper.Setup(e => e.GetPlateau(plateauInput)).Returns(() => plateau);

            List<Rover> roverList = new List<Rover>();

            Rover rover = GetRover(1, plateau);
            roverList.Add(rover);

            foreach (var r in roverList)
            {
                Position newPosition = new Position()
                {
                    XCoordinate = 2,
                    YCoordinate = 5,
                    Direction = Direction.E
                };

                _dataHelper.Setup(e => e.RunMoveCode(r, plateau)).Returns(() => newPosition);

                string expectedPosition = "2 5 E";
                string actualPosition = $"{newPosition.XCoordinate} {newPosition.YCoordinate} {newPosition.Direction}";

                Assert.Equal(expectedPosition, actualPosition);
            }
            _sut.Run();
        }

        private Rover GetRover(int id, Plateau plateau)
        {
            string initialPositionInput = "3 4 W";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => initialPositionInput);

            Position position = new Position()
            {
                XCoordinate = 3,
                YCoordinate = 4,
                Direction = Direction.W
            };

            _dataHelper.Setup(e => e.GetInitialPosition(initialPositionInput)).Returns(() => position);

            _dataHelper.Setup(e => e.ValidatePosition(position, plateau, 1));

            string moveCodeInput = "MRMR";
            _consoleHelper.Setup(c => c.ReadLine()).Returns(() => moveCodeInput);

            List<string> moveCodeList = new List<string> { "M", "R", "M", "R" };

            _dataHelper.Setup(e => e.GetMoveCodeList(moveCodeInput)).Returns(() => moveCodeList);

            Rover rover = new Rover
            {
                Id = id,
                Position = position,
                MoveCode = moveCodeList
            };

            _dataHelper.Setup(e => e.GetRover(id, position, moveCodeList)).Returns(() => rover);

            return rover;
        }
    }
}
