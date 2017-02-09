using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace MiaBoard.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Display(Name = "Midle Name")]
        [StringLength(255)]
        public string MidleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(255)]
        public string LastName { get; set; }

        public int Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateRegistration { get; set; }

        [Display(Name = "Hired Date")]
        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

        [Display(Name="Contact Number")]
        [StringLength(50)]
        public string ContactNo { get; set; }
           
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Dashlet> Dashlets { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection1New", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}