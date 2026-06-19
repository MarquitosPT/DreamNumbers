using DreamNumbers.Storages.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamNumbers.Storages.EFCore.DbContexts.Configurations
{
    internal class EuroMillionDrawConfiguration : IEntityTypeConfiguration<EuroMillionDraw>
    {
        public void Configure(EntityTypeBuilder<EuroMillionDraw> builder)
        {
            builder.ToTable("EuroMillionDraws");

            builder.HasKey(d => d.Id);

            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.DrawNumber).HasMaxLength(16).IsRequired();
            builder.Property(e => e.Numbers).IsRequired();
            builder.Property(e => e.Stars).IsRequired();
            builder.Property(e => e.ContestNumber).IsRequired();
        }
    }
}
