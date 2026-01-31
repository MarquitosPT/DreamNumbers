using DreamNumbers.Storages.EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DreamNumbers.Storages.EFCore.DbContexts
{
    public class DreamNumbersDbContext<TContext> : DbContext where TContext : DbContext
    {
        public DreamNumbersDbContext(DbContextOptions<TContext> options)
            : base(options) { }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DreamNumbersDbContext<TContext>).Assembly);
        }

        internal DbSet<Draw> Draws => Set<Draw>();
    }

}
