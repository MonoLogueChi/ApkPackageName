using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApkPackageName.Models.Config;
using ApkPackageName.Models.DbContext;
using ApkPackageName.Utils.Dao;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApkPackageName
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      AppSettings appSettings;
      using (var sr = new StreamReader("appsettings.json", Encoding.UTF8))
      {
        var s = sr.ReadToEndAsync();

        appSettings = JsonSerializer.Deserialize<AppSettings>(await s);
      }

      var host = CreateHostBuilder(args, appSettings.KestrelSettings).Build();
      CreateDbIfNotExists(host, appSettings);
      await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args, KestrelSettings ks)
    {
      return Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                  {
                    webBuilder.ConfigureKestrel(options =>
                    {
#if LINUX
                                if (ks.UnixSocketPath.Length > 0)
                                    foreach (var path in ks.UnixSocketPath)
                                    {
                                        if (File.Exists(path)) File.Delete(path);
                                        options.ListenUnixSocket(path);
                                    }
#endif
                      if (ks.Port.Length > 0)
                        foreach (var port in ks.Port)
                          options.Listen(IPAddress.Loopback, port);
                    }).UseStartup<Startup>();
                  });
    }

    private static void CreateDbIfNotExists(IHost host, AppSettings appSettings)
    {
      using var scope = host.Services.CreateScope();
      var services = scope.ServiceProvider;
      try
      {
        var context = services.GetRequiredService<ApkNameContext>();
        DbInitializer.Initialize(context, appSettings);
      }
      catch (Exception ex)
      {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
      }
    }
  }
}
