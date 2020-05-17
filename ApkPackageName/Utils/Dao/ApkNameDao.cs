using System.Linq;
using System.Threading.Tasks;
using ApkPackageName.Models.DataTable;
using ApkPackageName.Models.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ApkPackageName.Utils.Dao
{
  public class ApkNameDao
  {
    private readonly ApkNameContext _con;

    public ApkNameDao(ApkNameContext con)
    {
      _con = con;
    }

    public async Task<ApkName> GetOrCreateApkName(string packageName)
    {
      var a = _con.ApkNameCacheTable.Where(e => e.PackageName.Equals(packageName));

      if (await a.CountAsync() > 0) return await a.FirstOrDefaultAsync();

      var webCrawler = new WebCrawler();
      var apkName = await webCrawler.GetName(packageName);
      if (!string.IsNullOrEmpty(apkName))
      {
        var cacheTable = new ApkNameCacheTable
        {
          Name = apkName,
          PackageName = packageName
        };

        _con.ApkNameCacheTable.Update(cacheTable);
        await _con.SaveChangesAsync();
        return cacheTable;
      }

      return null;
    }
  }
}
