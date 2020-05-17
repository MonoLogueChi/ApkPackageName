using ApkPackageName.Models.Config;
using ApkPackageName.Models.DbContext;

namespace ApkPackageName.Utils.Dao
{
    public class DbInitializer
    {
        public static void Initialize(ApkNameContext context, AppSettings appSettings)
        {
            context.Database.EnsureCreated();
        }
    }
}
