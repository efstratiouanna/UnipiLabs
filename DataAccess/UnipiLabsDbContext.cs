using UnipiLabs.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace UnipiLabs.DataAccess
{

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<Users, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(UnipiLabsDbContext context)
            : base(context)
        {
        }
    }

    public class UnipiLabsDbContext : IdentityDbContext<Users, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public UnipiLabsDbContext() : base("UnipiLabsData")
        {
        }

        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<LabsAnnouncements> LabsAnnouncements { get; set; }
        public DbSet<LabsAvailability> LabsAvailability { get; set; }

        public System.Data.Entity.DbSet<UnipiLabs.Models.Enrollments> Enrollments { get; set; }
        public System.Data.Entity.DbSet<UnipiLabs.Models.ExamSlotsAvailability> ExamSlotsAvailability { get; set; }

        public System.Data.Entity.DbSet<UnipiLabs.Models.Events> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Use our custom table name for users instead of the default name
            // the asp.net generates
            modelBuilder.Entity<Users>()
                .ToTable("Users");

            //modelBuilder.Entity<IdentityRole>()
            //    .ToTable("Roles");

            //modelBuilder.Entity<IdentityUserRole>()
            //    .ToTable("UserRoles");

            //modelBuilder.Entity<IdentityUserClaim>()
            //    .ToTable("UserClaims");

            //modelBuilder.Entity<IdentityUserLogin>()
            //    .ToTable("UserLogins");
        }
    }


    public class UnipiUsersManager : UserManager<Users, int>
    {
        public UnipiUsersManager(IUserStore<Users, int> store)
            : base(store)
        {
            var manager = this;
            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<Users, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true
            };
            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
        }

    }


    public class UsersSignInManager: SignInManager<Users, int> {
        public UsersSignInManager(UnipiUsersManager userManager, IAuthenticationManager authenticationManager) :
               base(userManager, authenticationManager)
        { }
    }

}