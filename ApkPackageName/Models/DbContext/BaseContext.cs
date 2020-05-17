using System.IO;
using System.Text;
using System.Text.Json;
using ApkPackageName.Models.Config;
using Microsoft.EntityFrameworkCore;

namespace ApkPackageName.Models.DbContext
{
  public class BaseContext : Microsoft.EntityFrameworkCore.DbContext
  {
    private protected readonly Sql Sql;

    public BaseContext(DbContextOptions options) : base(options)
    {
      using var sr = new StreamReader("appsettings.json", Encoding.UTF8);
      var s = sr.ReadToEnd();
      var settings = JsonSerializer.Deserialize<AppSettings>(s);
      Sql = settings.Sql;
    }
  }
}
