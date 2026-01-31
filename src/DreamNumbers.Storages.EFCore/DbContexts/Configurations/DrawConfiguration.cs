using DreamNumbers.Storages.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamNumbers.Storages.EFCore.DbContexts.Configurations
{
    internal class DrawConfiguration : IEntityTypeConfiguration<Draw>
    {
        public void Configure(EntityTypeBuilder<Draw> builder)
        {
            builder.ToTable("Draws");

            builder.HasKey(d => d.Id);
        }
    }
}
