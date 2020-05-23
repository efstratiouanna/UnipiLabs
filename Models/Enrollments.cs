using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UnipiLabs.Models
{
    public class Enrollments
    {

        [Key]
        public int primKeyID { get; set; }
        
        [DisplayName("Κωδικός μαθήματος")]
        public string subjectID { get; set; }

        [DisplayName("Τίτλος μαθήματος")]
        public string subjectTitle { get; set; }

        [DisplayName("Κωδικός εργαστηρίου")]
        public string labID { get; set; }

        [DisplayName("Τίτλος εργαστηρίου")]
        public string labTitle { get; set; }

        [DisplayName("Ημέρα εργαστηρίου")]
        public string labDate { get; set; }

        [DisplayName("Ώρες εργαστηρίου")]
        public string labTime { get; set; }

        [DisplayName("Επιλεγμένη ώρα")]
        public string selectedExamSlot { get; set; }

        [DisplayName("ID ομάδας")]
        public int teamID { get; set; }

        [DisplayName("Μέλη ομάδας")]
        public string teamMembers { get; set; }

        [DisplayName("Βαθμός")]
        public string grade { get; set; }

        [DisplayName("ΑΜ μέλους ομάδας")]
        public List<string> team { get; set; }
    }
}