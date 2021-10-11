using GShark.Dto;
using GShark.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GShark.Methods
{
    public class NurbsServiceMethods
    {
        public static Func<CreatePointAtCurveRequestDto, CreatePointAtCurveResponseDto> CreateCurve = (request) =>
        {
            var nurbsCurve = new NurbsCurve(
                points: request.Points.Select(p => new Point3(p.X, p.Y, p.Z)).ToList(),
                weights: request.PointWeights.ToList(),
                degree: request.Degree);

            var point = nurbsCurve.PointAt(request.Parameter);

            return new CreatePointAtCurveResponseDto
            {
                Point = new Point3Dto() { X = point.X, Y = point.Y, Z = point.Z }
            };
        };
    }
}
