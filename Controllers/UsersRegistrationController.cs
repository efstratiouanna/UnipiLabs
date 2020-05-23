using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UnipiLabs.DataAccess;
using UnipiLabs.Models;

namespace UnipiLabs.Controllers
{
    public class UsersRegistrationController : Controller
    {
        private UnipiLabsDbContext db = new UnipiLabsDbContext();

       
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET
        public ActionResult Registration(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        // POST
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration([Bind(Include = "UserName,Password,ConfirmPassword,Name,Surname,Email,Role")] Users user)
        {
            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));

            

            if (ModelState.IsValid)
            {

                bool alreadyExists = db.Users.Any(m => m.UserName == user.UserName);

                if (alreadyExists == true)
                {
                    return RedirectToAction("Registration", "UsersRegistration", new { message = "Ο αριθμός μητρώου υπάρχει ήδη. Προσπαθήστε ξανά." });
                }
                    //έλεγχος γα το εάν ο χρήστης δίνει τη σωστή του ιδιότητα κατά την εγγραφή του
                    string firstLetter = user.UserName.Substring(0, 1);
                if (firstLetter == "P" && user.Role == "Professor")
                {
                    var result = await userManager.CreateAsync(user, user.Password);
                    if (result.Succeeded)
                    {
                        db.SaveChanges();
                        // Log the user in
                        var signInManager = new UsersSignInManager(userManager, AuthenticationManager);
                        await signInManager.SignInAsync(user, true, true);
                        return RedirectToAction("MySubjectsProfessor", "Professors");
                    }
                }
                else if (firstLetter == "S" && user.Role == "Student")
                {
                    var result = await userManager.CreateAsync(user, user.Password);
                    if (result.Succeeded)
                    {
                        db.SaveChanges();
                        // Log the user in
                        var signInManager = new UsersSignInManager(userManager, AuthenticationManager);
                        await signInManager.SignInAsync(user, true, true);
                        return RedirectToAction("MySubjectsStudent", "Students");
                    }      
                }
                else if (firstLetter == "L" && user.Role == "LabAdmin")
                {
                    var result = await userManager.CreateAsync(user, user.Password);
                    if (result.Succeeded)
                    {
                        db.SaveChanges();
                        // Log the user in
                        var signInManager = new UsersSignInManager(userManager, AuthenticationManager);
                        await signInManager.SignInAsync(user, true, true);
                        return RedirectToAction("ViewLabAvailability", "LabAdmin");
                    }      
                }
                else
                {
                    return RedirectToAction("Registration", "UsersRegistration", new { message = "Η ιδιότητά σας ή ο ΑΜ σας είναι λάθος. Προσπαθήστε ξανά." });
                }


            }
            return View(user);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
