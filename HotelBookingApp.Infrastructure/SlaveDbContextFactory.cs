/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HotelBookingApp.Infrastructure.Data
{
    public class SlaveDbContextFactory : IDesignTimeDbContextFactory<SlaveDbContext>
    {
        public SlaveDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SlaveDbContext>();
            var connectionString = configuration.GetConnectionString("SlaveConnection");
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));

            return new SlaveDbContext(optionsBuilder.Options);
        }
    }
}*/