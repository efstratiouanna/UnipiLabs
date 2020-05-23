using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using CompareAttribute = System.Web.Mvc.CompareAttribute;
using System.ComponentModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using UnipiLabs.DataAccess;

namespace UnipiLabs.Models
{
    public class Users : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {

        [Required(ErrorMessage = "Παρακαλώ εισάγετε το Username (AM) σας.")]
        [Display(Name = "Username (AM)")]
        public override string UserName { get; set; }

        [Required(ErrorMessage = "Παρακαλώ εισάγετε κωδικό.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(10)]
        [DisplayName("Κωδικός Πρόσβασης")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Οι κωδικοί δεν ταιριάζουν.")]
        [Required(ErrorMessage = "Παρακαλώ επιβεβαιώστε τον κωδικό σας.")]
        [DataType(DataType.Password)]
        [DisplayName("Επιβεβαίωση Κωδικού Πρόσβασης")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Παρακαλώ εισάγετε το όνομά σας.")]
        [DisplayName("Όνομα")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Παρακαλώ εισάγετε το επώνυμό σας.")]
        [DisplayName("Επώνυμο")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Παρακαλώ εισάγετε το email σας.")]

        [EmailAddress(ErrorMessage = "Μη έγκυρη διεύθυνση email.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Παρακαλώ επιλέξτε την ιδιότητά σας.")]
        [DisplayName("Ιδιότητα χρήστη")]
        public string Role { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Users, int> manager) {        

            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

    }

}
