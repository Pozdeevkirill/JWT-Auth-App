using AuthApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            Role adminRole = new Role { Id = 1, Name = "admin" };
            Role moderRole = new Role { Id = 2, Name = "moderator" };
            Role userRole = new Role { Id = 3, Name = "user" };

            builder.Entity<Role>().HasData(new Role[] { adminRole, moderRole, userRole });
            base.OnModelCreating(builder);
        }
    }
}
