using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WuQu
{
    using System.IO;
    using System.Reflection;
    using Serilog;

    public class Program
    {
        public static int Main(string[] args)
        {
            var logsLocation =
                Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log.txt");
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(logsLocation)
                .CreateLogger();

            try
            {
                Log.Logger.Information("WuQu Launch");
                CreateHostBuilder(args).Build().Run();
                Log.Logger.Information("WuQu Exit");
                return 0;
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, "WuQu Crash");
                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}