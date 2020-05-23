using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UnipiLabs.DataAccess;
using UnipiLabs.Models;
using UnipiLabs.Controllers;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace UnipiLabs.Controllers
{
    public class LabAdminController : Controller
    {

        //σύνδεση με τη βάση

        private UnipiLabsDbContext db = new UnipiLabsDbContext();

        //GET
        [Authorize]
        public ActionResult ViewLabAvailabilityAdmin()
        {
            //αρχική σελίδα του διαχειριστή εργαστηρίων. Βλέπει όλα τα εργαστήρια που έχουν δημιουργηθεί

            return View(db.LabsAvailability.ToList());
            
        }

        //GET
        [Authorize]
        public ActionResult Details(string id)
        {
            //ανάλογα με το id της διαθεσιμότητας του εργαστηρίου, βλέπει τις αντίστοιχες πληροφορίες

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LabsAvailability labsAvailability = db.LabsAvailability.Find(id);

            if (labsAvailability == null)
            {
                return HttpNotFound();
            }
            return View(labsAvailability);
        }


        [Authorize]
        public ActionResult ViewUsersAdmin()
        {
            return View(db.Users.ToList()); //προβολή όλων των χρηστών
        }



        // GET
        [Authorize]
        public ActionResult EditUsersAdmin(int id)
        {
           //προβολή συγκεκριμένου χρήστη προς επεξεργασία

            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsersAdmin([Bind(Include = "UserName,Password,ConfirmPassword,Name,Surname,Email,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                //αποθήκευση νέων πληροφοριών χρήστη

                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewUsersAdmin"); //ανακατεύθυνση στην προβολή των χρηστών
            }
            return View(users);
        }

        // GET
        [Authorize]
        public ActionResult DeleteUsersAdmin(int id)
        {
            //προβολή συγκεκριμένου χρήστη προς διαγραφή

            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST
        [Authorize]
        [HttpPost, ActionName("DeleteUsersAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUsersAdminConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users); //διαγραφή συγκεκριμένου χρήστη 
            db.SaveChanges();
            return RedirectToAction("ViewUsersAdmin"); //ανακατεύθυνση στην προβολή χρηστών
        }

        // GET
        [Authorize]
        public ActionResult EditProfileAdmin(string message)
        {
            ViewBag.Message = message;
            //ο χρήστης που είναι συνδεδεμένος τη δεδομένη στιγμή, μπορεί να επεξεργαστεί τις πληροφορίες του προφίλ του

            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var userId = User.Identity.GetUserId<int>();
            var user = userManager.FindById(userId);
            return View(user);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfileAdmin([Bind(Include = "Id,PasswordHash,SecurityStamp,UserName,Password,ConfirmPassword,Name,Surname,Email,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                //αποθήκευση νέων πληροφοριών συνδεδεμένου χρήστη στη βάση δεδομένων, εάν ο ΑΜ σε συνδυασμό με την ιδιότητα του χρήστη είναι σωστά

                string firstLetter = users.UserName.Substring(0, 1);
                if (firstLetter == "L" && users.Role == "LabAdmin")
                {
                    db.Entry(users).State = EntityState.Modified; //αποθηκεύουμε στη βάση δεδομένων τις αλλαγές που κάναμε
                    db.SaveChanges();
                    return RedirectToAction("ViewLabAvailabilityAdmin");
                }
                else
                {
                    return RedirectToAction("EditProfileAdmin", "LabAdmin", new { message = "Η ιδιότητά σας ή ο ΑΜ σας είναι λάθος. Προσπαθήστε ξανά." });
                }
            }
            return View(users);
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
