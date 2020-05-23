using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace UnipiLabs.Models
{
    public class Subjects
    {

        [Key]

        [DisplayName("Κωδικός Μαθήματος")]
        public string subjectID { get; set; }

        [DisplayName("Τίτλος Μαθήματος")]
        public string subjectTitle { get; set; }

        [DisplayName("Εξάμηνο Μαθήματος")]
        public int subjectSemester { get; set; }

        [DisplayName("Περιγραφή Μαθήματος")]
        public string subjectDescription { get; set; }

        [DisplayName("Εγγεγραμμένοι Φοιτητές")]
        public string regStudent { get; set; }

        [DisplayName("Διδάσκοντες")]
        public string professorTeaching { get; set; }


    }
}