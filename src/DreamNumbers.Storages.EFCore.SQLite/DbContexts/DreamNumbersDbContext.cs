using DreamNumbers.Storages.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DreamNumbers.Storages.EFCore.SQLite.DbContexts
{
    public class DreamNumbersDbContext : DreamNumbersDbContext<DreamNumbersDbContext>
    {
        public DreamNumbersDbContext(DbContextOptions<DreamNumbersDbContext> options) : base(options)
        {
        }
    }
}
