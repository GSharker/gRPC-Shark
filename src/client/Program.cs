using gRPC.Geometry;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace grpcSharkClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Line.LineClient(channel);

            var reply = await client.GetLineAsync(
                new LineRequest()
                {
                    StartPoint = new gRPC.Geometry.Point3() { X = 0.0, Y = 0.0, Z = 0.0 },
                    EndPoint = new gRPC.Geometry.Point3() { X = 10, Y = 10, Z = 10 }
                });
            Console.WriteLine($"Line length = {reply.Length}");
            Console.ReadKey();
        }
    }
}
