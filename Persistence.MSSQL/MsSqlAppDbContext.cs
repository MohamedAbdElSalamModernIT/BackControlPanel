
using Common.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.MSSQL {
  public class MsSqlAppDbContext : AppDbContext {
    public MsSqlAppDbContext(DbContextOptions options, IAuditService auditService=null) : base(options, auditService) {
    }
  }
}
