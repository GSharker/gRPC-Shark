using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GShark.Grpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        int port = 8000;

                        if (context.Configuration.GetSection("PORT").Exists() && int.TryParse(context.Configuration["PORT"], out int envPort))
                        {
                            Log.Information("Overriding default port ({Port}) for port specified in environment var ({EnvPort})", port, envPort);

                            port = envPort;
                        }

                        options.ListenAnyIP(
                               port,
                               listenOptions =>
                               {
                                   listenOptions.Protocols = HttpProtocols.Http2;
                               });

                        // used for grpc.http
                        options.ListenAnyIP(
                          port + 2,
                          listenOptions =>
                          {
                              listenOptions.Protocols = HttpProtocols.Http1;
                          });

                        // Add a {port + 1} mapping to https for grpc-web development
                        // as CORS pre-flight check needs to happen on https
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            options.ListenAnyIP(
                              port + 1,
                              listenOptions =>
                              {
                                  listenOptions.Protocols = HttpProtocols.Http2;

                                  listenOptions.UseHttps();
                              });

                            
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
