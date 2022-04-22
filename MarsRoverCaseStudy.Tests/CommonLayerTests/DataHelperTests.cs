using MarsRoverCaseStudy.Business.Common.Entities;
using MarsRoverCaseStudy.Business.Common.Entities.Enums;
using MarsRoverCaseStudy.Common.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MarsRoverCaseStudy.Tests.CommonLayerTests
{
    public class DataHelperTests
    {
        private readonly DataHelper _sut;
        private readonly Mock<IStringHelper> _stringHelper;

        public DataHelperTests()
        {
            _stringHelper = new Mock<IStringHelper>();
            _sut = new DataHelper(_stringHelper.Object);
        }

        #region GetPlateauTests
        [Fact]
        public void GetPlateau_InputNotTwoValues_ThrowsException()
        {
            string input = "5 5 5";
            List<string> inputList = new List<string> { "5", "5", "5" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            Assert.Throws<Exception>(() => _sut.GetPlateau(input));
        }

        [Fact]
        public void GetPlateau_InputContainsNonIntegerValue_ThrowsException()
        {
            string input = "5 W";
            List<string> inputList = new List<string> { "5", "W" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            Assert.Throws<Exception>(() => _sut.GetPlateau(input));
        }

        [Fact]
        public void GetPlateau_InputContainsLessThanOrEqualToZeroValue_ThrowsException()
        {
            string input = "5 -1";
            List<string> inputList = new List<string> { "5", "-1" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            Assert.Throws<Exception>(() => _sut.GetPlateau(input));
        }

        [Fact]
        public void GetPlateau_ValidInput_Successful()
        {
            string input = "4 4";
            List<string> inputList = new List<string> { "4", "4" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);
            
            _sut.GetPlateau(input);
        }
        #endregion

        #region GetInitialPositionTests
        [Fact]
        public void GetInitialPosition_InputNotThreeValues_ThrowsException()
        {
            string input = "3 2";
            List<string> inputList = new List<string> { "3", "2" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            Assert.Throws<Exception>(() => _sut.GetInitialPosition(input));
        }

        [Fact]
        public void GetInitialPosition_InputContainsNonIntegerValue_ThrowsException()
        {
            string input = "3 W E";
            List<string> inputList = new List<string> { "3", "W", "E" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            Assert.Throws<Exception>(() => _sut.GetInitialPosition(input));
        }

        [Fact]
        public void GetInitialPosition_InputContainsLessThanZeroValue_ThrowsException()
        {
            string input = "-2 3 N";
            List<string> inputList = new List<string> { "-2", "3", "N" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            Assert.Throws<Exception>(() => _sut.GetInitialPosition(input));
        }

        [Fact]
        public void GetInitialPosition_ThirdValueCannotBeParsed_ThrowsException()
        {
            string input = "2 3 C";
            List<string> inputList = new List<string> { "2", "3", "C" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            Assert.Throws<Exception>(() => _sut.GetInitialPosition(input));
        }

        [Fact]
        public void GetInitialPosition_ValidInput_Successful()
        {
            string input = "1 2 W";
            List<string> inputList = new List<string> { "1", "2", "W" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            _sut.GetInitialPosition(input);
        }
        #endregion

        #region GetMoveCodeListTests
        [Fact]
        public void GetMoveCodeList_InputContainsSpaceBetweenLetters_ThrowsException()
        {
            string input = "LM RLM";
            List<string> inputList = new List<string> { "L", "M", " ", "R", "L", "M" };

            Assert.Throws<Exception>(() => _sut.GetMoveCodeList(input));
        }

        [Fact]
        public void GetMoveCodeList_InputContainsLetterThatCannotBeParsed_ThrowsException()
        {
            string input = "RMTLM";
            List<string> inputList = new List<string> { "R", "M", "T", "L", "M" };

            Assert.Throws<Exception>(() => _sut.GetMoveCodeList(input));
        }

        [Fact]
        public void GetMoveCodeList_ValidInput_Successful()
        {
            string input = "RMLMM";
            List<string> inputList = new List<string> { "R", "M", "L", "M", "M" };

            _stringHelper.Setup(s => s.Split(input, StringSplitOptions.RemoveEmptyEntries)).Returns(inputList);

            _sut.GetMoveCodeList(input);
        }
        #endregion

        #region ValidatePositionTests
        [Fact]
        public void ValidatePosition_PositionOutOfBoundaries_ThrowsException()
        {
            Position position = new Position
            {
                XCoordinate = 2,
                YCoordinate = 4,
                Direction = Direction.W
            };

            Plateau plateau = new Plateau
            {
                XLength = 3,
                YLength = 3
            };

            Assert.Throws<Exception>(() => _sut.ValidatePosition(position, plateau, 1));
        }
        #endregion

        #region GetRoverTests
        [Fact]
        public void GetRover_IdContainsLessThanOrEqualToZeroValue_ThrowsException()
        {
            int roverId = -1;

            Position position = new Position
            {
                XCoordinate = 5,
                YCoordinate = 5,
                Direction = Direction.N
            };

            List<string> moveCodeList = new List<string> { "R", "M", "L", "M", "M" };

            Assert.Throws<Exception>(() => _sut.GetRover(roverId, position, moveCodeList));
        }

        [Fact]
        public void GetRover_ValidParameters_Successful()
        {
            int roverId = 2;

            Position position = new Position
            {
                XCoordinate = 5,
                YCoordinate = 5,
                Direction = Direction.N
            };

            List<string> moveCodeList = new List<string> { "R", "M", "L", "M", "M" };

            _sut.GetRover(roverId, position, moveCodeList);
        }
        #endregion

    }
}
