using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HotelBookingApp.Infrastructure.Data
{
    public class MasterDbContextFactory : IDesignTimeDbContextFactory<MasterDbContext>
    {
        public MasterDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MasterDbContext>();
            var connectionString = configuration.GetConnectionString("MasterConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new MasterDbContext(optionsBuilder.Options);
        }
    }
}