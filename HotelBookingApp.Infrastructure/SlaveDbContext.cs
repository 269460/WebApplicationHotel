using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Domain.Models;

namespace HotelBookingApp.Infrastructure.Data
{
    public class SlaveDbContext : DbContext
    {
        public SlaveDbContext(DbContextOptions<SlaveDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;Port=5506;Database=mydb;User Id=repl2;Password=111;SslMode=None;AllowPublicKeyRetrieval=True;", 
                    new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }
    }
}