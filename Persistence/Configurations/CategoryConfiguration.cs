
using Domain.Entities.Benaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.ValueGenerators;

namespace Persistence.Configurations {
    public class CategoryConfiguration : IEntityTypeConfiguration<Category> {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
            .HasValueGenerator<SeqIdValueGenerator>()
            .ValueGeneratedOnAdd();

            builder.HasOne(e => e.ParentCategory)
                  .WithMany(e => e.Categories)
                  .HasForeignKey(e => e.ParentCategoryId);
        }

       
    }
    
    public class ClientConfiguration : IEntityTypeConfiguration<Client> {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.IdentityId);
        }
    }
    
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee> {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(c => c.IdentityId);
        }
    }
}
