using System.Threading.Tasks;
using ApkPackageName.Models.DataTable;
using Microsoft.EntityFrameworkCore;

namespace ApkPackageName.Models.DbContext
{
  public class ApkNameContext : BaseContext
  {
    public ApkNameContext(DbContextOptions options) : base(options) { }

    public DbSet<ApkNameCacheTable> ApkNameCacheTable { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ApkNameCacheTable>().HasIndex(h => h.PackageName);
    }

    public async Task<int> ClearTable(string tableName)
    {
      return await Database.ExecuteSqlRawAsync($"TRUNCATE \"{tableName}\" RESTART IDENTITY;");
    }
  }
}
