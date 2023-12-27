using Microsoft.EntityFrameworkCore;
using Task0.Models.Entities;

namespace Task0.Data
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "Task0Db");
        }

        public DbSet<Event> Events { get; set; }
    }
}