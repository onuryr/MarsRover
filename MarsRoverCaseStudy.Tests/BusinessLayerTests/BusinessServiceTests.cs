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
        private readonly Mock<IMovementHelper> _movementHelper;

        public BusinessServiceTests()
        {
            _dataHelper = new Mock<IDataHelper>();
            _consoleHelper = new Mock<IConsoleHelper>();
            _movementHelper = new Mock<IMovementHelper>();
            _sut = new BusinessService(_dataHelper.Object, _consoleHelper.Object, _movementHelper.Object);
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

            Assert.Throws<Exception>(() => _sut.Run(2));
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

            Assert.Throws<Exception>(() => _sut.Run(2));
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

            Assert.Throws<Exception>(() => _sut.Run(2));
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

            int roverId = 2;
            Rover rover = new Rover { Id = roverId, Position = position, MoveCode = moveCodeList };

            _dataHelper.Setup(e => e.GetRover(roverId, position, moveCodeList)).Returns(() => rover);

            List<Rover> roverList = new List<Rover>();
            roverList.Add(rover);

            _movementHelper.Setup(e => e.RunMoveCode(rover, plateau)).Throws<Exception>();

            _sut.Run(2);
        }

        [Fact]
        public void Run_ValidInputs_Successful()
        {
            string plateauInput = "5 5";
            string initialPositionInput1 = "3 4 W";
            string moveCodeInput1 = "MRMR";
            string initialPositionInput2 = "1 2 N";
            string moveCodeInput2 = "MRR";

            _consoleHelper.SetupSequence(t => t.ReadLine())
                .Returns(plateauInput)
                .Returns(initialPositionInput1)
                .Returns(moveCodeInput1)
                .Returns(initialPositionInput2)
                .Returns(moveCodeInput2);

            Plateau plateau = new Plateau
            {
                XLength = 5,
                YLength = 5
            };

            _dataHelper.Setup(e => e.GetPlateau(plateauInput)).Returns(() => plateau);

            List<Rover> roverList = new List<Rover>();

            Position position1 = new Position()
            {
                XCoordinate = 3,
                YCoordinate = 4,
                Direction = Direction.W
            };
            Position position2 = new Position()
            {
                XCoordinate = 1,
                YCoordinate = 2,
                Direction = Direction.N
            };

            List<string> moveCodeList1 = new List<string> { "M", "R", "M", "R" };
            List<string> moveCodeList2 = new List<string> { "M", "R", "R" };

            _dataHelper.Setup(e => e.GetInitialPosition(initialPositionInput1)).Returns(() => position1);
            _dataHelper.Setup(e => e.GetInitialPosition(initialPositionInput2)).Returns(() => position2);

            _dataHelper.Setup(e => e.ValidatePosition(position1, plateau, 1));
            _dataHelper.Setup(e => e.ValidatePosition(position2, plateau, 2));

            _dataHelper.Setup(e => e.GetMoveCodeList(moveCodeInput1)).Returns(() => moveCodeList1);
            _dataHelper.Setup(e => e.GetMoveCodeList(moveCodeInput2)).Returns(() => moveCodeList2);

            Rover rover1 = new Rover { Id = 1, Position = position1, MoveCode = moveCodeList1 };
            Rover rover2 = new Rover { Id = 2, Position = position2, MoveCode = moveCodeList2 };

            _dataHelper.Setup(e => e.GetRover(1, position1, moveCodeList1)).Returns(() => rover1);
            _dataHelper.Setup(e => e.GetRover(2, position2, moveCodeList2)).Returns(() => rover2);

            roverList.Add(rover1);
            roverList.Add(rover2);

            Position newPosition1 = new Position()
            {
                XCoordinate = 2,
                YCoordinate = 5,
                Direction = Direction.E
            };

            Position newPosition2 = new Position()
            {
                XCoordinate = 1,
                YCoordinate = 3,
                Direction = Direction.S
            };

            _movementHelper.Setup(e => e.RunMoveCode(rover1, plateau)).Returns(() => newPosition1);
            _movementHelper.Setup(e => e.RunMoveCode(rover2, plateau)).Returns(() => newPosition2);

            string expectedPosition1 = "2 5 E";
            string actualPosition1 = $"{newPosition1.XCoordinate} {newPosition1.YCoordinate} {newPosition1.Direction}";
            Assert.Equal(expectedPosition1, actualPosition1);

            string expectedPosition2 = "1 3 S";
            string actualPosition2 = $"{newPosition2.XCoordinate} {newPosition2.YCoordinate} {newPosition2.Direction}";
            Assert.Equal(expectedPosition2, actualPosition2);

            _sut.Run(2);
        }
    }
}
