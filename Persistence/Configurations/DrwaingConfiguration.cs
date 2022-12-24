using Domain.Entities.Benaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.Configurations
{
    public class DrwaingConfiguration : IEntityTypeConfiguration<Drawing>
    {
        public void Configure(EntityTypeBuilder<Drawing> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();
            
            builder.Property(c => c.RequestNo)
            .ValueGeneratedOnAdd();

            builder.HasMany(e => e.Logs)
                .WithOne(e=>e.Drwaing).HasForeignKey(e => e.DrwaingId);
        }
    }
}