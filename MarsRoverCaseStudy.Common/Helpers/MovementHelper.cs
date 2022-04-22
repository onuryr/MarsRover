using MarsRoverCaseStudy.Business.Common.Entities;
using MarsRoverCaseStudy.Business.Common.Entities.Enums;
using System;

namespace MarsRoverCaseStudy.Common.Helpers
{
    public interface IMovementHelper
    {
        Position RunMoveCode(Rover rover, Plateau plateau);
    }

    public class MovementHelper : IMovementHelper
    {
        private readonly IDataHelper _dataHelper;

        public MovementHelper(IDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        public Position RunMoveCode(Rover rover, Plateau plateau)
        {
            foreach (var letter in rover.MoveCode)
            {
                if (Enum.TryParse(letter, out BodyRelativeDirection direction))
                {
                    rover.Position = Turn(direction, rover.Position);
                }
                else
                {
                    rover.Position = Move(rover.Position);
                    _dataHelper.ValidatePosition(rover.Position, plateau, rover.Id);
                }
            }

            return rover.Position;
        }

        private Position Move(Position position)
        {
            switch (position.Direction)
            {
                case Direction.N:
                    position.YCoordinate++;
                    break;
                case Direction.E:
                    position.XCoordinate++;
                    break;
                case Direction.S:
                    position.YCoordinate--;
                    break;
                case Direction.W:
                    position.XCoordinate--;
                    break;
            }

            return position;
        }

        private Position Turn(BodyRelativeDirection bodyRelativeDirection, Position position)
        {
            switch (bodyRelativeDirection)
            {
                case BodyRelativeDirection.L:
                    switch (position.Direction)
                    {
                        case Direction.N:
                            position.Direction = Direction.W;
                            break;
                        case Direction.S:
                            position.Direction = Direction.E;
                            break;
                        case Direction.W:
                            position.Direction = Direction.S;
                            break;
                        case Direction.E:
                            position.Direction = Direction.N;
                            break;
                    }
                    break;
                case BodyRelativeDirection.R:
                    switch (position.Direction)
                    {
                        case Direction.N:
                            position.Direction = Direction.E;
                            break;
                        case Direction.S:
                            position.Direction = Direction.W;
                            break;
                        case Direction.W:
                            position.Direction = Direction.N;
                            break;
                        case Direction.E:
                            position.Direction = Direction.S;
                            break;
                    }
                    break;
            }

            return position;
        }
    }
}
