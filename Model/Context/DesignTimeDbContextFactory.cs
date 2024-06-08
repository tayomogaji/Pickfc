using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pickfc.Model.Context;

namespace Bristows.BPREM.Model.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PickfcContext>
    {
        private readonly string[] connect = { "Dell", "Azure" };

        public PickfcContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<PickfcContext>();
            builder.EnableSensitiveDataLogging();
            return new PickfcContext(configuration.GetConnectionString(connect[1]));
        }
    }
}
