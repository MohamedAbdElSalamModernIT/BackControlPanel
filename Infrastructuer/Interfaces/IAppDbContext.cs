using System.Threading;
using System.Threading.Tasks;
using Common;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Entities.Benaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<T> Set<T>() where T : class;
        ValueTask<EntityEntry<T>> CreateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseEntity;
        EntityEntry<TEntity> Edit<TEntity>(TEntity entity) where TEntity : class;

        DbSet<Place> tblPlaces { get; set; }
        DbSet<Domain.Entities.Benaa.Version> tblVersions { get; set; }
        DbSet<Information> tblInformation { get; set; }
        DbSet<BuildingType> tblBuildingTypes { get; set; }
        DbSet<Condition> tblConditions { get; set; }
        DbSet<ConditionsMap> tblConditionsMap { get; set; }
        DbSet<Baladia> tblAlBaladiat { get; set; }
        DbSet<Area> _tblAreas { get; set; }
        DbSet<Amana> _tblAlamanat { get; set; }


        DbSet<AppUser> AppUsers { get; set; }
        DbSet<Category> tblCategories { get; set; }
        DbSet<Drawing> tblDrawings { get; set; }
        DbSet<DrawingLog> tblDrawingLogs { get; set; }
        DbSet<ConditionResult> tblConditionResults { get; set; }
        DbSet<Client> tblClients { get; set; }

        DbSet<RefreshToken> RefreshToken { get; set; }
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<IDbContextTransaction> CreateTransaction();
        void Commit();
        void Rollback();
    }
}