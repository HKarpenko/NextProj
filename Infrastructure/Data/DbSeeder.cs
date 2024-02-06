using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data
{
    public class DbSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SeedData(AppDbContext context)
        {
            if (context.Events.Any())
            {
                return;
            }

            var initialData = new List<Event>()
            {
                new Event {
                    Name = "Dancing class",
                    AdditionalInfo = "Take switch shoes",
                    Category = new Category { Name = "Dancing" },
                    Description = "Dancing classes for Bachata",
                    Images = "https://i0.wp.com/danceclub.szczecin.pl/wp-content/uploads/2022/07/AdobeStock_310484173-scaled.jpeg?fit=2560%2C1709&ssl=1",
                    Place = new Place
                    {
                        Country = "Poland",
                        City = "Warsaw",
                        Address = "Moliera str. 4/6",
                        PostalCode = "00-076"
                    },
                    Occurrences = new List<EventOccurrence>()
                    {
                        new EventOccurrence
                        {
                            Time = DateTime.Now.AddDays(5)
                        }
                    }
                },
                new Event {
                    Name = "Real Madrid vs FC Barselona",
                    AdditionalInfo = "First beer for free",
                    Category = new Category { Name = "Football" },
                    Description = "Watching live video of football match",
                    Images = "https://www.matthewclark.co.uk/media/6112/people-in-pub-watching-football.jpg?width=850&height=478&mode=crop&quality=75",
                    Place = new Place
                    {
                        Country = "Germany",
                        City = "Berlin",
                        Address = "Veteranenstraße 26",
                        PostalCode = "10119"
                    },
                    Occurrences = new List<EventOccurrence>()
                    {
                        new EventOccurrence
                        {
                            Time = DateTime.Now.AddDays(7),
                        }
                    }
                },
                new Event {
                    Name = "Standap show",
                    AdditionalInfo = "Only 18+ allowed",
                    Category = new Category { Name = "Standap" },
                    Description = "Enjoy comedic performance in which the performer addresses the audience directly from the stage",
                    Images = "https://i.guim.co.uk/img/media/ae71051df54f246e1d34fa9bcaea8f4e6fdedb9b/0_133_4000_2400/master/4000.jpg?width=1200&quality=85&auto=format&fit=max&s=12637660caff9bc8a7ef073b148bd6da",
                    Place = new Place
                    {
                        Country = "Ukraine",
                        City = "Kyiv",
                        Address = "Velyka Zhytomyrska 16",
                        PostalCode = "02000"
                    },
                    Occurrences = new List<EventOccurrence>()
                    {
                        new EventOccurrence
                        {
                            Time = DateTime.Now.AddDays(12),
                        }
                    }
                },
                new Event {
                    Name = "Cooking workshop",
                    AdditionalInfo = "Reserve 3 hours of your time",
                    Category = new Category { Name = "Cooking" },
                    Description = "During the Art of Cooking like a Chef workshop, you will learn all the techniques and tips to be able to cook like a skilled cuisine Chef in your own home.",
                    Images = "https://cdn-fkicp.nitrocdn.com/ZvQiQIyacNYIuXwXzPreuCTfVWTjbkvc/assets/images/optimized/rev-2f01176/supperclubyvr.com/wp-content/uploads/2021/11/ateliers-1024x666.jpg",
                    Place = new Place 
                    { 
                        Country = "Ukraine",
                        City = "Lviv",
                        Address = "Dzherelna 1", 
                        PostalCode = "79000"
                    },
                    Occurrences = new List<EventOccurrence>()
                    {
                        new EventOccurrence
                        {
                            Time = DateTime.Now.AddDays(6),
                        }
                    }
                },
                new Event {
                    Name = "Go-Karting around the lake",
                    AdditionalInfo = "Buy 10 minutes, get 3 minutes for free",
                    Category = new Category { Name = "Go-Karting" },
                    Description = "Go-karting is a great way to relieve stress and have fun. Go-karting is more than just a thrilling and exciting activity – it is also a great way to relieve stress and have fun",
                    Images = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/09/98/57/7a/gokarting-center.jpg?w=1200&h=-1&s=1",
                    Place = new Place { 
                        Country = "United Kingdom", 
                        City = "London", 
                        Address = "St James's Park", 
                        PostalCode = "SW1A 2BJ"
                    },
                    Occurrences = new List<EventOccurrence>()
                    {
                        new EventOccurrence
                        {
                            Time = DateTime.Now.AddDays(1),
                        }
                    }
                }
            };

            context.Events.AddRange(initialData);
            context.SaveChanges();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
                SeedData(dbContext);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}