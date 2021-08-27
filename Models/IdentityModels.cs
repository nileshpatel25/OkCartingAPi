using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace CartingManagmentApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public string name { get; set; }
        public string compnayname { get; set; }
        public string gstin { get; set; }
        public string address { get; set; }
        public string latitude { get; set; }
        public string logitude { get; set; }
        public string address2 { get; set; }
        public string landmark { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string othercontactno { get; set; }
        public string discription { get; set; }
        public DateTime joiningdate { get; set; }
        public bool active { get; set; }
        public string source { get; set; }       
        public string pushTokenId { get; set; }
        public string profilepic { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<vehiclemaster> vehiclemasters { get; set; }
        public DbSet<vehicle> vehicles { get; set; }

        public DbSet<driver> drivers { get; set; }

        public DbSet<customer> customers { get; set; }
        public DbSet<jobwork> jobworks { get; set; }
        public DbSet<jobworkdetail> jobworkdetails { get; set; }

        public DbSet<customerpayment> customerpayments { get; set; }
        public DbSet<driverattendance> driverattendances { get; set; }
        public DbSet<driveradvancesalary> driveradvancesalaries { get; set; }
        public DbSet<SMSLink> SMSLinks { get; set; }
        public DbSet<Appversion> appversions { get; set; }
        public DbSet<fuelmaster> fuelmasters { get; set; }
        public DbSet<LoginStatus> LoginStatuses { get; set; }
        public DbSet<petrolpump> Petrolpumps { get; set; }
        public DbSet<vendorpayment> Vendorpayments { get; set; }

        public DbSet<expensetype> expensetypes { get; set; }
        public DbSet<expensedetails> expensedetails { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}