using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Sampling.Local;
using System.Configuration;

namespace ECChkAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                               .Enrich.WithExceptionDetails()
                               .MinimumLevel.Information()
                               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                               .Enrich.FromLogContext()
                               .WriteTo.File(
                                      new JsonFormatter(),
                                     @"Test.log",
                                     rollingInterval: RollingInterval.Day,
                                     rollOnFileSizeLimit: true,
                                     fileSizeLimitBytes: 10000000)
                               .CreateLogger();

            Serilog.Log.Information("Starting web host");
            //var recorder = new AWSXRayRecorderBuilder().WithSamplingStrategy(new LocalizedSamplingStrategy("sampling-rules.json")).Build();
           // AWSXRayRecorder.InitializeInstance();
            
            CreateHostBuilder(args).Build().Run();
            //var host = new WebHostBuilder()
            //  .UseKestrel()
            //  .UseContentRoot(Directory.GetCurrentDirectory())
            //  .UseIISIntegration()
            //  .UseStartup<Startup>()
            //  .Build();

            //host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseSerilog();
                    
                });
    }
}
