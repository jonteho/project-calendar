using MvcApplication1.Models;

namespace MvcApplication1.Repositorys
{
    public class UserRepository
    {
        // Repository för metoderna för användare där vi anropar vårat context som har anslutning till våran databas

        public void CreateUser(string email, string name)
        {
            // Skapar ny användare utifrån parametrarna
            using ( var context = new ProjectCalendarDbContext())
            {
                context.Users.Add(new User { Email = email, Name = name});
                context.SaveChanges();
            }
        }
    }
}