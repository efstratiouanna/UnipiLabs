using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UnipiLabs.Models
{
    public class ExamSlotsAvailability
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int primKeyID { get; set; }

        [DisplayName("Κωδικός Εργαστηρίου")]
        public string labID { get; set; }

        [DisplayName("Ώρα")]
        public string slot { get; set; }

        public int registeredTeamsNumber { get; set; }

        public bool available { get; set; }

    }
}