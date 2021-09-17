using Grpc.Net.Client;
using GShark.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GShark.gRPC.Client
{
    public class NurbsCurveClient : IDisposable
    {
        private readonly GrpcChannel channel;
        private readonly SampleService.SampleServiceClient service;

        public NurbsCurveClient(string host)
        {
            _ = host ?? throw new ArgumentNullException(nameof(host), "Host can't be null");

            channel = GrpcChannel.ForAddress(host);
            service = new SampleService.SampleServiceClient(channel);
        }

        public async Task<CreateCurveResponseDto> CreateNurbsCurve(CreateCurveRequestDto request)
        {
            Point3[] points = request
                .Points
                .Select(p => new GShark.gRPC.Point3
                {
                    X = p.X,
                    Y = p.Y,
                    Z = p.Z
                })
                .ToArray();

            var grpcRequest = new CreateCurveRequest();

            grpcRequest.Degree = request.Degree;
            grpcRequest.Points.AddRange(points);
            grpcRequest.PointWeights.AddRange(request.PointWeights);

            var createCurveResponse = await service.CreateCurveAsync(grpcRequest);

            return new CreateCurveResponseDto()
            {
                Degree = createCurveResponse.Degree,
                ControlPoints = createCurveResponse.ControlPoints.Select(p => new Point4Dto
                {
                    X = p.X,
                    Y = p.Y,
                    Z = p.Z,
                    W = p.W
                }),
                Knots = createCurveResponse.Knots
            };
        }

        public void Dispose()
        {
            channel.Dispose();
        }
    }
}
