using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnipiLabs.Models;

namespace UnipiLabs.DataAccess
{
    public class InitialData : System.Data.Entity.DropCreateDatabaseIfModelChanges<UnipiLabsDbContext>
    {

        //protected override void Seed(UnipiLabs.DataAccess.UnipiLabsData context)
        //{

        //    //List Users
        //    var users = new List<Users>
        //    {
        //    new Users { Id = "S14001", Password = "123", ConfirmPassword = "123", Name = "Γιάννης", Surname = "Παπαδόπουλος",
        //        Email = "papadop@hotmail.com", Role = "Student" },
        //    new Users { Id = "S14002", Password = "123", ConfirmPassword = "123", Name = "Αντώνης", Surname = "Αντωνίου",
        //        Email = "antoniou@hotmail.com", Role = "Student" },
        //    new Users { Id = "P01002", Password = "123", ConfirmPassword = "123", Name = "Μαρία", Surname = "Βίρβου",
        //        Email = "virvou@unipi.gr", Role = "Professor" },
        //    new Users { Id = "L01001", Password = "123", ConfirmPassword = "123", Name = "Κώστας", Surname = "Γκρέκας",
        //        Email = "cgrekas@gmail.com", Role = "LabAdmin" },

        //     };
        //    users.ForEach(d => context.Users.Add(d));
        //    context.SaveChanges();

        //    //List Students
        //    var students = new List<Students>
        //    {
        //    new Students { ID = "S14001", subjRegisteredID = "ΠΛΗ6-1" },
        //    new Students { ID = "S14002", subjRegisteredID = "ΠΛΗ8-1", examsRegistered = "LAB1", teamID = 1, teamMembers = "S14002,S14044" },
        //    new Students { ID = "S14044", subjRegisteredID = "ΠΛΗ8-1", examsRegistered = "LAB1", teamID = 1, teamMembers = "S14002,S14044" }
        //     };
        //    students.ForEach(d => context.Students.Add(d));
        //    context.SaveChanges();

        //    //List Professors
        //    var professors = new List<Professors>
        //    {
        //    new Professors { ID = "P01001", subjectID = "ΠΛΗ8-1", subjectTitle ="Εκπαιδευτικό Λογισμικό" },
        //    new Professors { ID = "P01002", subjectID = "ΠΛΗ6-1", subjectTitle ="Αλληλεπίδραση Ανθρώπου και Υπολογιστή" }
        //    };
        //    professors.ForEach(d => context.Professors.Add(d));
        //    context.SaveChanges();

        //    //List Subjects
        //    var subjects = new List<Subjects>
        //    {
        //    new Subjects { subjectID = "ΠΛΗ8-1", subjectTitle ="Εκπαιδευτικό Λογισμικό", subjectDescription = "Το μάθημα έχει εργασία.",
        //    regStudent = "S14044", professorTeaching = "P01001"},
        //    new Subjects { subjectID = "ΠΛΗ8-1", subjectTitle ="Εκπαιδευτικό Λογισμικό", subjectDescription = "Το μάθημα έχει εργασία.",
        //    regStudent = "S14002", professorTeaching = "P01001"},
        //    new Subjects { subjectID = "ΠΛΗ6-1", subjectTitle ="Αλληλεπίδραση Ανθρώπου και Υπολογιστή", subjectDescription = "Το μάθημα έχει εργασία.",
        //    regStudent = "S14001", professorTeaching = "P01002"}
        //    };
        //    subjects.ForEach(d => context.Subjects.Add(d));
        //    context.SaveChanges();

        //    //List LabsAnnouncements
        //    /*var labsAnnouncements = new List<LabsAnnouncements>
        //    {
        //    new LabsAnnouncements { labID = "LAB1", subjectExamed = "ΠΛΗ8-1", Date = DateTime.ParseExact("01/03/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture), 
        //        beginTime = DateTime.ParseExact("11:00:00", "hh:mm:ss", CultureInfo.InvariantCulture),
        //    endTime = DateTime.ParseExact("14:00:00", "hh:mm:ss", CultureInfo.InvariantCulture), examSlotsDuration = 20,
        //    regStudent = "S14002", teamID = 1},
        //    new LabsAnnouncements { labID = "LAB1", subjectExamed = "ΠΛΗ8-1", Date = DateTime.ParseExact("01/03/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //        beginTime = DateTime.ParseExact("11:00:00", "hh:mm:ss", CultureInfo.InvariantCulture),
        //    endTime = DateTime.ParseExact("14:00:00", "hh:mm:ss", CultureInfo.InvariantCulture), examSlotsDuration = 20,
        //    regStudent = "S14044", teamID = 1}
        //    };
        //    labsAnnouncements.ForEach(d => context.LabsAnnouncements.AddOrUpdate());
        //    context.SaveChanges();

        //    //List LabsAvailability
        //    var labsAvailability = new List<LabsAvailability>
        //    {
        //    new LabsAvailability { Datetime = DateTime.ParseExact("01/03/2020 11:00:00", "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture),
        //    availableComputers = 20, regTeamsNumber = 1}
        //    };
        //    labsAvailability.ForEach(d => context.LabsAvailability.AddOrUpdate());
        //    context.SaveChanges();*/

        //}
    }
}