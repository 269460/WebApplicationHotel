using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Domain.Models;

namespace HotelBookingApp.Infrastructure.Data
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Number).HasColumnType("VARCHAR(255)");
                entity.Property(e => e.Description).HasColumnType("TEXT");
            });

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=4406;user=repl;password=111;database=mydb;",
                    new MySqlServerVersion(new Version(8, 4, 1)));
            }
        }
    }

}
