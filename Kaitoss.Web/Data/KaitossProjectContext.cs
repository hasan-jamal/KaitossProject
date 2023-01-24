using Kaitoss.Web.Models;
using Kaitoss.Web.Utlity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kaitoss.Web.Data
{
    public partial class KaitossProjectContext : IdentityDbContext<ApplicationUser>
    {
        public KaitossProjectContext()
        {
        }

        public KaitossProjectContext(
            DbContextOptions<KaitossProjectContext> options) : base(options) { }

            public virtual DbSet<Contact> Contacts { get; set; } = null!;
            public virtual DbSet<Goal> Goals { get; set; } = null!;
            public virtual DbSet<Service> Services { get; set; } = null!;
            public virtual DbSet<About> Abouts { get; set; } = null!;
            public virtual DbSet<Information> Informations { get; set; } = null!;
            public virtual DbSet<Blog> Blogs { get; set; } = null!;
         


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().
               HasData(new IdentityRole
               {
                   Name = SD.Role_Admin,
                   NormalizedName = SD.Role_Admin_NormalizedName
               });

            builder.Entity<IdentityUserRole<Guid>>().HasKey(x => new { x.UserId, x.RoleId });
            base.OnModelCreating(builder);
        }


    }
    }

