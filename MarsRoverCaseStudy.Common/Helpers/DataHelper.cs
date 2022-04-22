using MarsRoverCaseStudy.Business.Common.Entities;
using MarsRoverCaseStudy.Business.Common.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRoverCaseStudy.Common.Helpers
{
    public interface IDataHelper
    {
        Plateau GetPlateau(string input);
        Position GetInitialPosition(string input);
        List<string> GetMoveCodeList(string input);
        Rover GetRover(int id, Position position, List<string> moveCodeList);
        void ValidatePosition(Position position, Plateau plateau, int roverNumber);
    }

    public class DataHelper : IDataHelper
    {
        private readonly IStringHelper _stringHelper;

        public DataHelper(IStringHelper stringHelper)
        {
            _stringHelper = stringHelper;
        }

        public Plateau GetPlateau(string input)
        {
            List<string> plateauXAndY = _stringHelper.Split(input);

            if (plateauXAndY.Count != 2)
            {
                throw new Exception("You must enter exactly 2 values. Example: '5 6'");
            }

            if (!(int.TryParse(plateauXAndY[0], out int xLength) && int.TryParse(plateauXAndY[1], out int yLength)))
            {
                throw new Exception("All values must be integer.");
            }

            if (xLength <= 0 || yLength <= 0)
            {
                throw new Exception("All values must be greater than 0.");
            }

            return new Plateau
            {
                XLength = xLength,
                YLength = yLength
            };
        }

        public Position GetInitialPosition(string input)
        {
            List<string> roverCoordinates = _stringHelper.Split(input);

            if (roverCoordinates.Count != 3)
            {
                throw new Exception("You must enter exactly 3 values. Example: '4 5 N'");
            }

            if (!(int.TryParse(roverCoordinates[0], out int xCoordinate) && int.TryParse(roverCoordinates[1], out int yCoordinate)))
            {
                throw new Exception("First two values must be integer.");
            }

            if (xCoordinate < 0 || yCoordinate < 0)
            {
                throw new Exception("First two values must be equal to or greater than 0.");
            }

            if (!Enum.TryParse(roverCoordinates[2].ToUpper(), out Direction direction))
            {
                throw new Exception($"Third value must be one of the following directions => {Direction.N}, {Direction.S}, {Direction.E}, {Direction.W}");
            }

            return new Position
            {
                XCoordinate = xCoordinate,
                YCoordinate = yCoordinate,
                Direction = direction
            };
        }

        public List<string> GetMoveCodeList(string input)
        {
            List<string> moveCodeList = input.Trim().ToCharArray().Select(c => c.ToString().ToUpper()).ToList();

            foreach (var moveLetter in moveCodeList)
            {
                if (string.IsNullOrWhiteSpace(moveLetter))
                {
                    throw new Exception($"There should be no spaces between letters.");
                }

                if (!(Enum.TryParse(moveLetter, out BodyRelativeDirection bodyRelativeDirection) || Enum.TryParse(moveLetter, out Movement move)))
                {
                    throw new Exception($"Move code may only include these letters => {BodyRelativeDirection.L}, {BodyRelativeDirection.R}, {Movement.M}");
                }
            }

            return moveCodeList;
        }

        public Rover GetRover(int id, Position position, List<string> moveCodeList)
        {
            if (id <= 0)
            {
                throw new Exception($"Rover id must be greater than 0.");
            }

            return new Rover 
            { 
                Id = id, 
                Position = position, 
                MoveCode = moveCodeList 
            };
        }

        public void ValidatePosition(Position position, Plateau plateau, int roverNumber)
        {
            if (position.XCoordinate > plateau.XLength || position.XCoordinate < 0
                || position.YCoordinate > plateau.YLength || position.YCoordinate < 0)
            {
                throw new Exception($"Rover {roverNumber} is out of boundaries: {position.XCoordinate} {position.YCoordinate} {position.Direction}. Plateau: {plateau.XLength} {plateau.YLength}");
            }
        }

    }
}
