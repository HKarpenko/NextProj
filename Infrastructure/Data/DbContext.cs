using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventOccurrence> EventsOccurrences { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventDayRecurrence> DayRecurrences { get; set; }
        public DbSet<EventDayRecurrence2DayPosition> DayRecurrences2DayPositions { get; set; }
        public DbSet<EventSubscription> EventSubscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}