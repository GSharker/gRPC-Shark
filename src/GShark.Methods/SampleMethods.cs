using GShark.Dto;
using GShark.Geometry;
using System;
using System.Linq;

namespace GShark.Methods
{
    /// <summary>
    /// TODO: Replace with generated code
    /// </summary>
    public class SampleMethods
    {
        public static Func<CreateCurveRequestDto, CreateCurveResponseDto> CreateCurve = (request) =>
        {
            var nurbsCurve = new NurbsCurve(
                points: request.Points.Select(p => new Point3(p.X, p.Y, p.Z)).ToList(),
                weights: request.PointWeights.ToList(),
                degree: request.Degree);

            // TODO: Finalize curve response
            return new CreateCurveResponseDto
            {
                Degree = nurbsCurve.Degree,
                Knots = nurbsCurve.Knots,
                ControlPoints = nurbsCurve
                    .ControlPoints
                    .Select(p => new Point4Dto { X = p.X, Y = p.Y, Z = p.Z, W = p.W })
            };
        };
    }
}
