using Grpc.Net.Client;
using GShark.Dto;
using GShark.gRPC.Client;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using System;
using System.Linq;
using System.Net.Http.Json;

namespace GShark.Tests.Load
{
    class Program
    {
        static void Main(string[] args)
        {
            int degree = 3;
            var points = new Geometry.Point3[]
            {
                new Geometry.Point3
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                },
                new Geometry.Point3
                {
                    X = 0,
                    Y = 0,
                    Z = 100
                },
                new Geometry.Point3
                {
                    X = 40,
                    Y = 100,
                    Z = 100
                },
                new Geometry.Point3
                {
                    X = 40,
                    Y = 400,
                    Z = 400
                }
            };
            var weights = Enumerable.Range(0, points.Length).Select((_, i) => (double)i);

            CreateCurveRequestDto dto = new CreateCurveRequestDto
            {
                Degree = degree,
                Points = points.Select(p => new Point3Dto { X = p.X, Y = p.Y, Z = p.Z }),
                PointWeights = weights
            };

            var grpcRequest = new GShark.gRPC.CreateCurveRequest();

            GShark.gRPC.Point3[] grpcPoints = points
               .Select(p => new GShark.gRPC.Point3
               {
                   X = p.X,
                   Y = p.Y,
                   Z = p.Z
               })
               .ToArray();

            grpcRequest.Degree = degree;
            grpcRequest.Points.AddRange(grpcPoints);
            grpcRequest.PointWeights.AddRange(weights);

            var client = new NurbsCurveClient("https://localhost:8001");

            IStep grpcNurbsCurveStep = Step.Create(
                name: "grpc:nurbs_curve",
                execute: async context =>
                {
                    var response = await client.CreateNurbsCurve(dto);

                    return Response.Ok(response);
                });

            IStep httpNurbsCurveStep = Step.Create(
                name: "http:nurbs_curve",
                clientFactory: HttpClientFactory.Create(),
                execute: async context =>
                {
                    var request = Http
                        .CreateRequest("POST", "https://localhost:5001/sample/nurbscurve")
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonContent.Create(dto));

                    var response = await Http.Send(request, context);
                    return Response.Ok(response);
                });

            Scenario nurbsCurveScenario = ScenarioBuilder
                .CreateScenario("create_nurbs_curve",
                grpcNurbsCurveStep,
                httpNurbsCurveStep)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    // 16 "threads" continuously stressing for 20 seconds
                    Simulation.KeepConstant(copies: 16, during: TimeSpan.FromSeconds(20)),
                    Simulation.InjectPerSec(100, during: TimeSpan.FromSeconds(20))
                );

            NBomberRunner
                .RegisterScenarios(nurbsCurveScenario)
                .Run();
        }
    }
}
