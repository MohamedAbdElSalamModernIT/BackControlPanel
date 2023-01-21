using Domain.Entities;
using Domain.Entities.Benaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            ;

            builder.HasMany(e => e.Logs)
                .WithOne(e=>e.Drwaing).HasForeignKey(e => e.DrwaingId);

            
        }
    }
    
    public class AppFileConfiguration : IEntityTypeConfiguration<AppFile>
    {
        public void Configure(EntityTypeBuilder<AppFile> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();

        }
    }
    
    public class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();

            builder.HasMany(e => e.Drawings)
                .WithOne(e => e.Office).HasForeignKey(e => e.OfficeId);

            builder.HasMany(e => e.Engineers)
               .WithOne(e => e.Office)
               .HasForeignKey(e => e.OfficeId);

            builder.HasOne(e => e.Owner)
                .WithOne(e => e.OwnerOffice)
                .HasForeignKey<Office>(e => e.OwnerId);

        }
    }
}