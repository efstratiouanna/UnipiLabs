using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace UnipiLabs.Models
{
    public class LabsAvailability
    {
        
        [Key]
        [DisplayName("Κωδικός εργαστηρίου:")]
        public string labID { get; set; }

        [DisplayName("Ημερομηνία εργαστηρίου:")]
        public string labDate { get; set; }

        [DisplayName("Ώρες εργαστηρίου:")]
        public string labTime { get; set; }

        [DisplayName("Κωδικός εξεταζόμενου μαθήματος:")]
        public string subjectExamed { get; set; }

        [DisplayName("Τίτλος εξεταζόμενου μαθήματος:")]
        public string subjectTitle { get; set; }

        [DisplayName("Διδάσκων καθηγητής:")]
        public string professorTeaching { get; set; }

        [DisplayName("Αριθμός διαθέσιμων υπολογιστών:")]
        public int availableComputers { get; set; }

        public virtual ICollection<LabsAnnouncements> LabsAnnouncements { get; set; }

    }
}