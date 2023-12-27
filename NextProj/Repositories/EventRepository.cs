using Task0.Data;
using Task0.Models.Entities;

namespace Task0.Repositories
{
    public class EventRepository : IEventRepository
    {
        public EventRepository()
        {
            using (var context = new ApiContext())
            {
                var events = new List<Event>()
                {
                    new Event {
                        Name = "Dancing class",
                        AdditionalInfo = "Take switch shoes",
                        Category = "Dancing",
                        Description = "Dancing classes for Bachata",
                        Place = "Warsaw",
                        Time = DateTime.Now.AddDays(5),
                        Images = "https://i0.wp.com/danceclub.szczecin.pl/wp-content/uploads/2022/07/AdobeStock_310484173-scaled.jpeg?fit=2560%2C1709&ssl=1"
                    },
                    new Event {
                        Name = "Real Madrid vs FC Barselona",
                        AdditionalInfo = "First beer for free",
                        Category = "Football",
                        Description = "Watching live video of football match",
                        Place = "Warsaw",
                        Time = DateTime.Now.AddDays(7),
                        Images = "https://www.matthewclark.co.uk/media/6112/people-in-pub-watching-football.jpg?width=850&height=478&mode=crop&quality=75"
                    }
                };
                context.Events.AddRange(events);
                context.SaveChanges();
            }
        }

        public void AddEvent(Event eventModel)
        {
            using (var context = new ApiContext())
            {
                context.Events.Add(eventModel);
                context.SaveChanges();
            }
        }

        public void DeleteEvent(long id)
        {
            using (var context = new ApiContext())
            {
                var eventModel = context.Events.FirstOrDefault(e => e.Id == id);
                if (eventModel != null)
                {
                    context.Events.Remove(eventModel);
                    context.SaveChanges();
                }
            }
        }

        public List<Event> GetAllEvents()
        {
            using (var context = new ApiContext())
            {
                var list = context.Events.ToList();

                return list;
            };
        }

        public Event GetEventById(long id)
        {
            using (var context = new ApiContext())
            {
                var eventModel = context.Events.FirstOrDefault(e => e.Id == id);

                return eventModel;
            };
        }

        public void SaveEvent(Event eventModel)
        {
            using (var context = new ApiContext())
            {
                context.Events.Update(eventModel);
                context.SaveChanges();
            };
        }
    }
}
