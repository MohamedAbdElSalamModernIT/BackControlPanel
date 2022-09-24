using Domain.Entities.Benaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ConditionsMapConfiguration : IEntityTypeConfiguration<ConditionsMap>
    {
        public void Configure(EntityTypeBuilder<ConditionsMap> builder)
        {
            builder.HasKey(e => new
            {
                e.BuildingTypeID,
                e.AlBaladiaID,
                e.ConditionID
            });
            builder.HasOne(e => e.Baladia)
                .WithMany().HasForeignKey(e => e.AlBaladiaID);

        }
    }
}