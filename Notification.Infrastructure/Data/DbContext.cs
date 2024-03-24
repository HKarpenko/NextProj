using Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Notification.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<NotificationJob> NotificationJobs { get; set; }
    }
}
