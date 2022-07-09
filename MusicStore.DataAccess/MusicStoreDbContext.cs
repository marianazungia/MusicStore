using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.DataAccess
{
    public class MusicStoreDbContext : IdentityDbContext<MusicStoreUserIdentity>
    {
        public MusicStoreDbContext()
        {

        }
        public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Genre>()
                .HasData(new List<Genre>
                {
                    new Genre { Id = 1 ,Description = "Rock"},
                    new Genre { Id = 2 ,Description = "Salsa"},
                    new Genre { Id = 3 ,Description = "Reggeaton"},
                });

        }
        
    }
}
