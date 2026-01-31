using DreamNumbers.Storages.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DreamNumbers.Storages.EFCore.SQLite.DbContexts
{
    internal class DreamNumbersDbContext : DreamNumbersDbContext<DreamNumbersDbContext>
    {
        public DreamNumbersDbContext(DbContextOptions<DreamNumbersDbContext> options) : base(options)
        {
        }
    }
}
