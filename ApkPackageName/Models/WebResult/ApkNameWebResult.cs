using ApkPackageName.Models.DataTable;

namespace ApkPackageName.Models.WebResult
{
  public class ApkNameWebResult : WebResult<ApkName>
  {
    public ApkNameWebResult() { }
    public ApkNameWebResult(int code) : base(code) { }
    public ApkNameWebResult(ApkName name) : base(name) { }
  }
}
