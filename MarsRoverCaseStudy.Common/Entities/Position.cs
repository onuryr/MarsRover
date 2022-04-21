using MarsRoverCaseStudy.Business.Common.Entities.Enums;

namespace MarsRoverCaseStudy.Business.Common.Entities
{
    public class Position
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public Direction Direction { get; set; }
    }
}
