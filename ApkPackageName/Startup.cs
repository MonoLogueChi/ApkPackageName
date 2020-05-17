using ApkPackageName.Models.DbContext;
using ApkPackageName.Utils;
using ApkPackageName.Utils.Configuration;
using ApkPackageName.Utils.Dao;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace ApkPackageName
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var config = new AppConfiguration(Configuration);
      var appSetting = config.GetAppSetting();

      services.AddControllers();

      //数据库连接
      services.AddDbContextPool<ApkNameContext>(option =>
          {
            var sql = config.GetAppSetting().Sql;
            sql.Port = sql.Port == 0 ? 5432 : sql.Port;
            option.UseNpgsql(
                $"Host={sql.Host};Port={sql.Port};Database={sql.DataBase};Username={sql.UserName};Password={sql.PassWord};");
#if DEBUG
            option.UseLoggerFactory(new LoggerFactory(new[] {new DebugLoggerProvider()}));
#endif
          },
          appSetting.Sql.PoolSize);

      // 转接头，代理
      services.Configure<ForwardedHeadersOptions>(options =>
      {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
      });

      //跨域
      services.AddCors(options =>
      {
        options.AddDefaultPolicy(builder =>
            builder.WithOrigins(appSetting.WithOrigins)
                   .SetIsOriginAllowedToAllowWildcardSubdomains().WithMethods("GET", "POST", "OPTIONS")
                   .AllowAnyHeader());
      });

      //注入
      services.AddSingleton(s => config);

      services.AddScoped<ApkNameDao>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

      app.UseForwardedHeaders();
      app.UseRouting();

      app.UseCors();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}
