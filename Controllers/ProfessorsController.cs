using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Windows.Documents;
using UnipiLabs.ActionFilters;
using UnipiLabs.DataAccess;
using UnipiLabs.Models;
using UnipiLabs.ViewModels;

namespace UnipiLabs.Controllers
{
    public class ProfessorsController : Controller
    {
        private UnipiLabsDbContext db = new UnipiLabsDbContext(); //σύνδεση με τη βάση δεδομένων

        // GET
        [Authorize]
        public async Task<ActionResult> MySubjectsProfessor()
        {
            //παίρνουμε τα στοιχεία του χρήστη που συνδέθηκε 

            var userManager = new UnipiUsersManager(new CustomUserStore(db));
            var userId = User.Identity.GetUserId<int>();
            var user = await userManager.FindByIdAsync(userId);

            string professor = user.Name + " " + user.Surname;
            return View(db.Subjects.Where(t => t.professorTeaching == professor)); //και εμφανίζουμε τα μαθήματα που διδάσκει ο συγκεκριμένος καθηγητής
        }

        // GET
        [Authorize]
        public async Task<ActionResult> CreateNewSubjectProfessor(string message)
        {
            //παίρνουμε τα στοιχεία του χρήστη που συνδέθηκε 

            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var userId = User.Identity.GetUserId<int>();
            var user = await userManager.FindByIdAsync(userId);

            ViewData["professor"] = user.Name +" "+ user.Surname; //UserName του συνδεδεμένου καθηγητή το οποίο μπαίνει αυτόματα στο πλαίσιο του διδάσκοντα 
                                                                  //και δεν μπορεί να επεξεργαστεί από τον χρήστη
            ViewBag.Message = message;
            return View();
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewSubjectProfessor([Bind(Include = "subjectID,subjectTitle,subjectSemester,subjectDescription,regStudent,professorTeaching")] Subjects subjects)
        {
            if (ModelState.IsValid)
            {
                bool alreadyExists = db.Subjects.Any(m => m.subjectID == subjects.subjectID);
                if (alreadyExists == true)
                {
                    return RedirectToAction("CreateNewSubjectProfessor", new { message = "Ο κωδικός μαθήματος υπάρχει ήδη. Προσπαθήστε ξανά."});
                }
                db.Subjects.Add(subjects); //προσθήκη νεόυ μαθήματος στη βάση δεδομένων
                db.SaveChanges();
                return RedirectToAction("MySubjectsProfessor"); //ανακατεύθυνση στην αρχική σελίδα
            }

            return View(subjects);
        }

        // GET
        [Authorize]
        public ActionResult EditSubjectProfessor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Subjects subjects = db.Subjects.Find(id); //βρίσκουμε το συγκεκριμένο μάθημα πο θέλουμε να επεξεργαστούμε

            ViewData["subjectSemester"] = subjects.subjectSemester;

            if (subjects == null)
            {
                return HttpNotFound();
            }
            return View(subjects);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubjectProfessor([Bind(Include = "subjectID,subjectTitle,subjectSemester,subjectDescription,regStudent,professorTeaching")] Subjects subjects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjects).State = EntityState.Modified; //αποθηκεύουμε στη βάση δεδομένων τις αλλαγές που κάναμε στο συγκεκριμένο μάθημα
                db.SaveChanges();
                return RedirectToAction("MySubjectsProfessor"); //ανακατεύθυνση στην αρχική σελίδα
            }
            return View(subjects);
        }

        // GET
        [Authorize]
        public ActionResult DeleteSubjectProfessor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Subjects subjects = db.Subjects.Find(id); //βρίσκουμε το συγκεκριμένο μάθημα πο θέλουμε να διαγράψουμε

            if (subjects == null)
            {
                return HttpNotFound();
            }
            return View(subjects);
        }

        // POST
        [Authorize]
        [HttpPost, ActionName("DeleteSubjectProfessor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubjectProfessorConfirmed(string id)
        {
            Subjects subjects = db.Subjects.Find(id);
            db.Subjects.Remove(subjects); //αφαιρούμε το συγκεκριμένο μάθημα από τη βάση δεδομένων

            List<LabsAnnouncements> lab = db.LabsAnnouncements.Where(m => m.subjectExamed == id).ToList();

            foreach (var item in lab)
            {
                List<ExamSlotsAvailability> slots = db.ExamSlotsAvailability.Where(m => m.labID == item.labID).ToList();

                foreach (var item2 in slots)
                {
                    db.ExamSlotsAvailability.Remove(item2);
                }
                db.LabsAnnouncements.Remove(item);
            }

            List<LabsAvailability> labAvailability = db.LabsAvailability.Where(m => m.subjectExamed == id).ToList();

            foreach (var item in labAvailability)
            {
                db.LabsAvailability.Remove(item);
            }


            List<Events> ev = db.Events.Where(m => m.subID == id).ToList();

            foreach (var item in ev)
            {
                db.Events.Remove(item);
            }

            List<Enrollments> enrollments = db.Enrollments.Where(m => m.subjectID == id).ToList();

            foreach (var item in enrollments)
            {
                db.Enrollments.Remove(item);
            }

            db.SaveChanges();
            return RedirectToAction("MySubjectsProfessor"); //ανακατεύθυνση στην αρχική σελίδα
        }


        [Authorize]
        public ActionResult CreateSubjectLabProfessor( string message, /*string labID,*/ string subID/*, DateTime beginDateTime, DateTime endDateTime*/)
        {
            
            Subjects subject = db.Subjects.Find(subID); //βρίσκουμε το συγκεκριμένο μάθημα

            LabsAnnouncements lab = new LabsAnnouncements();

            //παίρνουμε την έναρξη και τη λήξη του εργαστηρίου καθώς και τον κωδικό του μαθήματος που αφορά το εργαστήριο

            lab.subjectExamed = subject.subjectTitle;

            int numberOfLabs = db.LabsAnnouncements.Count(); //μετράει τις συνολικές εγγραφές της διαθεσιμότητας των εργαστηρίων
            ViewBag.labNumber = numberOfLabs + 1; //κατά σειρά αριθμός του εργαστηριου προς δημιουργία 

            //μεταφορά μεταβλητών στο αντίστοιχο View μέσω ViewData

            ViewData["subjectTitle"] = subject.subjectTitle;
            ViewData["message"] = message;
            return View(lab);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubjectLabProfessor([Bind(Include = "labID,labTitle,subjectExamed,labDate,labTime,start,end,BeginDateTime,EndDateTime,RegistrationDeadline,examSlotsDuration,numberOfTeamMembers")] LabsAnnouncements labsAnnouncements, string subID)
        {
            Subjects subject = db.Subjects.Find(subID); //βρίσκουμε το συγκεκριμένο μάθημα

            ViewData["subjectTitle"] = subject.subjectTitle; 

            string labDate = labsAnnouncements.labDate.ToShortDateString(); //μετατρέπουμε την ημερομηνία του εργαστηρίου που όρισε ο καθηγητής σε string


            string time = labsAnnouncements.labTime; //παιρνάμε την ώρα του εργαστηρίου που επέλεξε ο καθηγητής σε μια string μεταβλητή

            string message = "";

            bool isTaken = db.LabsAnnouncements.Any(m => m.labDate == labsAnnouncements.labDate && m.labTime == labsAnnouncements.labTime);
            if (isTaken)
            {
                message = "Η ώρα αυτή δεν είναι διαθέσιμη. Επιλέξτε άλλη ώρα ή άλλη ημέρα.";
                return RedirectToAction("CreateSubjectLabProfessor", new { subID, message }); //ανακατεύθυνση στα εργαστήρια του συγκεκριμένου μαθήματος
            }
            else
            {
                char[] separator1 = { '-', ' ' }; //ορίζουμε ως διαχωριστικό των ωρών την παύλα

                string[] TimeList = time.Split(separator1); //διαχωρίζουμε το string με τις ώρες με τη μέθοδο Split

                string beginTime = TimeList[0]; //ορίζουμε την ώρα έναρξης του εργαστηρίου
                string endTime = TimeList[1]; //οριζουμε την ώρα λήξης του εργαστηρίου


                //μετατρέπουμε τις παραπάνω ώρες σε timespan ώστε να είναι έγκυρες στην μετατροπή που θα γίνει παρακάτω
                TimeSpan beginTimeSpan = TimeSpan.Parse(beginTime);
                TimeSpan endTimeSpan = TimeSpan.Parse(endTime);


                //ορισμός τελικών dateTime που αφορούν την πλήρη ημερομηνία και ώρα εναρξης και λήξης του εργαστηρίου αντίστοιχα
                DateTime finalBeginDateTime = DateTime.ParseExact(labDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                .Add(beginTimeSpan);


                DateTime finalEndDateTime = DateTime.ParseExact(labDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                .Add(endTimeSpan);

                //παιρνάμε τα παραπάνω δεδομένα στο αντικείμενο labsAnnouncements
                labsAnnouncements.BeginDateTime = finalBeginDateTime;
                labsAnnouncements.EndDateTime = finalEndDateTime;

                //μετράμε πόσα εργαστήρια έχουν δημιουργηθεί στη βάση δεδομένων και προσθέτουμε 1 για να ορίσουμε τον κωδικό του εργαστηρίου που θα ορίσουμε αυτή τη στιγμή
                int numberOfLabs = db.LabsAnnouncements.Count();
                int labNumber = numberOfLabs + 1;
                labsAnnouncements.labID = "LAB" + labNumber;



                if (ModelState.IsValid)
                {
                    labsAnnouncements.subjectExamed = subID;

                    //διαμορφώνουμε τον τίτλο του εργαστηρίου ώστε να περιέχει και τον τίτλο του μαθήματος και τον τίτλο του εργαστηρίου
                    labsAnnouncements.labTitle = subject.subjectTitle + "-" + labsAnnouncements.labTitle;


                    DateTime startDateTime = labsAnnouncements.BeginDateTime; //παίρνουμε την ημερομηνία και έναρξης του εργαστηρίου
                    DateTime endDateTime = labsAnnouncements.EndDateTime; //παίρνουμε την ημερομηνία και λήξης του εργαστηρίου
                    double duration = labsAnnouncements.examSlotsDuration; //παίρνουμε τη διάρκεια του εργαστηρίου

                    //ορίζουμε τις μεταβλητές morning και afternoon για να τις χρησιμοποιήσουμε στη δημιουργία των slots
                    string morning = "";
                    string afternoon = "";
                    int numberOfSlots = 0; //αρχικοποίηση αριθμού slots

                    while (true)
                    {
                        DateTime dtNext = startDateTime.AddMinutes(duration); //αρχικοποιούμε τη μεταβλητή dtNext ορίζοντας την ώρα έναρξης του εργαστηρίου + τη χρονική διάρκεια του εργαστηρίου
                        if (startDateTime > endDateTime || dtNext > endDateTime)
                            break;
                        if (startDateTime < DateTime.Parse("12:00 PM")) //εάν η ώρα είναι πρωινή, τότε την προσθέτουμε στο string morning
                        {
                            morning += startDateTime.ToShortTimeString() + "-" + dtNext.ToShortTimeString() + ","; //ώρα έναρξης slot - ώρα λήξης slot

                        }
                        else //εάν η ώρα δεν είναι πρωινή, τότε την προσθέτουμε στο string aternoon
                        {
                            afternoon += startDateTime.ToShortTimeString() + "-" + dtNext.ToShortTimeString() + ","; //ώρα έναρξης slot - ώρα λήξης slot
                        }
                        startDateTime = dtNext; //για το επόμενο slot βάζουμε στην ώρα έναρξης την ώρα λήξης του προηγούμενου slot
                        numberOfSlots = numberOfSlots + 1; //αυξάνουμε τον αριθμό των slots κατά 1
                    }

                    if (morning.Length > 0)
                        morning = " " + morning;
                    if (afternoon.Length > 0)
                        afternoon = " " + afternoon;
                    labsAnnouncements.ExamSlots = morning + afternoon; //παιρνάμε στην αντίστοιχη στήλη του εργαστηρίου όλα τα slots που δημιουργήσαμε



                    // παιρνάμε το string με τα slots σε ένα άλλο string ώστε να τα διαχωρίσουμε
                    string ExamSlots = morning + afternoon;

                    char[] separator = { ',', ' ' }; //ορίζουμε ως διαχωριστικό το ,

                    // ορίζουμε την τελική λίστα που θα περιέχει τα slot του εργαστηρίου

                    List<string> finalList = new List<string>();


                    string[] ExamSlotsList = ExamSlots.Split(separator); //διαχωρίζουμε το string βάσει του κόμματος, χρησιμοποιώντας τη μέθοδο Split, και τα προσθέτουμε σε μια λίστα από string

                    foreach (var item in ExamSlotsList)
                    {
                        finalList.Add(item); //τέλος, γεμίζουμε την τελική λίστα που ορίσαμε παραπάνω με κάθε slot
                    }

                    //ViewBag.ExamSlotsList = ExamSlotsList;


                    ExamSlotsAvailability slots = new ExamSlotsAvailability();

                    foreach (var item in finalList.ToList())
                    {
                        if (item == "")
                        {
                            finalList.Remove(item); //σε περίπτωση που υπάρχουν κενά slots που δημιουργήθηκαν παραπάνω, τα αφαιρούμε από τη λίστα
                        }
                    }

                    foreach (var item in finalList)
                    {

                        slots.labID = labsAnnouncements.labID; //ορίζουμε το labID των  slots ως το labID των εργαστηρίων
                        slots.slot = item; //αποθηκεύουμε κάθε slot
                        slots.available = true; //το ορίζουμε ως "δαθέσιμο"
                        db.ExamSlotsAvailability.Add(slots); //και το προσθέτουμε στη βάση δεδομένων
                        db.SaveChanges();

                    }

                    Events ev = new Events();
                    ev.EventID = labNumber;
                    ev.Subject = labsAnnouncements.labTitle;
                    ev.Start = labsAnnouncements.BeginDateTime;
                    ev.End = labsAnnouncements.EndDateTime;
                    ev.RegistrationDeadLine = labsAnnouncements.RegistrationDeadline;
                    ev.labID = labsAnnouncements.labID;
                    ev.subID = labsAnnouncements.subjectExamed; 
                    ev.IsFullDay = false;

                    if(subject.subjectSemester == 1)
                    {
                        ev.ThemeColor = "yellow";
                    }
                    else if (subject.subjectSemester == 2)
                    {
                        ev.ThemeColor = "orange";
                    }
                    else if (subject.subjectSemester == 3)
                    {
                        ev.ThemeColor = "blue";
                    }
                    else if (subject.subjectSemester == 4)
                    {
                        ev.ThemeColor = "green";
                    }
                    else if (subject.subjectSemester == 5)
                    {
                        ev.ThemeColor = "pink";
                    }
                    else if (subject.subjectSemester == 6)
                    {
                        ev.ThemeColor = "purple";
                    }
                    else if (subject.subjectSemester == 7)
                    {
                        ev.ThemeColor = "brown";
                    }
                    else if (subject.subjectSemester == 8)
                    {
                        ev.ThemeColor = "red";
                    }

                    db.Events.Add(ev);

                    LabsAvailability lab = new LabsAvailability();

                    lab.labID = labsAnnouncements.labID;
                    lab.labDate = labDate;
                    lab.labTime = labsAnnouncements.labTime;
                    lab.subjectExamed = labsAnnouncements.subjectExamed;
                    lab.subjectTitle = subject.subjectTitle;
                    lab.professorTeaching = subject.professorTeaching;
                    lab.availableComputers = 20;

                    db.LabsAvailability.Add(lab);

                    db.LabsAnnouncements.Add(labsAnnouncements);

                    db.SaveChanges();

                    return RedirectToAction("SubjectLabsProfessor", new { subjectID = subID }); //ανακατεύθυνση στα εργαστήρια του συγκεκριμένου μαθήματος
                }
                else
                {
                    return View();
                }
            }

        }

        // GET
        [Authorize]
        public ActionResult DeleteSubjectLabProfessor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabsAnnouncements labsAnnouncements = db.LabsAnnouncements.Find(id); //βρίσκουμε το συγκεκριμένο εργαστήριο που θέλουμε να διαγράψουμε
            if (labsAnnouncements == null)
            {
                return HttpNotFound();
            }
            return View(labsAnnouncements);
        }

        // POST
        [Authorize]
        [HttpPost, ActionName("DeleteSubjectLabProfessor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubjectLabProfessorConfirmed(string id)
        {
            LabsAnnouncements labsAnnouncements = db.LabsAnnouncements.Find(id);
            

            List<ExamSlotsAvailability> examSlots = db.ExamSlotsAvailability.ToList(); //δημιουργία αντικειμένου διαθεσιμότητας των slots, προς διαγραφή τους

            List<int> primKeyIDs = new List<int>(); //λίστα για να αποθηκεύσουμε τα primKeyIDs των slots

            foreach (var item in examSlots)
            {
                if (item.labID == id)
                {
                    primKeyIDs.Add(item.primKeyID); //για κάθε slot, αποθηκεύουμε το primKeyID του στη λίστα
                }
            }

            //για κάθε primKeyID, το βρίσκουμε και διαγράφουμε την εγγραφή με τα slots
            foreach (var item in primKeyIDs)
            {
                ExamSlotsAvailability slot = db.ExamSlotsAvailability.Find(item);
                db.ExamSlotsAvailability.Remove(slot);
                db.SaveChanges();
            }


            db.LabsAnnouncements.Remove(labsAnnouncements); //αφαιρούμε το εργαστήριο από τη βάση δεδομένων

            LabsAvailability lab = db.LabsAvailability.Find(id); //βρίσκουμε το id του εργαστηρίου που εμφανίζεται στον διαχειριστή του εργαστηρίου
            db.LabsAvailability.Remove(lab); //και το αφαιρούμε από τη βάση δεδομένων

            //string labNumber = id.Substring(Math.Max(0, id.Length - 1));
           // int eventID = Convert.ToInt32(labNumber);
            List<Events> ev = db.Events.Where(m => m.labID == id).ToList();

            foreach(var item in ev)
            {
                db.Events.Remove(item);
            }

            List<Enrollments> enrollments = db.Enrollments.Where(m => m.labID == id).ToList();

            foreach (var item in enrollments)
            {
                db.Enrollments.Remove(item);
            }


            db.SaveChanges();
            return RedirectToAction("MySubjectsProfessor"); //ανακατεύθυνση στην αρχική σελίδα
        }

        [Authorize]
        public ActionResult SubjectLabsProfessor(string subjectID)
        {
            
            Subjects subject = db.Subjects.Find(subjectID);

            ViewData["subID"] = subjectID;
            string subjectTitle = subject.subjectTitle;
            ViewData["subjectTitle"] = subject.subjectTitle;

            //var userManager = new UnipiUsersManager(new CustomUserStore(db));
            //var user = userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            
            return View(db.LabsAnnouncements.Where(t => t.subjectExamed == subjectID || t.subjectExamed == subjectTitle));

        }

        [Authorize]
        public ActionResult SubjectLabDetailsProfessor(string id, string subjectID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabsAnnouncements labsAnnouncements = db.LabsAnnouncements.Find(id); //βρίσκουμε το συγκεκριμένο εργαστήριο
            if (labsAnnouncements == null)
            {
                return HttpNotFound();
            }
            
            return View(labsAnnouncements); //και εμφανίζουμε τις πληροφορίες του
        }


        // GET
        [Authorize]
        public ActionResult GradeTeamsProfessor(string labID, string subID)
        {
            if (labID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ViewData["id"] = labID;
            ViewData["subID"] = subID;

            //εμφανίζουμε μια λίστα από τα enrollments του συγκεκριμένου εργαστηρίου, χρησιμοποιόντας τη γενική κλάση ListModel που φτιάξαμε για την εμφάνιση αντικειμένων συγκεκριμένου μοντέλου
            var viewModel = new ListModel<Enrollments>(db.Enrollments.Where(m => m.labID == labID).ToList());
            return View(viewModel);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GradeTeamsProfessor(ListModel<Enrollments> viewModel, string subID)
        {
            if (ModelState.IsValid)
            {
                //για κάθε εγγραφή των enrollments, κάνουμε attach το enrollment και ορίζουμε ότι ο βαθμός (grade) έχει τροποποιηθεί

                foreach (var enrollment in viewModel.Items)
                {
                    db.Enrollments.Attach(enrollment);
                    db.Entry(enrollment).Property(x => x.grade).IsModified = true;
                    db.SaveChanges(); //αποθηκεύμε τις αλλαγές στη βάση δεδομένων
                }
                return RedirectToAction("SubjectLabsProfessor", new { subjectID = subID }); //ανακατεύθυνση στα εργαστήρια του συγκεκριμένου μαθήματος
            }
            
            return View(viewModel);
        }

        [Authorize]
        public ActionResult RegisteredStudentsProfessor(string id)
        {
            //προβολή εγγεγραμμένων φοιτητών στο μάθημα

            //ορίζουμε τη λίστα που θα αποθηκευτούν οι εγγεγραμμένοι φοιτητές
            List<string> RegisteredStudents = new List<string>();


            Subjects subject = db.Subjects.Find(id); //βρίσκουμε το id του συγκεκριμένου μαθήματος

            string registeredStudents = subject.regStudent; //βάζουμε σε ένα string το string της βάσης δεδομένων που περιέχει τους εγγεγραμμένους φοιτητές στο μάθημα

            char[] separator = { ',', ' ' }; //ορίζουμε ως διαχωριστικό το ,

            if (registeredStudents != null)
            {
                string[] RegisteredStudentsList = registeredStudents.Split(separator); //διαχωρίζουμε το string με τους φοιτητές χρησιμοποιώντας το κόμμα και τη μέθοδο split 

                //για κάθε εγγεγραμμένο φοιτητή κάνουμε ένα query στη βάση δεδομένων και παίρνουμε το UserName, το Όνομα και το Επίθετό του ώστε να τα εμφανίσουμε λεπτομερώς στον καθηγητή
                foreach (var item in RegisteredStudentsList)
                {
                    var registeredStudent = (from userlist in db.Users
                                             where userlist.UserName == item.ToString()
                                             select new
                                             {
                                                 userlist.Id,
                                                 userlist.UserName,
                                                 userlist.Name,
                                                 userlist.Surname
                                             }).ToList();

                    //για κάθε εγγεγραμμένο φοιτητή, προσθέτουμε διαδοχικά το UserName του, το Όνομά του και το Επίθετό του στη λίστα με τους συνολικούς εγγεγραμμένους φοιτητές
                    RegisteredStudents.Add(registeredStudent.FirstOrDefault().UserName);
                    RegisteredStudents.Add(registeredStudent.FirstOrDefault().Name);
                    RegisteredStudents.Add(registeredStudent.FirstOrDefault().Surname);

                }//και τα βάζουμε σε μια λίστα από strings
            }


            ViewBag.StudentsList = RegisteredStudents; //μεταφέρουμε τη λίστα με τους εγγεγραμμένους φοιτητές στο αντίστοιχο View μέσω ViewBag
            
            return View();
        }

        [Authorize]
        public ActionResult ViewTeamsProfessor(string labID)
        {
            //προβολή ομάδων φοιτητών

            List<string> StudentsTeams = new List<string>(); //δημιουργούμε τη λίστα στην οποία θα προσθέσουμε τα στοιχεία των φοιτητών κάθε ομάδας

            List<Enrollments> enrollmentsList = db.Enrollments.Where(m => m.labID == labID).ToList(); //βρίσκουμε τις εγγραφές που αφορούν το συγκεκριμένο εργαστήριο

            char[] separator = { ',', ' ' }; //ορίζουμε ως διαχωριστικό το κόμμα για να διαχωρίσουμε παρακάτω το string με τα ΑΜ των φοιτητών


            //για κάθε φοιτητή, προσθέτουμε το ΑΜ του σε ένα string που θα διαχωρίσουμε και θα προσθέσουμε στη λίστα TeamMembersList
            foreach (var item in enrollmentsList)
            {
                string teamMembers = item.teamMembers;
                

                string[] TeamMembersList = teamMembers.Split(separator);


                //για κάθε μέλος της εκάστοτε ομάδας, παίρνουμε από τη βάση δεδομένων το ΑΜ, το όνομα και το επίθετό τους και τα προσθέτουμε στην τελική λίστα με τα στοιχεία
                //όλων των φοιτητών ανά ομάδα
                foreach (var member in TeamMembersList)
                {
                    var studentMember = (from userlist in db.Users
                                             where userlist.UserName == member
                                             select new
                                             {
                                                 userlist.Id,
                                                 userlist.UserName,
                                                 userlist.Name,
                                                 userlist.Surname
                                             }).ToList();

                    StudentsTeams.Add(item.selectedExamSlot);
                    StudentsTeams.Add(item.teamID.ToString());
                    StudentsTeams.Add(studentMember.FirstOrDefault().UserName);
                    StudentsTeams.Add(studentMember.FirstOrDefault().Name);
                    StudentsTeams.Add(studentMember.FirstOrDefault().Surname);
                }
            }

            ViewBag.StudentsTeams = StudentsTeams; //μεταφέρουμε τη λίστα των εγγεγραμμένων φοιτητών στο αντίστοιχο view μέσω ViewBag

            return View();
        }

        // GET
        [Authorize]
        public ActionResult EditProfileProfessor(string message)
        {
            ViewBag.Message = message;
            //χρησιμοποιώντας το AspNet Identity της Microsoft, παίρνουμε τα στοιχεία του συνδεδεμενου χρήστη και μεταβαίνει στην αντίστοιχη σελίδα για να επεξεργαστεί το προφίλ του
            var userManager = new UnipiUsersManager(new CustomUserStore(new UnipiLabsDbContext()));
            var userId = User.Identity.GetUserId<int>();
            var user = userManager.FindById(userId);
            return View(user);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfileProfessor([Bind(Include = "Id,PasswordHash,SecurityStamp,UserName,Password,ConfirmPassword,Name,Surname,Email,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                string firstLetter = users.UserName.Substring(0, 1);
                if (firstLetter == "P" && users.Role == "Professor")
                {
                    db.Entry(users).State = EntityState.Modified; //αποθηκεύουμε στη βάση δεδομένων τις αλλαγές που κάναμε
                    db.SaveChanges();
                    return RedirectToAction("MySubjectsProfessor");
                }
                else
                {
                    return RedirectToAction("EditProfileProfessor", "Professors", new { message = "Η ιδιότητά σας ή ο ΑΜ σας είναι λάθος. Προσπαθήστε ξανά." });
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
