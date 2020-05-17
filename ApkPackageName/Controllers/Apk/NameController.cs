using System.Collections.Generic;
using System.Threading.Tasks;
using ApkPackageName.Models.WebResult;
using ApkPackageName.Utils;
using ApkPackageName.Utils.Dao;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApkPackageName.Controllers.Apk
{
  [Route("api/apk/name/")]
  [ApiController]
  [EnableCors]
  public class NameController : ControllerBase
  {
    private readonly ApkNameDao _dao;
    public NameController(ApkNameDao dao)
    {
      _dao = dao;
    }
    [HttpGet("{packageName}")]
    public async Task<ApkNameWebResult> Get(string packageName)
    {
      var name = await _dao.GetOrCreateApkName(packageName);
      if (name != null)
      {
        return new ApkNameWebResult(name);
      }
      return new ApkNameWebResult(1);
    }
  }
}
