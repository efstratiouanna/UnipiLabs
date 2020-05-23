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
using System.Web.UI.WebControls;
using UnipiLabs.Hubs;
using Microsoft.AspNet.SignalR;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace UnipiLabs.Controllers
{
    public class StudentsController : Controller
    {
        private UnipiLabsDbContext db = new UnipiLabsDbContext(); //σύνδεση με τη βάση δεδομένων

        // GET
        [Authorize]
        public ActionResult MySubjectsStudent()
        {
            //παίρνουμε τα στοιχεία του χρήστη με τη βοήθεια του AspNet Identity της Microsoft
            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var user = userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());

            return View(db.Subjects.Where(t => t.regStudent.Contains(user.UserName))); //ο χρήστης μεταβαίνει στην αρχική του σελίδα που αποτελείται από τα μαθήματα στα οποία είναι εγγεγραμμένος
        }

        [Authorize]
        public ActionResult RegisterToSubjectStudent(string subjectID, string subjectTitle)
        {
            //εγγραφή φοιτητή σε μάθημα

            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var userId = User.Identity.GetUserId<int>();
            var user = userManager.FindById(userId);


            Subjects subject = db.Subjects.Find(subjectID); //βρίσκουμε το id του συγκεκριμένου μαθήματος
            string prevValue = subject.regStudent; //βρίσκουμε τους ήδη εγγεγραμμένους φοιτητές σε αυτό το μάθημα

            if (prevValue == null)
            {
                subject.regStudent = user.UserName; //αν δεν υπάρχουν εγγεγραμμένοι φοιτητές, βάζουμε κατευθείαν ως πρώτο φοιτήτή τον συνδεδεμένο χρήστη
            }
            else
            {
                subject.regStudent = prevValue + "," + user.UserName; //αν υπάρχουν κι ΄΄αλλοι εγγεγραμμένοι φοιτητές, προσθέτουμε πρώτα εκείνους και μετά τον συνδεδεμένο φοιτητή, 
            }                                                           //χωρίζοντάς του με ένα κόμμα

            db.SaveChanges();

            return RedirectToAction("MySubjectsStudent"); //ανακατεύθυνση στην αρχική σελίδα
        }

        [Authorize]
        public ActionResult SubjectDetailsStudent(string subjectID)
        {
            if (subjectID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjects subjects = db.Subjects.Find(subjectID); //βρίσκουμε το id του συγκεκριμένου μαθήματος και βλέπουμε τις πληροφορίες του
            if (subjects == null)
            {
                return HttpNotFound();
            }
            return View(subjects);
        }


        // GET
        [Authorize]
        public ActionResult UnregisterFromSubjectStudent(string subjectID)
        {


            //παίρνουμε τα στοιχεία του χρήστη με τη βοήθεια του AspNet Identity της Microsoft
            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var user = userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());

            Subjects subject = db.Subjects.Find(subjectID);

            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Students/UnregisterFromSubjectStudent
        [HttpPost, ActionName("UnregisterFromSubjectStudent")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult UnregisterFromSubjectStudentConfirmed(string subjectID)
        {
            //παίρνουμε τα στοιχεία του χρήστη με τη βοήθεια του AspNet Identity της Microsoft
            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var user = userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());

            Subjects subject = db.Subjects.Find(subjectID);


            string regStudents = subject.regStudent;



            char[] separator = { ',', ' ' }; //ορίζουμε ως διαχωριστικό το ,

            // ορίζουμε τη λίστα που θα περιέχει τους εγγεγραμμένους φοιτητές

            List<string> regStudentsList = new List<string>();


            string[] registeredStudents = regStudents.Split(separator); //διαχωρίζουμε το string βάσει του κόμματος, χρησιμοποιώντας τη μέθοδο Split, και τα προσθέτουμε σε μια λίστα από strings


            //προσθέτουμε κάθε ΑΜ στη λίστα των φοιτητών που ορίσαμε παραπάνω
            foreach (var item in registeredStudents)
            {
                regStudentsList.Add(item);
            }

            //αν ο συνδεδεμένος χρήστης ανήκει στους εγγεγραμμένος φοιτητές, τότε τον αφαιρούμε από τη λίστα
            if (regStudentsList.Contains(user.UserName))
            {
                regStudentsList.Remove(user.UserName);
            }
            else
            {
                return HttpNotFound();
            }

            //ορίζουμε το string που θα περιέχει όλους τους υπόλοιπους φοιτητές που είναι εγγεγραμμένοι στο μάθημα
            string newRegStudents = "";
            foreach (var item in regStudentsList)
            {
                if (newRegStudents == "")
                {
                    newRegStudents = item;
                }
                else
                {
                    newRegStudents = newRegStudents + "," + item;
                }

            }
            subject.regStudent = newRegStudents; //αποθηκεύουμε το νέο string στη βάση δεδομένων
            db.SaveChanges();

            return RedirectToAction("MySubjectsStudent");
        }

        [Authorize]
        public ActionResult MyLabsStudent()
        {
            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var userId = User.Identity.GetUserId<int>();
            var user = userManager.FindById(userId);

            string userID = user.UserName;

            return View(db.Enrollments.Where(t => t.teamMembers.Contains(user.UserName))); //εμφανίζουμε τιςεγγραφές στα εργαστήρια στα οποία εχει εγγραφεί ο συνδεδεμένος χρήστης
        }

        [Authorize]
        public ActionResult MyGradesStudent()
        {
            var userID = User.Identity.GetUserId<int>();
            var user = db.Users.Find(userID);

            return View(db.Enrollments.Where(t => t.teamMembers.Contains(user.UserName))); //εμφανίζουμε τιςεγγραφές στα εργαστήρια στα οποία εχει εγγραφεί ο συνδεδεμένος χρήστης
        }

       

        [Authorize]
        public ActionResult AllSubjectsStudent()
        {
            var userID = User.Identity.GetUserId<int>();
            var user = db.Users.Find(userID);

            return View(db.Subjects.Where(t => !t.regStudent.Contains(user.UserName) || t.regStudent == null)); //εμφανίζουμε όλα τα μαθήματα
        }

        [Authorize]
        public ActionResult AvailableSlotsStudent(string labID, string subID)
        {
            ViewData["subjectExamed"] = subID;
            Subjects subject = db.Subjects.Find(subID);
            ViewData["subjectTitle"] = subject.subjectTitle;
            return View(db.ExamSlotsAvailability.ToList().Where(m => m.labID == labID && m.available == true)); //εμφανίζουμε όλα τα διαθέσιμα slot στα οποία μπορεί να εγγραφεί ο φοιτητής
        }

        [Authorize]
        public ActionResult LabDetailsStudent(int primKeyID)
        {
            if (primKeyID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Enrollments enrollment = db.Enrollments.Find(primKeyID); //βρίσκουμε τη συγκεκριμένη εγγραφή και εμφανίζουμε στον χρήστη τις πληροφορίες του εργαστηρίου του

            return View(enrollment);
        }


        // GET
        [Authorize]
        public ActionResult AddMyTeamStudent(string labID, int primKeyID, string subID)
        {

            //παίρνουμε όλες τις πληροφορίες που χρειάζεται ο φοιτητής να γνωρίζει για το εργαστήριο του συγκεκριμένου μαθήματος και τις μεταφέρουμε στο αντίστοιχο View με ViewData

            LabsAnnouncements lab = db.LabsAnnouncements.Find(labID);
            int numberOfTeamMembers = lab.numberOfTeamMembers;
            ViewBag.numberOfTeamMembers = numberOfTeamMembers - 1;
            ViewData["labID"] = labID;
            ViewData["labTitle"] = lab.labTitle;
            ViewData["labDate"] = lab.BeginDateTime.Date.ToShortDateString();
            ViewData["labTime"] = lab.BeginDateTime.ToShortTimeString() + " - " + lab.EndDateTime.ToShortTimeString();

            Subjects subject = db.Subjects.Find(subID);
            ViewData["subID"] = subID;
            ViewData["subjectTitle"] = subject.subjectTitle;

            ExamSlotsAvailability slotAvailability = db.ExamSlotsAvailability.Find(primKeyID);
            ViewData["teamID"] = slotAvailability.registeredTeamsNumber + 1;
            ViewData["selectedSlot"] = slotAvailability.slot;

            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMyTeamStudent([Bind(Include = "primKeyID, subjectID, labID, teamID, selectedExamSlot, team, teamMembers, subjectTitle, labTitle, labDate, labTime")] Enrollments enrollments, int primKeyID, string labID, string subID, List<string> team)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>(); //δημιουργία μεταβλητής για σύνδεση με το hub που χρησιμοποιούμε για τις ειδοποιήσεις SignalR


            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var user = userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());

            string userID = user.UserName;

            LabsAnnouncements lab = db.LabsAnnouncements.Find(labID);


            //προσθέτουμε τα μέλη της ομάδας στη βάση δεδομένων στα Enrollments και στα Subjects
            enrollments.teamMembers = enrollments.team[0];

            for (int i = 1; i <= enrollments.team.Count() - 1; i++)
            {
                enrollments.teamMembers = enrollments.teamMembers + "," + enrollments.team[i];
            }


            if (lab.regStudents == null)
            {
                lab.regStudents = enrollments.teamMembers;

            }
            else
            {
                lab.regStudents = lab.regStudents + "," + enrollments.teamMembers;

            }
            db.Entry(lab).State = EntityState.Modified;
            db.SaveChanges();


            ExamSlotsAvailability availabilitySlot = db.ExamSlotsAvailability.Find(primKeyID);



            availabilitySlot.registeredTeamsNumber = enrollments.teamID; //ο αριθμός των εγγεγραμμένων ομάδων ισούται με το team ID της ομάδας ξεχωριστά σε κάθε slot

            LabsAvailability availability = db.LabsAvailability.Find(labID);



            if (availabilitySlot.registeredTeamsNumber >= availability.availableComputers)
            {
                availabilitySlot.available = false; //εάν ο αριθμός των εγγεγραμμένων ομάδων είναι >= του αριθμού των διαθέσιμων υπολογιστών του εργαστηρίου, τότε δεν ελιναι πλέον διαθέσιμο
            }

            enrollments.primKeyID = primKeyID;


            if (ModelState.IsValid)
            {

                team.Remove(userID); //αφαιρούμε τον συνδεδεμένο χρήστη από την ομάδα για να μην πάει και σε εκείνον ειδοποίηση ότι γράφτηκε στην ομάδα του, παρά μόνο στα υπόλοιπα
                                     //μέλη της ομάδας του

                //σε κάθε μέλος της ομάδας, μέσω του hub πο δημιουργήσαμε προηγουμένως, πηγαίνει ειδοποίηση ότι ο συνδεδεμένος χρήστης τους έγραψε στην ομάδα του στο συγκεκριμένο εργαστήριο
                hubContext.Clients.Users(team).addNewMessageToPage("Ο χρήστης με ΑΜ " + userID + " σας πρόσθεσε στην ομάδα του στο εργαστήριο με τίτλο " + lab.labTitle);

                //προσθέτουμε τα μέλη της ομάδας στους εγγεγραμμένους φοιτητές του εργαστηρίου
                LabsAnnouncements labRegistered = db.LabsAnnouncements.Find(labID);
                if (labRegistered.regStudents == null)
                {
                    labRegistered.regStudents = enrollments.teamMembers;
                }
                else
                {
                    labRegistered.regStudents = labRegistered.regStudents + enrollments.teamMembers;
                }
                

                db.Enrollments.Add(enrollments); //αποθήκευση εγγραφής στη βάση δεδομένων
                db.SaveChanges();


                return RedirectToAction("MyLabsStudent", enrollments); //ανακατεύθυνση στα εργαστήρια στα οποία συμμετέχει ο συνδεδεμένος φοιτητής

            }


            return View(enrollments);
        }


        // GET
        [Authorize]
        public ActionResult EditProfileStudent(string message)
        {
            ViewBag.Message = message;
            //χρησιμοποιώντας το AspNet Identity της Microsoft, παίρνουμε τα στοιχεία του συνδεδεμενου χρήστη και μεταβαίνει στην αντίστοιχη σελίδα για να επεξεργαστεί το προφίλ του
            Users users = db.Users.Find(User.Identity.GetUserId<int>());
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfileStudent([Bind(Include = "Id,PasswordHash,SecurityStamp,UserName,Password,ConfirmPassword,Name,Surname,Email,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                string firstLetter = users.UserName.Substring(0, 1);
                if (firstLetter == "S" && users.Role == "Student")
                {
                    db.Entry(users).State = EntityState.Modified; //αποθηκεύουμε στη βάση δεδομένων τις αλλαγές που κάναμε
                    db.SaveChanges();
                    return RedirectToAction("MySubjectsStudent");
                }
                else
                {
                    return RedirectToAction("EditProfileStudent", "Students", new { message = "Η ιδιότητά σας ή ο ΑΜ σας είναι λάθος. Προσπαθήστε ξανά." });
                }
            }
            return View(users);
        }


        public JsonResult GetEvents()
        {
            var events = db.Events.ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public ActionResult LabsCalendar()
        {

            return View();
        }

    }

}
