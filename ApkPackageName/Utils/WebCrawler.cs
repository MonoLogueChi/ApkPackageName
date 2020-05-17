using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace ApkPackageName.Utils
{
  public class WebCrawler
  {
    private readonly HtmlWeb _web = new HtmlWeb();

    public async Task<string> GetName(string packageName)
    {
      return await GetNameByCoolApk(packageName) ??
             await GetNameByQq(packageName) ??
             await GetNameByMi(packageName) ??
             await GetNameByMeiZu(packageName) ??
             await GetNameByPlayStore(packageName);
    }

    public async Task<string> GetName(ApplicationMarket market, string packageName)
    {
      string name = null;
      try
      {
        switch (market)
        {
          case ApplicationMarket.CoolApk:
            name = await GetNameByCoolApk(packageName);
            break;
          case ApplicationMarket.Qq:
            name = await GetNameByQq(packageName);
            break;
          case ApplicationMarket.Mi:
            name = await GetNameByMi(packageName);
            break;
          case ApplicationMarket.MeiZu:
            name = await GetNameByMeiZu(packageName);
            break;
          case ApplicationMarket.PlayStore:
            name = await GetNameByPlayStore(packageName);
            break;
        }
      }
      catch
      {
        // ignored
      }

      return name;
    }

    /// <summary>
    ///   从酷安爬取数据
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    private async Task<string> GetNameByCoolApk(string packageName)
    {
      var htmlDoc = await _web.LoadFromWebAsync($"https://www.coolapk.com/apk/{packageName}");
      var detailAppTitle = htmlDoc?.DocumentNode.SelectSingleNode("//p[@class='detail_app_title']");
      detailAppTitle?.RemoveChild(detailAppTitle.LastChild);
      return HttpUtility.HtmlDecode(string.IsNullOrWhiteSpace(detailAppTitle?.InnerText)
          ? null
          : detailAppTitle?.InnerText);
    }

    /// <summary>
    ///   从应用宝爬取数据
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    private async Task<string> GetNameByQq(string packageName)
    {
      var htmlDoc = await _web.LoadFromWebAsync($"https://sj.qq.com/myapp/detail.htm?apkName={packageName}");

      var detailAppTitle = htmlDoc?.DocumentNode.SelectSingleNode("//div[@class='det-name-int']");
      return HttpUtility.HtmlDecode(string.IsNullOrWhiteSpace(detailAppTitle?.InnerText)
          ? null
          : detailAppTitle?.InnerText);
    }

    /// <summary>
    ///   从小米应用商店爬取数据
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    private async Task<string> GetNameByMi(string packageName)
    {
      var htmlDoc = await _web.LoadFromWebAsync($"http://app.mi.com/details?id={packageName}");

      var detailAppTitle = htmlDoc?.DocumentNode.SelectSingleNode("//div[@class='intro-titles']/h3");
      return HttpUtility.HtmlDecode(string.IsNullOrWhiteSpace(detailAppTitle?.InnerText)
          ? null
          : detailAppTitle?.InnerText);
    }

    /// <summary>
    ///   从魅族商店爬取数据
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    private async Task<string> GetNameByMeiZu(string packageName)
    {
      var htmlDoc = await _web.LoadFromWebAsync($"http://app.meizu.com/apps/public/detail?package_name={packageName}");

      var detailAppTitle = htmlDoc?.DocumentNode.SelectSingleNode("//div[@class='detail_top']/h3");
      return HttpUtility.HtmlDecode(string.IsNullOrWhiteSpace(detailAppTitle?.InnerText)
          ? null
          : detailAppTitle?.InnerText);
    }

    /// <summary>
    ///   从PlayStore爬取数据
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    private async Task<string> GetNameByPlayStore(string packageName)
    {
      var htmlDoc = await _web.LoadFromWebAsync($"https://play.google.com/store/apps/details?id={packageName}");

      var detailAppTitle = htmlDoc?.DocumentNode.SelectSingleNode("//h1[@class='AHFaub']/span");
      return HttpUtility.HtmlDecode(string.IsNullOrWhiteSpace(detailAppTitle?.InnerText)
          ? null
          : detailAppTitle?.InnerText);
    }
  }

  public enum ApplicationMarket
  {
    CoolApk,
    Qq,
    Mi,
    MeiZu,
    PlayStore
  }
}
