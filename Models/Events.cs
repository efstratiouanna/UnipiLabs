using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UnipiLabs.Models
{
    public class Events
    {
        [Key]
        public int EventID { get; set; }

        public string labID { get; set; }

        public string subID { get; set; }

        public string Subject { get; set; }

        public DateTime RegistrationDeadLine { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string ThemeColor { get; set; }

        public bool IsFullDay { get; set; }

    }
}