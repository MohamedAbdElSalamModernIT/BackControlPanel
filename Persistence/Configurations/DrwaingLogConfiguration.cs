using Domain.Entities.Benaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.Configurations
{
    public class DrwaingLogConfiguration : IEntityTypeConfiguration<DrawingLog>
    {
        public void Configure(EntityTypeBuilder<DrawingLog> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();

            builder.HasOne(e => e.Drwaing)
                .WithMany(e => e.Logs).HasForeignKey(e => e.DrwaingId);

            builder.HasMany(e => e.Results)
                .WithOne(e=>e.Log).HasForeignKey(e => e.LogId);
        }
    }
}