using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UnipiLabs.DataAccess;
using UnipiLabs.Models;

namespace UnipiLabs.Controllers
{
    public class HomeController : Controller
    {

        // see https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/march/cutting-edge-a-first-look-at-asp-net-identity
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Index(string message)
        {

            //σε περίπτωση που ο χρήστης δεν είχε αποσυνδεθεί από την τελευταία του είσοδο, μετα΄φέρεται στην αρχική του σελίδα ανάλογα με την ιδιότητά του
            //διαφορετικά του εμφανίζεται η σελίδα του login για να κάνει είσοδο
            if (Request.IsAuthenticated)
            {
                var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
                var user = userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
                return redirectLoggedInUser(user);
            }
             
            ViewBag.Message = message;
            return View();
        }


        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(Users user)
        {
            //χρησιμοποιώντας το AspNet.Identity της Microsoft, ελέγχεται στη βάση δεδομένων εάν υπάρχει ο χρήστης και αν το ΑΜ΄και ο κωδικός ταιριάζουν

            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var signinmanager = new UsersSignInManager(userManager, AuthenticationManager);
            
            var loginresult = await signinmanager.PasswordSignInAsync(user.UserName, user.Password, true, false);

            //εάν η σύνδεση είναι επιτυχής, αποθηκευουμε τον χρήστη που συνδέθηκε και μεταφερόμαστε στην παρακάτω μέθοδο που ελέγχουμε την ιδίότητά του για να ανακατευθυνθεί στη σωστή σελίδα

            if (loginresult == SignInStatus.Success)
            {
                    var userId = signinmanager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId<int>(); 
                    var loggedInUser = await userManager.FindByIdAsync(userId);

                return redirectLoggedInUser(loggedInUser);  
            }

            //εάν τα στοιχεία είναι λάθος, ο χρήστης ανακατευθύνεται στην Index για να προσπαθήσει να κάνει login ξανά, λαμβάνοτας αντίστοιχο μήνυμα λάθους μέσω javascript alert

            return RedirectToAction("Index", "Home", new { message = "Το AM ή ο κωδικός πρόσβασής σας είναι λάθος. Ελέγξτε τα στοιχεία εισόδου." });

        }

        public ActionResult LogOut()
        {
            //όταν ο χρήστης αποσυνδεθεί από τον λογαριασμό του, "αδειάζει" το cookie και μεταφέρεται στην Index

            FormsAuthentication.SignOut();
            Request.GetOwinContext().Authentication.SignOut(); // clear the cookie
            return RedirectToAction("Index", "Home");
        }

        private ActionResult redirectLoggedInUser(Users loggedInUser)
        {
            //ανάλογα με την ιδιότητά του, ο χρήστης ανακατεύθυνεται στην αρχική του σελίδα κατά την είσοδό του

            if (loggedInUser.Role == "Student")
            {
                return RedirectToAction("MySubjectsStudent", "Students");
            }
            else if (loggedInUser.Role == "Professor")
            {
                return RedirectToAction("MySubjectsProfessor", "Professors");
            }
            else if (loggedInUser.Role == "LabAdmin")
            {
                return RedirectToAction("ViewLabAvailabilityAdmin", "LabAdmin");
            }
            else
            {
                throw new Exception("Unknown user role " + loggedInUser.Role);
            }
        }
    }
}