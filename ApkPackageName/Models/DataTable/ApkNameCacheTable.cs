using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApkPackageName.Models.DataTable
{
  public class ApkNameCacheTable : ApkName
  {
    [Key]
    [Column("Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    ///   生成时间 UTC
    /// </summary>
    [Column(TypeName = "timestamp(3)")]
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///   修改时间 UTC
    /// </summary>
    [Column(TypeName = "timestamp(3)")]
    public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
  }

  public class ApkName
  {
    /// <summary>
    ///   包名
    /// </summary>
    [Required]
    public string PackageName { get; set; }

    /// <summary>
    ///   应用名
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    ///   备注
    /// </summary>
    public string Remarks { get; set; }
  }
}
