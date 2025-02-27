using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using TaskDbContext context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
            context.Database.Migrate();
        }
    }
}
