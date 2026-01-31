using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DreamNumbers.Storages.EFCore.SQLite.DbContexts
{
    internal class DreamNumbersDbContextFactory : IDesignTimeDbContextFactory<DreamNumbersDbContext>
    {
        public DreamNumbersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DreamNumbersDbContext>();
            optionsBuilder.UseSqlite();

            return new DreamNumbersDbContext(optionsBuilder.Options);
        }
    }
}
