using Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations {

  public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole> {
    public void Configure(EntityTypeBuilder<UserRole> builder) {
      //builder.HasKey(e => new { e.RoleId, e.UserId });

      builder.Property(e => e.RoleId).HasMaxLength(265);
      builder.Property(e => e.UserId).HasMaxLength(265);
    }
  }
}