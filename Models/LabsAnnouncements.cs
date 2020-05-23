using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace UnipiLabs.Models
{

    public class LabsAnnouncements
    {

        [Key]
        [DisplayName("Κωδικός Εργαστηρίου")]
        public string labID { get; set; }

        [DisplayName("Τίτλος Εργαστηρίου")]
        public string labTitle { get; set; }

        [DisplayName("Εξεταζόμενο Μάθημα")]
        public string subjectExamed { get; set; }

        [DisplayName("Ημερομηνία Εργαστηρίου")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime labDate { get; set; }

        [DisplayName("Ώρες εργαστηρίου")]
        public string labTime { get; set; }

        [DisplayName("Έναρξη Εργαστηρίου")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:yyy/MM/dd hh:mm tt}",ApplyFormatInEditMode = false)]
        public DateTime BeginDateTime { get; set; }

        [DisplayName("Λήξη Εργαστηρίου")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]

        public DateTime EndDateTime { get; set; }

        [DisplayName("Προθεσμία Εγγραφής")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]

        public DateTime RegistrationDeadline { get; set; }

        [DisplayName("Χρονική Διάρκεια")]
        public int examSlotsDuration { get; set; }
        public string ExamSlots { get; set; }

        [DisplayName("Εγγεγραμμένοι Φοιτητές")]
        public string regStudents { get; set; }

        [DisplayName("Αριθμός Φοιτητών/Ομάδα")]
        public int numberOfTeamMembers { get; set; }
    }
}