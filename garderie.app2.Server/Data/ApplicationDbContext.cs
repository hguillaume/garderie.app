using Microsoft.EntityFrameworkCore;
using garderie.app2.Server.Models.Entities;

namespace garderie.app2.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Daycare> Daycares { get; set; }
        public virtual DbSet<Kid> Kids { get; set; }
    }
}
