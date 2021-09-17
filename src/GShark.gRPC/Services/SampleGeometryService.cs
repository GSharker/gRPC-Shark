using Grpc.Core;
using GShark.Dto;
using GShark.gRPC;
using GShark.Methods;
using System.Linq;
using System.Threading.Tasks;

namespace GShark.Grpc
{
    public class SampleGeometryService : SampleService.SampleServiceBase
    {
        public override Task<CreateCurveResponse> CreateCurve(CreateCurveRequest request, ServerCallContext context)
        {
            var requestDto = new CreateCurveRequestDto
            {
                Degree = request.Degree,
                Points = request.Points.Select(p => new Point3Dto { X = p.X, Y = p.Y, Z = p.Z }),
                PointWeights = request.PointWeights
            };

            CreateCurveResponseDto responseDto = SampleMethods.CreateCurve(requestDto);

            return Task.FromResult(ToMessage(responseDto));
        }

        private static CreateCurveResponse ToMessage(CreateCurveResponseDto responseDto)
        {
            var response = new CreateCurveResponse();

            response.Degree = responseDto.Degree;
            response.Knots.AddRange(responseDto.Knots);
            response.ControlPoints.AddRange(responseDto
                .ControlPoints
                .Select(p => new Point4
                {
                    X = p.X,
                    Y = p.Y,
                    Z = p.Z,
                    W = p.W
                })
                .ToList());

            return response;
        }
    }
}
