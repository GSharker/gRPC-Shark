using gRPC.Geometry;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace gRPCShark
{
    public class LineService : Line.LineBase
    {
        private readonly ILogger<LineService> _logger;
        public LineService(ILogger<LineService> logger)
        {
            _logger = logger;
        }

        public async override Task<LineResult> GetLine(LineRequest request, ServerCallContext context)
        {
            GShark.Geometry.Point3 ps = new GShark.Geometry.Point3(request.StartPoint.X, request.StartPoint.Y, request.StartPoint.Z);
            GShark.Geometry.Point3 pe = new GShark.Geometry.Point3(request.EndPoint.X, request.EndPoint.Y, request.EndPoint.Z);
            GShark.Geometry.Line l = new GShark.Geometry.Line(ps, pe);
            return new LineResult
            {
                StartPoint = request.StartPoint,
                EndPoint = request.EndPoint,
                Length = l.Length
            };
        }
    }
}
