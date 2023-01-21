


using Domain.Enums;

namespace Infrastructure.Interfaces
{
    public interface IAuditService
    {
        T CreateEntity<T>(T entity);
        T UpdateEntity<T>(T entity);
        T DeleteEntity<T>(T entity);
        string UserName { get; }
        string UserId { get; }
        string OfficeId { get; }
        string AmanaId { get; }
        string BaladiaId { get; }
        string UserType { get; }
    }
}