using Domain.Entities.Benaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.Configurations
{
    public class ConditionResultConfiguration : IEntityTypeConfiguration<ConditionResult>
    {
        public void Configure(EntityTypeBuilder<ConditionResult> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();

            builder.HasOne(e => e.Log)
                .WithMany(e=>e.Results).HasForeignKey(e => e.LogId);
        }
    }
}