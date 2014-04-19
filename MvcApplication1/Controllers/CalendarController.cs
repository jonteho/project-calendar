using System;
using System.Web.Mvc;
using MvcApplication1.Models;
using MvcApplication1.Repositorys;
using WebMatrix.WebData;

namespace MvcApplication1.Controllers
{
    public class CalendarController : Controller
    {
        [Authorize] // skydd, måste vara inloggad för att se...
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Month(int? year, int? month)
        {
            if (year == null && month == null)
            {
                var dateNow = DateTime.Now;
                return RedirectToAction("Month", new { year = dateNow.Year, month = dateNow.Month });
            }
            var listEvents = new CalendarRepository();
            var listByEmail = listEvents.GetAllByEmail(HttpContext.User.Identity.Name);
            var daysInMonth = new Month(year.Value, month.Value, listByEmail);

            return View(daysInMonth);
        }
        public ActionResult Week(int? year, int? month, int? dayWeek)
        {
            if (year == null && month == null && dayWeek == null)
            {
                var dateNow = DateTime.Now;
                return RedirectToAction("Week", new { year = dateNow.Year, month = dateNow.Month, dayWeek = dateNow.Day });
            }
            var date = new Week(year.Value, month.Value, dayWeek.Value);

            return View(date);
        }
        public ActionResult AddEvent(Events newEvent)
        {
            // För att skapa ett nytt event så anropar vi metoden Save i vårat repository där vi skickar med det nya eventet,
            // som innehåller: se Models.Event som i sin tur matas in av användaren i Views/Event
            var events = new CalendarRepository();
            newEvent.User_Id = WebSecurity.GetUserId(HttpContext.User.Identity.Name);
            events.Save(newEvent);
            return RedirectToAction("ShowEvent");
        }
        public ActionResult ShowEvent()
        {
            // För att visa en spcifik lista på events så anropar vi metoden GetAllByEmail där vi skickar med användarens Användarnamn
            var listEvents = new CalendarRepository();
            var list = listEvents.GetAllByEmail(HttpContext.User.Identity.Name);
            
            return View(list);
        }
        public ActionResult DeleteEvent(int id)
        {
            // Skickar med id på ett specifikt event till metoden Delete i vårat repository
            var events = new CalendarRepository();
            events.Delete(id);
            
            return RedirectToAction("Month");
        }
        // anropas när man ska hämta information alltså get
        public ActionResult EditEvent(int id = 0)
        {
            var eventId = new CalendarRepository();
            var newEvent = eventId.Edit(id);
            return View(newEvent);
        }
        [HttpPost]
        public ActionResult EditEvent(Events events)
        {
            var eventId = new CalendarRepository();
            eventId.EditEvent(events);
            return RedirectToAction("Month");
        }
    }
}
