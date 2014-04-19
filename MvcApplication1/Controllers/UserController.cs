using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using MvcApplication1.Models;
using MvcApplication1.Repositorys;
using WebMatrix.WebData;

namespace MvcApplication1.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Create()
        {
            return RedirectToAction("Index", "Calendar");
        }
        public ActionResult CreateUser(string name, string email, string password)
        {
            // skapar användaren utfrån vad användaren har matat in i Views/Register, sedan anropar vi metoden Createuser
            // i vårat UserRepository och skickar med inmatade informationen email och name.
            var user = new UserRepository();
            user.CreateUser(email, name); // Denna lägger till i tabellen Users i databasen
            WebSecurity.CreateAccount(email, password); // Denna lägger till i tabellen Membership i databasen
            WebSecurity.Login(email, password);

            return RedirectToAction("Index", "Calendar");
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost] // sätta post så det går att använda samma metodnamn...
        public ActionResult Login(string email, string password)
        {
            if (email == "" || password == "")
            {
                MessageBox.Show("Felaktig inloggning!");
            }
            else if (WebSecurity.Login(email, password))
            {
                return RedirectToAction("Index", "Calendar");
            }
            return View();
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "User");
        }

        public ActionResult ChangePassword(string email, string currentPassword, string newPassword)
        {
            if (WebSecurity.ChangePassword(email, currentPassword, newPassword))
            {
                MessageBox.Show("Lösenord ändrat!");
                return RedirectToAction("index", "calendar");
            }
            return View();
        }
        public ViewResult ResetPassword()
        {
            return View();
        }
		
		// Reset av lösenord
        [HttpPost]
        public ViewResult ResetPassword(string email) // emailadress som postats från vyn, se ResetPassword.cshtml
        {
			//Validera

			// om email finns 
			if (WebSecurity.UserExists(email) && email !="")
	        {
		        const string myemail = "hostresetpass@gmail.com";
			        // för att kunna använda en tredje part, för att skapa ett konto för hantering för mottagnde av epost
		        const string password = "c4l3nd4r";

		        var loginInfo = new NetworkCredential(myemail, password);
			        //  NetworkCredential fixar så att servern får tillgång till dina inloggningsuppgifter 
		        var msg = new MailMessage(); // repesenterar ett epostmeddelande som kan anävnda sig av Smtp klient
		        var smtpClient = new SmtpClient("smtp.gmail.com", 587);
			        // SMTP server inställningar med portnummer för att kunna skicka epost via Gmail från valfritt email

		        var token = WebSecurity.GeneratePasswordResetToken(email, 50);
			        //Genererar ett lösen till användarens email, när användaren klickar på länken som mailats till, då ska man komma till en sida för att skriva in nytt lösenord, 	returnerar ett säkert hashvärde, 50 min är giltighetstiden


		        if (Request.Url != null)
		        {
			        var hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
			        var confirmationUrl = hostUrl +
			                              VirtualPathUtility.ToAbsolute("~/User/newpassword?email=" + email + "&token=" + token);

			        msg.From = new MailAddress(myemail);
			        msg.To.Add(new MailAddress(email));
			        msg.Subject = "reset password";
			        msg.Body = "Ändra ditt lösenord > <a href=\"" + confirmationUrl + "\">" + confirmationUrl + "</a>";
		        }

		        msg.IsBodyHtml = true;
		        smtpClient.EnableSsl = true;
		        smtpClient.UseDefaultCredentials = false;
		        smtpClient.Credentials = loginInfo;
		        smtpClient.Send(msg);

		        if (ModelState.IsValid)
		        {
			        return View("EmailConfirmMessage");
		        }
	        }
	        // Om fel skicka, ge ett felmeddelande
            MessageBox.Show("Du har glömt fylla i fältet eller skrivit in en ogiltig Email!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return View();
        }

        public ActionResult NewPassword(string email, string token)
        {
            var model = new Month {Token = token};
            return View();
        }

        [HttpPost]
        public ActionResult Reset(string token, string newPassword)
        {
            if (WebSecurity.ResetPassword(token, newPassword))
            {
                MessageBox.Show("Lösenord ändrat!");
                return RedirectToAction("index", "calendar");
            }
            return RedirectToAction("Login");
        }
    }
}