using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DreamNumbers.Storages.EFCore.SQLServer.DbContexts
{
    internal class DreamNumbersDbContextFactory : IDesignTimeDbContextFactory<DreamNumbersDbContext>
    {
        public DreamNumbersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DreamNumbersDbContext>();
            optionsBuilder.UseAzureSql();

            return new DreamNumbersDbContext(optionsBuilder.Options);
        }
    }
}
