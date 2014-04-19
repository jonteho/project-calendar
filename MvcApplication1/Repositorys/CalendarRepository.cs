using System.Collections.Generic;
using System.Data;
using System.Linq;
using MvcApplication1.Models;

namespace MvcApplication1.Repositorys
{
    public class CalendarRepository
    {
        // Repository för metoderna för events där vi anropar vårat context som har anslutning till våran databas
        
        public void Save(Events newEvent)
        {
            // Sparar det nya eventet
            using (var context = new ProjectCalendarDbContext())
            {
                context.Event.Add(newEvent);
                context.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            // Väljer det id som kommer in och tar sedan bort det
            using (var context = new ProjectCalendarDbContext())
            {
                var listEvent = context.Event.Single(events => events.Id == id);
                context.Event.Remove(listEvent);
                context.SaveChanges();
            }
        }
        public List<Events> GetAllByEmail(string email)
        {
            // Hämta specifik lista på events där användarens email matchar rätt email
            using (var context = new ProjectCalendarDbContext())
            {
                return context.Event.Where(item => item.User.Email == email).ToList();
            }
        }
        public Events Edit(int id = 0)
        {
            // Hitta rätt id på det events som man vill ändra
            using (var context = new ProjectCalendarDbContext())
            {
                var eventId = context.Event.Find(id);
                return eventId;
            }
        }
        public Events EditEvent(Events events)
        {
            using (var context = new ProjectCalendarDbContext())
            {
                context.Entry(events).State = EntityState.Modified;
                context.SaveChanges();
                return events;
            }
        }
        public List<Events> GetEventByDate()
        {
            // Hämta specifik lista på events där användarens email matchar rätt email
            using (var context = new ProjectCalendarDbContext())
            {
                return context.Event.ToList();
            }
        }
    }
}