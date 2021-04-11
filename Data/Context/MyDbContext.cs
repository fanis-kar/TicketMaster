using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Context
{
    public class MyDbContext : DbContext
    {
        
        public DbSet<Act> Acts { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Venue> Venues { get; set; }
        
        public MyDbContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseLazyLoadingProxies();
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Act>(e =>
            {
                e.HasKey("Id");
                e.Property(x => x.Id).UseIdentityColumn();
                e.Property(x => x.Id).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            });

            modelBuilder.Entity<Ticket>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property("Id").IsRequired().ValueGeneratedNever();
                e.Property(c => c.Id).HasColumnName("ID").IsRequired();
                //e.Property(x => x.Id).UseIdentityColumn();
                //e.HasOne(x => x.Show).WithMany(x=>x.Tickets);
                e.Property(x => x.Price).HasColumnType("decimal(18,4)");
                //e.Property(x => x.Id).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            });

            modelBuilder.Entity<Show>(e =>
            {
                e.HasKey("Id");
                e.Property(x => x.Id).UseIdentityColumn();
                e.HasOne(x => x.Venue);                
                e.HasOne(x => x.Act);
                e.HasMany(x => x.Tickets);
                e.Property(x => x.Id).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);

            });

            modelBuilder.Entity<Venue>(e =>
            {
                e.HasKey("Id");
                e.Property(x => x.Id).UseIdentityColumn();
                e.Property(x => x.Id).Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
                //e.HasMany(x => x.Shows);               
            });
        }
    }
}
