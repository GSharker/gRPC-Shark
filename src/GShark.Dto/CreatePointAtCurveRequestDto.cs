using System.Collections.Generic;

namespace GShark.Dto
{
    public class CreatePointAtCurveRequestDto
    {
        public IEnumerable<Point3Dto> Points { get; set; }
        public IEnumerable<double> PointWeights { get; set; }
        public int Degree { get; set; }
        public double Parameter {  get; set; }
    }
}
