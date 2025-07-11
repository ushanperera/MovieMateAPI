using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;

namespace MovieMateAPI.Dependencies.Configs
{
    public static class SerilogConfig
    {
        public static void AddSerilogServises(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(builder.Configuration)
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();

            builder.Host.UseSerilog();


        }

    }
}
