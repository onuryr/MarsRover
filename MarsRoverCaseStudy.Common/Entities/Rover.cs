using System.Collections.Generic;

namespace MarsRoverCaseStudy.Business.Common.Entities
{
    public class Rover
    {
        public int Id { get; set; }
        public Position Position { get; set; }
        public List<string> MoveCode { get; set; }
    }
}
