using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcApplication1.Models;

namespace MvcApplication1
{
    public class ProjectCalendarDbContext : DbContext
    {
        // Koppling mellan anslutningen ProjectCalendarConnection som jobbar mot databasen.. se Global.asax connectionsStrings
        public ProjectCalendarDbContext() : base("ProjectCalendarConnection")
		{			
		}
		public DbSet<Events> Event { get; set; }
        public DbSet<User> Users { get; set; }
    }
}