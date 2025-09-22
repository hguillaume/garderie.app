using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using garderie.app2.Server.Models.Entities;

namespace garderie.app2.Server.Data
{
    // Fix: Inherit only from IdentityDbContext<IdentityUser>, which itself inherits from DbContext.
    //public class ApplicationDbContext : DbContext
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Daycare> Daycares { get; set; }
        public virtual DbSet<Kid> Kids { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
    }
}
