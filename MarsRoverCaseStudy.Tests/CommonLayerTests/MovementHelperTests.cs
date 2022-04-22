using MarsRoverCaseStudy.Business.Common.Entities;
using MarsRoverCaseStudy.Business.Common.Entities.Enums;
using MarsRoverCaseStudy.Common.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MarsRoverCaseStudy.Tests.CommonLayerTests
{
    public class MovementHelperTests
    {
        private readonly MovementHelper _sut;
        private readonly Mock<IDataHelper> _dataHelper;

        public MovementHelperTests()
        {
            _dataHelper = new Mock<IDataHelper>();
            _sut = new MovementHelper(_dataHelper.Object);
        }

        [Fact]
        public void RunMoveCode_PositionOutOfBoundaries_ThrowsException()
        {
            Rover rover = new Rover
            {
                Id = 1,
                Position = new Position
                {
                    XCoordinate = 2,
                    YCoordinate = 2,
                    Direction = Direction.S
                },
                MoveCode = new List<string> { "M", "M", "M", "R", "R", "M", "M", "M" }
            };

            Plateau plateau = new Plateau
            {
                XLength = 3,
                YLength = 3
            };

            _dataHelper.Setup(e => e.ValidatePosition(rover.Position, plateau, 1)).Throws<Exception>();

            Assert.Throws<Exception>(() => _sut.RunMoveCode(rover, plateau));
        }

        [Fact]
        public void RunMoveCode_ValidParameters_Successful()
        {
            Rover rover = new Rover
            {
                Id = 1,
                Position = new Position
                {
                    XCoordinate = 2,
                    YCoordinate = 2,
                    Direction = Direction.S
                },
                MoveCode = new List<string> { "M", "L", "L", "M", "R", "M", "R", "R", "M", "L", "R", "R", "L" }
            };

            Plateau plateau = new Plateau
            {
                XLength = 3,
                YLength = 3
            };

            _sut.RunMoveCode(rover, plateau);
        }
    }
}
