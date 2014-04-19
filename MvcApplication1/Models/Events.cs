using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication1.Models
{
    public class Events
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }

        [ForeignKey("User_Id")]
        public User User { get; set; }
        public int? User_Id { get; set; }

        public Events()
        {
            DateString = Date.ToShortDateString();
        }
    }
}