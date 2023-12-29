using Microsoft.EntityFrameworkCore;
using NextProj.Models.Entities;

namespace NextProj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}