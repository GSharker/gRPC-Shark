using System.Collections.Generic;

namespace GShark.Dto
{
    public class CreateCurveRequestDto
    {
        public IEnumerable<Point3Dto> Points { get; set; }
        public IEnumerable<double> PointWeights { get; set; }
        public int Degree { get; set; }
    }
}
