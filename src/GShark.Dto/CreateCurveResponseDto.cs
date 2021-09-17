using System.Collections.Generic;

namespace GShark.Dto
{
    public class CreateCurveResponseDto
    {
        public int Degree { get; set; }

        public IEnumerable<double> Knots { get; set; }

        public IEnumerable<Point4Dto> ControlPoints { get; set; }
    }
}
