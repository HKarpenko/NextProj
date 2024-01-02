using Microsoft.EntityFrameworkCore;
using NextProj.Data;

namespace NextProj.Helpers
{
    public static class MigrationHelper
    {
        public static void RunMigrations(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }

}
